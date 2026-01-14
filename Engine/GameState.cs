
using System.Text.Json.Serialization;
using RPGFramework.Enums;
using RPGFramework.Combat;
using RPGFramework.Core;
using RPGFramework.Geography;
using RPGFramework.Persistence;
using System.Text.Json.Serialization;


namespace RPGFramework
{
    /// <summary>
    /// Represents the global state and core management logic for the game server, 
    /// including loaded areas, players, game time, and server lifecycle.
    /// </summary>
    /// <remarks><para> <b>Singleton Access:</b> Use the <see cref="Instance"/> property 
    /// to access the single <see cref="GameState"/> instance throughout the application. </para>
    /// <para> <b>Persistence:</b> The <see cref="Persistence"/> property determines how 
    /// game data is loaded and saved. By default, a JSON-based persistence
    /// mechanism is used, but this can be replaced with a custom implementation. </para> 
    internal sealed class GameState
    {
        // Static Fields and Properties
        private static readonly Lazy<GameState> _instance = new(() => new GameState());

        public static GameState Instance => _instance.Value;

        // The persistence mechanism to use. Default is JSON-based persistence.
        public static IGamePersistence Persistence { get; set; } = new JsonGamePersistence();

        public bool IsRunning { get; private set; } = false;

        #region --- Fields ---
        private CancellationTokenSource? _saveCts;
        private Task? _saveTask;
        private CancellationTokenSource? _timeOfDayCts;
        private Task? _timeOfDayTask;
        private CancellationTokenSource? _tickCts;
        private Task? _tickTask;
        private CancellationTokenSource? _weatherCts;
        private Task? _weatherTask;
        private CancellationTokenSource? _npcCts;
        private Task? _npcTask;
        private CancellationTokenSource? _itemDecayCts;
        private Task? _itemDecayTask;
        private CancellationTokenSource? _announcementsCts;
        private Task? _announcementsTask;
        #endregion

        #region --- Properties ---

        /// <summary>
        /// All Areas are loaded into this dictionary
        /// </summary>
        [JsonIgnore] public Dictionary<int, Area> Areas { get; set; } =
            new Dictionary<int, Area>();

        // TODO: Move this to configuration settings class
        public DebugLevel DebugLevel { get; set; } = DebugLevel.Debug;

        /// <summary>
        /// The date of the game world. This is used for time of day, etc.
        /// </summary>
        public DateTime GameDate { get; set; } = new DateTime(2021, 1, 1);
        public DateTime ServerStartTime { get; init; } 

        /// <summary>
        /// Gets or sets the collection of help entries, indexed by their name (must be unique).
        /// </summary>
        [JsonIgnore] public Dictionary<string, HelpEntry> HelpEntries { get; set; } = new Dictionary<string, HelpEntry>();

        /// <summary>
        /// All Players are loaded into this dictionary, with the player's name as the key 
        /// </summary>
        [JsonIgnore] public Dictionary<string, Player> Players { get; set; } = new Dictionary<string, Player>();

        [JsonIgnore] public Random Random { get; } = new Random();
        public int StartAreaId { get; set; } = 0;
        public int StartRoomId { get; set; } = 0;

        public TelnetServer? TelnetServer { get; private set; }

        #endregion --- Properties ---
        // Relocate later
        public List<CombatObject> Combats = new List<CombatObject>();
        #region --- Methods ---
        private GameState()
        {
            ServerStartTime = DateTime.Now;
        }

        public void AddPlayer(Player player)
        {
            Players.Add(player.Name, player);
        }

        public List <Player> GetPlayersOnline()
        {
            return Players.Values.Where(o => o.IsOnline).ToList();
        }

        // CODE REVIEW: Aiden (PR #13)
        // I moved Trim around a little for readability and renamed to 
        // GetPlayerByName since DisplayName means something different.
        // It would probably be more efficient to use GetPlayersOnline as a starting point.
        // That way you wouldn't have to check for online and you could automatically skip over
        // all offline players. Once you read this and update (or not) you can feel free to
        // remove these notes.
        public Player? GetPlayerByName(string name)
        {
            name = name.Trim();

            if (string.IsNullOrWhiteSpace(name))
                return null;

            foreach (var player in Players.Values)
            {
                if (!player.IsOnline)
                    continue;

                if (string.Equals(player.Name,
                                  name,
                                  StringComparison.OrdinalIgnoreCase))
                {
                    return player;
                }
            }

            return null;
        }

        /// <summary>
        /// This would be used by an admin command to load an area on demand. 
        /// For now useful primarily for reloading externally crearted changes
        /// </summary>
        /// <param name="areaName"></param>
        private Task LoadArea(string areaName)
        {
            Area? area = GameState.Persistence.LoadAreaAsync(areaName).Result;
            if (area != null)
            {
                if (Areas.ContainsKey(area.Id))
                    Areas[area.Id] = area;
                else
                    Areas.Add(area.Id, area);

                GameState.Log(DebugLevel.Alert, $"Area '{area.Name}' loaded successfully.");
            }

            return Task.CompletedTask;
        }

        // Load all Area files from /data/areas. Each Area file will contain some
        // basic info and lists of rooms and exits.
        private async Task LoadAllAreas()
        {
            Areas.Clear();

            var loaded = await Persistence.LoadAreasAsync();
            foreach (var kvp in loaded)
            {
                Areas.Add(kvp.Key, kvp.Value);
                GameState.Log(DebugLevel.Alert, $"Area '{kvp.Value.Name}' loaded successfully.");
            }
        }

        private int TickCount = 0;
        private Task TickTask(int interval)
        {
            while (IsRunning)
            {
                // Update game state here
                // For example, process NPC actions, environment changes, etc.
                TickCount++;
                Thread.Sleep(interval);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Loads all player data from persistent storage and adds each player 
        /// to the <see cref="Players"/> collection.
        /// </summary>
        /// <remarks>This method loads all player objects from the data source and 
        /// populates the <see cref="Players"/> dictionary using each player's name 
        /// as the key. Existing entries in <see cref="Players"/>
        /// are not cleared before loading; newly loaded players are added or 
        /// overwrite existing entries with the same name.</remarks>
        private async Task LoadAllPlayers()
        {
            Players.Clear();

            var loaded = await Persistence.LoadPlayersAsync();
            foreach (var kvp in loaded)
            {
                Players.Add(kvp.Key, kvp.Value);
                GameState.Log(DebugLevel.Debug, $"Player '{kvp.Value.Name}' loaded successfully.");
            }

            GameState.Log(DebugLevel.Alert, $"{Players.Count} players loaded.");
        }

        private async Task LoadCatalogs()
        {
            HelpEntries.Clear();
            try
            {
                HelpEntries = await Persistence.LoadHelpCatalogAsync();
                GameState.Log(DebugLevel.Alert, $"Help catalog loaded with {HelpEntries.Count} entries.");
            }
            catch ( FileNotFoundException fex)
            {
                GameState.Log(DebugLevel.Warning, $"Help catalog file not found. Loading blank.");
            }
        }

        /// <summary>
        /// Saves all area entities asynchronously to the persistent storage.
        /// </summary>
        /// <remarks>This method initiates an asynchronous operation to persist 
        /// the current set of areas. The save operation is performed for all 
        /// areas contained in the collection at the time of invocation.</remarks>
        /// <returns>A <see cref="Task"/> that represents the asynchronous save operation.</returns>
        private Task SaveAllAreas()
        {
            return Persistence.SaveAreasAsync(Areas.Values);
        }

        /// <summary>
        /// Saves all player data asynchronously.
        /// </summary>
        /// <param name="includeOffline"><see langword="true"/> to include offline 
        /// players in the save operation; otherwise, only online players are saved.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        public Task SaveAllPlayers(bool includeOffline = false)
        {
            var toSave = includeOffline
                ? Players.Values
                : Players.Values.Where(p => p.IsOnline);

            return Persistence.SavePlayersAsync(toSave);
        }

        public Task SaveCatalogs()
        {
            return Persistence.SaveHelpCatalog(HelpEntries);
        }

        /// <summary>
        /// Saves the specified player to persistent storage asynchronously.
        /// </summary>
        /// <param name="p">The <see cref="Player"/> instance to be saved. Cannot be <c>null</c>.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        public Task SavePlayer(Player p)
        {
            return Persistence.SavePlayerAsync(p);
        }

        /// <summary>
        /// Initializes and starts the game server 
        ///   loading all areas
        ///   loading all players
        ///   starting the Telnet server
        ///   launching background threads for periodic tasks.
        /// </summary>
        /// <remarks>This method must be called before the server can accept 
        /// Telnet connections or process game logic. It loads all required game 
        /// data and starts background threads for saving state and updating the
        /// time of day.</remarks>
        /// <returns>A task that represents the asynchronous start operation.</returns>
        public async Task Start()
        {
            // Prevent multiple starts (TODO: Add restart / stop functionality)
            if (IsRunning)
                throw new InvalidOperationException("Game server is already running.");

            IsRunning = true;

            // Initialize game data if it doesn't exist
            await Persistence.EnsureInitializedAsync(new GamePersistenceInitializationOptions());

            await LoadAllAreas();
            await LoadAllPlayers();
            await LoadCatalogs();

            // Load Item (Weapon/Armor/Consumable/General) catalogs
            // Load NPC (Mobs/Shop/Guild/Quest) catalogs



            // TODO: Consider moving thread methods to their own class

            // Start threads that run periodically
            _saveCts = new CancellationTokenSource();
            _saveTask = RunAutosaveLoopAsync(TimeSpan.FromMilliseconds(10000), _saveCts.Token);

            _timeOfDayCts = new CancellationTokenSource();
            _timeOfDayTask = RunTimeOfDayLoopAsync(TimeSpan.FromMilliseconds(15000), _timeOfDayCts.Token);

            // Other threads will go here
            _tickThread = new Thread(() => TickTask(1000));
            _tickThread.IsBackground = true;
            _tickThread.Start();
            // Weather?
            _weatherThread = new Thread(() => WeatherTask(60000));
            _weatherThread.IsBackground = true;
            _weatherThread.Start();
            // Area threads?
            // NPC threads?
            _npcThread = new Thread(() => NPCTask(10000));
            _npcThread.IsBackground = true;
            _npcThread.Start();
            // Room threads?
            _itemDecayThread = new Thread(() => ItemDecayTask(120000));
            _itemDecayThread.IsBackground = true;
            _itemDecayThread.Start();
            _anouncmentsThread = new Thread(() => AnnouncementsTask(300000));
            _anouncmentsThread.IsBackground = true;
            _anouncmentsThread.Start();

            // This needs to be last
            this.TelnetServer = new TelnetServer(5555);
            await this.TelnetServer.StartAsync();

        }

        /// <summary>
        /// Stops the server, saving all player and area data, disconnecting online players, 
        /// and terminating the application.
        /// </summary>
        /// <remarks>This method performs a graceful shutdown by persisting all relevant data, 
        /// logging out online users, stopping background threads, and closing network 
        /// connections before exiting the process.</remarks>
        /// <returns>A task that represents the asynchronous stop operation. The application 
        /// will terminate upon completion.</returns>
        /// TODO: Allow user to supply a duration to avoid immediate shutdown
        public async Task Stop()
        {
            await SaveAllPlayers(includeOffline: true);
            await SaveAllAreas();

            foreach (var player in Players.Values.Where(p => p.IsOnline))
            {
                player.Logout();
            }

            this.TelnetServer!.Stop();

            // Signal threads to stop
            IsRunning = false;

            // Wait for threads to finish
            _saveCts?.Cancel();
            _timeOfDayCts?.Cancel();

            // Exit program
            Environment.Exit(0);
        }

        #endregion --- Methods ---

        #region --- Static Methods ---
        internal static void Log(DebugLevel level, string message)
        {
            if (level <= GameState.Instance.DebugLevel)
            {
                Console.WriteLine($"[{level}] {message}");
            }
        }

        #endregion

        #region --- Thread Methods ---
        /// <summary>
        /// Things that need to be saved periodically
        /// </summary>
        /// <param name="interval"></param>
        private async Task RunAutosaveLoopAsync(TimeSpan interval, CancellationToken ct)
        {
            GameState.Log(DebugLevel.Alert, "Autosave thread started.");
            while (!ct.IsCancellationRequested && IsRunning)
            {
                try
                {
                    await SaveAllPlayers();
                    await SaveAllAreas();
                    await SaveCatalogs();

                    GameState.Log(DebugLevel.Info, "Autosave complete.");
                }
                catch (Exception ex)
                {
                    GameState.Log(DebugLevel.Error, $"Error during autosave: {ex.Message}");
                }

                await Task.Delay(interval, ct);
            }

            GameState.Log(DebugLevel.Alert, "Autosave thread stopping.");
        }

        /// <summary>
        /// Update the time periodically.
        /// We might want to create game variables that indicate how often this should run
        /// and how much time should pass each time. For now it adds 1 hour / minute.
        /// </summary>
        /// <param name="interval"></param>
        private async Task RunTimeOfDayLoopAsync(TimeSpan interval, CancellationToken ct)
        {
            GameState.Log(DebugLevel.Alert, "Time of Day thread started.");
            while (!ct.IsCancellationRequested && IsRunning)
            {
                try
                {
                    GameState.Log(DebugLevel.Debug, "Updating time...");
                    double hours = interval.TotalMinutes * 60;
                    GameState.Instance.GameDate = GameState.Instance.GameDate.AddHours(hours);
                }
                catch (Exception ex)
                {
                    GameState.Log(DebugLevel.Error, $"Error during time update: {ex.Message}");
                }

                await Task.Delay(interval, ct);
            }
            GameState.Log(DebugLevel.Alert, "Time of Day thread stopping.");
        }

        private void AnnouncementsTask(int interval)
        {
            while (IsRunning)
            {
                // Check for announcements to make
                // Announce to all players in relevant areas/rooms
                Thread.Sleep(interval);
            }
        }

        private void ItemDecayTask(int interval)
        {
            while (IsRunning)
            {
                foreach (var item in GameState.Instance.Items.Values){

                    if (item.IsPerishable)
                    {
                        item.UsesRemaining--;
                    }
                }
            }
        }

        private void WeatherTask(int interval)
        {
            while (IsRunning)
            {
                // Update weather in all areas
                //choose random from list, apply to area
                //repeat for every area
                //await build team for areas/weather types
                foreach (var area in Areas.Values)
                {
                    area.UpdateWeather();
                }
                Thread.Sleep(interval);
            }
        }

        // / <summary> weather update method, move later
        // / </summary>
        public void UpdateWeather()
        {
            // choose random from list, apply to area
            // repeat for every area
            int randomWeatherIndex = new Random().Next(0, weatherStates.Count - 1);
            string newWeather = weatherStates[randomWeatherIndex];
            // apply newWeather to area
            // decide on how weather effects things like combat, npcs, visibility, movement, etc.
            // figure out how to implement those effects later, probably within combat and npc methods
        }
        //placeholder weather states, await build teams final choices
        List<string> weatherStates = new List<string>()
        {
            "Sunny",
            "Cloudy",
            "Rainy",
            "Stormy",
            "Snowy",
            "Windy"
        };
        // end weather update method
        private void NPCTask(int interval)
        {
            while (IsRunning)
            {
                // run command to check for players, then choose NPC actions
                // hostile/agro prioritize attacking players, then other NPCs, then wandering
                // friendly prioritize helping players, then other NPCs, then wandering
                // neutral prioritize wandering, then attacking hostile NPCs
                // repeat for every NPC
                // await NPC team for behaviors/actions and NPCs
                // make sure to include some randomness so NPCs don't all act at once
                // don't allow for NPCs to leave tasks when engaged (e.g., attacking, helping)
                foreach (var npc in GameState.Instance.NonPlayers)
                {
                    if (npc.IsEngaged)
                    {
                        continue;
                    }
                    if (npc is Hostile)
                    {
                        
                        if (npc.GetRoom().Players.Count > 0)
                        {
                            List<Player> potentialTargets = new List<Player>();
                            // run combat initializtion method(s)
                            foreach (var player in npc.GetRoom().Players)
                            {
                                // notify player of attack
                                if (player.IsEngaged)
                                {
                                    continue;
                                }
                                potentialTargets.Add(player);
                            }
                            if (potentialTargets.Count > 0)
                            {
                                var target = potentialTargets[new Random().Next(0, potentialTargets.Count-1)];
                                // notify player of attack
                                target.WriteLine($"The {npc.Name} attacks you!");
                                // run combat initialization method(s)
                                CombatObject combat = new CombatObject();
                                Combats.Add(combat);
                                combat.CombatInitialization(npc, target, combat);
                            }
                            else if (npc.GetRoom().NPC.Count > 1)
                        {
                                List<NonPlayer> potentialNpcTargets = new List<NonPlayer>();
                                
                                foreach (var otherNpc in npc.GetRoom().GetCharacters())
                                {
                                if (otherNpc == npc || otherNpc.IsEngaged)
                                {
                                    continue;
                                }
                                    else
                                    {
                                        potentialNpcTargets.Add(otherNpc);
                                    }
                                }
                                var target = potentialTargets[new Random().Next(0, potentialNpcTargets.Count - 1)];
                                CombatObject combat = new CombatObject();
                                Combats.Add(combat);
                                combat.CombatInitialization(npc, target, combat);
                            }

                    }
                    else if (npc is Army)
                    {
                        if (npc.GetRoom().NPC.Hostile.Count > 0)
                        {
                            foreach (var player in npc.GetRoom().Players)
                            {
                                // notify player of attack
                                player.WriteLine($"The {npc.Name} attacks an enemy!");
                            }
                            // run combat initialization method(s)
                        }
                        else
                        {
                            npc.DoGuardAction();
                        }

                    }
                    else if (npc is NonHostile)
                    {
                        if (npc.GetRoom().Players.Count > 0)
                        {
                            npc.DoNPCAction();
                        }
                    }
                        Thread.Sleep(interval);
                }
            }

            #endregion --- Thread Methods ---

        }
    }
}

