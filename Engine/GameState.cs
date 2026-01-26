
using System.Text.Json.Serialization;
using RPGFramework.Enums;
using RPGFramework.Combat;
using RPGFramework.Workflows;
using RPGFramework.Core;
using RPGFramework.Geography;
using RPGFramework.Persistence;
using RPGFramework.Interfaces;

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
    /// TODO: Given the number of properties that are not serialized, we should consider making a GameStateData DTO.
    internal sealed class GameState
    {
        // Static Fields and Properties
        private static readonly Lazy<GameState> _instance = new(() => new GameState());

        public static GameState Instance => _instance.Value;

        // The persistence mechanism to use. Default is JSON-based persistence.
        public static IGamePersistence Persistence { get; set; } = new JsonGamePersistence();

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
        private CancellationTokenSource? _combatManagerCts;
        private Task? _combatManagerTask;

        private int _tickCount = 0;
        #endregion

        #region --- Properties ---
        // TODO: Move this to configuration settings class
        public DebugLevel DebugLevel { get; set; } = DebugLevel.Debug;
        /// <summary>
        /// The date of the game world. This is used for time of day, etc.
        /// </summary>
        public DateTime GameDate { get; set; } = new DateTime(2021, 1, 1);

        public int StartAreaId { get; set; } = 0;
        public int StartRoomId { get; set; } = 0;

        #region --- Unserialized Properties ---
        [JsonIgnore] public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// All Areas are loaded into this dictionary
        /// </summary>
        [JsonIgnore] public Dictionary<int, Area> Areas { get; set; } = [];

        // Relocate later
        [JsonIgnore] public List<CombatWorkflow> Combats = new List<CombatWorkflow>();

        [JsonIgnore] public DateTime ServerStartTime { get; private set; }
        
        /// <summary>
        /// All Players are loaded into this dictionary, with the player's name as the key 
        /// </summary>
        [JsonIgnore] public Dictionary<string, Player> Players { get; set; } = [];

        [JsonIgnore] public Random Random { get; } = new Random();
        [JsonIgnore] public TelnetServer? TelnetServer { get; private set; }

        #region --- Catalogs ---        
        [JsonIgnore] public List<ICatalog> Catalogs { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of help entries, indexed by their name (must be unique).
        /// </summary>
        [JsonIgnore] public Catalog<string, HelpEntry> HelpCatalog { get; set; } = [];
        [JsonIgnore] public Catalog<string, Mob> MobCatalog { get; set; } = [];
        [JsonIgnore] public Catalog<string, NonPlayer> NPCCatalog { get; set; } = [];

        #endregion --- Catalogs ---

        #endregion --- Unserialized Properties ---
        #endregion --- Properties ---
        
        #region --- Methods ---
        private GameState()
        {
            Catalogs.Add(HelpCatalog);
            Catalogs.Add(MobCatalog);
            Catalogs.Add(NPCCatalog);
        }

        public void AddPlayer(Player player)
        {
            Players.Add(player.Name, player);
        }

        public List<Player> GetPlayersOnline()
        {
            return Players.Values.Where(o => o.IsOnline).ToList();
        }

        #region GetPlayerByName Method
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
        #endregion

        #region LoadArea Methods
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
        #endregion

        #region LoadAllPlayers Method
        /// <summary>
        /// Loads all player data from persistent storage and adds each player 
        /// to the <see cref="Players"/> collection.
        /// </summary>
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
        #endregion

        #region LoadCatalogs Method
        /// <summary>
        /// Loop through all catalogs and load them. Each catalog
        /// should be added to the Catalogs list during initialization.
        /// </summary>
        /// <returns></returns>
        private async Task LoadCatalogs()
        {
            foreach (ICatalog catalog in Catalogs)
            {
                await catalog.LoadCatalogAsync();
            }
        }
        #endregion

        #region SaveAllAreas Method
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
        #endregion

        #region SaveAllPlayers Method
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
        #endregion

        #region SaveCatalogsAsync Method
        public async Task SaveCatalogsAsync()
        {
            foreach (ICatalog catalog in Catalogs)
            {
                catalog.SaveCatalogAsync().Wait();
            }            
        }
        #endregion

        #region SavePlayer Method
        /// <summary>
        /// Saves the specified player to persistent storage asynchronously.
        /// </summary>
        /// <param name="p">The <see cref="Player"/> instance to be saved. Cannot be <c>null</c>.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        public Task SavePlayer(Player p)
        {
            return Persistence.SavePlayerAsync(p);
        }
        #endregion

        #region Start Method (Async)
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
            ServerStartTime = DateTime.Now;

            // Initialize game data if it doesn't exist
            await Persistence.EnsureInitializedAsync(new GamePersistenceInitializationOptions());

            await LoadAllAreas();
            await LoadAllPlayers();
            await LoadCatalogs();

            // TODO: Consider moving thread methods to their own class

            // Start threads that run periodically
            _announcementsCts = new CancellationTokenSource();
            _announcementsTask = RunAnnouncementsLoopAsync(TimeSpan.FromSeconds(30), _announcementsCts.Token);

            _combatManagerCts = new CancellationTokenSource();
            _combatManagerTask = RunCombatManagerLoopAsync(TimeSpan.FromSeconds(1), _combatManagerCts.Token);

            _itemDecayCts = new CancellationTokenSource();
            _itemDecayTask = RunItemDecayLoopAsync(TimeSpan.FromMinutes(2), _itemDecayCts.Token);

            _npcCts = new CancellationTokenSource();
            _npcTask = RunNPCLoopAsync(TimeSpan.FromSeconds(10), _npcCts.Token);

            _saveCts = new CancellationTokenSource();
            _saveTask = RunAutosaveLoopAsync(TimeSpan.FromMilliseconds(10000), _saveCts.Token);

            _timeOfDayCts = new CancellationTokenSource();
            _timeOfDayTask = RunTimeOfDayLoopAsync(TimeSpan.FromMilliseconds(15000), _timeOfDayCts.Token);

            // Other threads will go here
            _tickCts = new CancellationTokenSource();
            _tickTask = RunTickLoopAsync(TimeSpan.FromSeconds(1), _tickCts.Token);

            _weatherCts = new CancellationTokenSource();
            _weatherTask = RunWeatherLoopAsync(TimeSpan.FromMinutes(1), _weatherCts.Token);

            // This needs to be last
            this.TelnetServer = new TelnetServer(5555);
            await this.TelnetServer.StartAsync();
        }
        #endregion

        #region Stop Method (Async)
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
            await SaveCatalogsAsync();

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
            _tickCts?.Cancel();
            _weatherCts?.Cancel();
            _npcCts?.Cancel();
            _itemDecayCts?.Cancel();
            _announcementsCts?.Cancel();
            _combatManagerCts?.Cancel();
            // Exit program
            Environment.Exit(0);
        }
        #endregion

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
                    await SaveCatalogsAsync();

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

        private async Task RunAnnouncementsLoopAsync(TimeSpan interval, CancellationToken ct)
        {
            GameState.Log(DebugLevel.Alert, "Announcements thread started.");
            while (!ct.IsCancellationRequested && IsRunning)
            {
                try
                {
                    GameState.Log(DebugLevel.Debug, "Announcing things...");
                    // Do the actual work
                }
                catch (Exception ex)
                {
                    GameState.Log(DebugLevel.Error, $"Error during announcements: {ex.Message}");
                }

                await Task.Delay(interval, ct);
            }
            GameState.Log(DebugLevel.Alert, "Announcements thread stopping.");
        }

        private async Task RunCombatManagerLoopAsync(TimeSpan interval, CancellationToken ct)
        {
            GameState.Log(DebugLevel.Alert, "Combat Manager thread started.");
            while (!ct.IsCancellationRequested && IsRunning)
            {
                try
                {
                    GameState.Log(DebugLevel.Debug, "Managing combats...");
                    foreach (CombatWorkflow combat in Combats)
                    {
                        // remove dead combatants
                        foreach (Character combatant in combat.Combatants)
                        {
                            if (!combatant.Alive)
                            {
                                combat.Combatants.Remove(combatant);
                            }
                        }
                        // lists of factions
                        // check if they have any characters in them, then increase the number of active factions 
                        // based on that
                        int activeFactions = 0;
                        if (combat.Elf.Count > 0)
                            activeFactions++;
                        if (combat.Bandit.Count > 0)
                            activeFactions++;
                        if (combat.Monster.Count > 0)
                            activeFactions++;
                        if (combat.Construct.Count > 0)
                            activeFactions++;
                        if (combat.Army.Count > 0)
                            activeFactions++;
                        // miscellaneous is a list that contains creatures (like wolves based on npc team input)
                        // and players that makes it so that any characters in that list (npc's in particular)
                        // can attack anyone in the combat while also allowing for us to keep track of how many 
                        // creatures should/would still be fighting
                        if ((activeFactions <= 1 && combat.Miscellaneous.Count <= 0) || (activeFactions <= 0 && combat.Miscellaneous.Count <= 1))
                        {
                            // end combat if there is no more opposing characters
                            foreach (Character c in combat.Combatants)
                            {
                                // removes the current workflow from every character and removes them before deleting the combat object
                                c.CurrentWorkflow = null;
                                combat.Combatants.Remove(c);
                            }
                            Combats.Remove(combat);
                        }
                        // at the start of combat assign first active combatant based on initiative order
                        if (combat.ActiveCombatant == null)
                            combat.ActiveCombatant = combat.Combatants[0];
                        // run npc turn if npc, otherwise wait for 30 seconds to pass for player turns
                        if (combat.ActiveCombatant is NonPlayer npc)
                        {
                            NonPlayer.TakeTurn(npc, combat);
                            combat.TurnTimer++;
                            continue;
                        }
                        else
                        {
                            if (combat.PreviousActingCharacter != null)
                            {

                                if (combat.PreviousActingCharacter == combat.ActiveCombatant)
                                {
                                    // update timer for player turns if it is the same player as the last run of this task
                                    combat.TurnTimer++;
                                    if (combat.TurnTimer >= 30)
                                    {
                                        // end player turn if 30 seconds have passed
                                        int indexOfNextCombatant = combat.Combatants.IndexOf(combat.ActiveCombatant) + 1;
                                        if (indexOfNextCombatant > combat.Combatants.Count - 1)
                                            indexOfNextCombatant = 0;
                                        combat.ActiveCombatant = combat.Combatants[indexOfNextCombatant];
                                    }
                                    else if (combat.PreviousActingCharacter != combat.ActiveCombatant)
                                    {
                                        // update so that new player gets full turn time
                                        combat.PreviousActingCharacter = combat.ActiveCombatant;
                                        combat.TurnTimer = 1;
                                    }
                                }

                            }
                        }
                    }
                    
                        
                }
                catch (Exception ex)
                {
                    GameState.Log(DebugLevel.Error, $"Error during combat management: {ex.Message}");
                }
                await Task.Delay(interval, ct);
            }
            GameState.Log(DebugLevel.Alert, "Combat Manager thread stopping.");
        }
        private async Task RunItemDecayLoopAsync(TimeSpan interval, CancellationToken ct)
        {
            GameState.Log(DebugLevel.Alert, "Item Decay thread started.");
            while (!ct.IsCancellationRequested && IsRunning)
            {
                try
                {
                    GameState.Log(DebugLevel.Debug, "Decaying items...");
                    /* CODE REVIEW: Rylan (PR #16)
                     * This doesn't exist yet, and isn't where instances of items will be stored anyway
                     * 
                    foreach (var item in GameState.Instance.Items.Values)
                    {

                        if (item.IsPerishable)
                        {
                            item.UsesRemaining--;
                        }
                    }
                    */
                }
                catch (Exception ex)
                {
                    GameState.Log(DebugLevel.Error, $"Error during Item Decay: {ex.Message}");
                }

                await Task.Delay(interval, ct);
            }
            GameState.Log(DebugLevel.Alert, "Item Decay thread stopping.");
        }

        // CODE REVIEW: Rylan (PR #16)
        // We should consider whether this is necessary.
        private async Task RunTickLoopAsync(TimeSpan interval, CancellationToken ct)
        {
            GameState.Log(DebugLevel.Alert, "Tick thread started.");
            while (!ct.IsCancellationRequested && IsRunning)
            {
                try
                {
                    //GameState.Log(DebugLevel.Debug, "Updating tick...");
                    _tickCount++;
                }
                catch (Exception ex)
                {
                    GameState.Log(DebugLevel.Error, $"Error during tick update: {ex.Message}");
                }

                await Task.Delay(interval, ct);
            }
            GameState.Log(DebugLevel.Alert, "Tick thread stopping.");
        }

        private async Task RunWeatherLoopAsync(TimeSpan interval, CancellationToken ct)
        {
            GameState.Log(DebugLevel.Alert, "Weather thread started.");
            while (!ct.IsCancellationRequested && IsRunning)
            {
                try
                {
                    GameState.Log(DebugLevel.Debug, "Predicting the weather...");
                    // Update weather in all areas
                    //choose random from list, apply to area
                    //repeat for every area
                    //await build team for areas/weather types
                    foreach (var area in Areas.Values)
                    {
                        UpdateWeather();
                    }
                }
                catch (Exception ex)
                {
                    GameState.Log(DebugLevel.Error, $"Error during weather update: {ex.Message}");
                }

                await Task.Delay(interval, ct);
            }
            GameState.Log(DebugLevel.Alert, "Weather thread stopping.");
        }

        // CODE REVIEW: Rylan (PR #16)
        // All of the weather code (UpdateWeather, weatherStates)
        // needs to be moved to its own class.
        // An enum for WeatherState would be better than a list of strings.
        //   It should go in the enums folder.
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

        // CODE REVIEW: Rylan (PR #16)
        // I think this section needs to be heavily refactored. The functionality itself
        // probably should live in the NonPlayer, Mob, etc. classes. 
        // See me to discuss a better design for this.
        private async Task RunNPCLoopAsync(TimeSpan interval, CancellationToken ct)
        {
            GameState.Log(DebugLevel.Alert, "NPC thread started.");
            while (!ct.IsCancellationRequested && IsRunning)
            {
                try
                {
                    GameState.Log(DebugLevel.Debug, "Playing NPCs...");
                    // run command to check for players, then choose NPC actions
                    // hostile/agro prioritize attacking players, then other NPCs, then wandering
                    // friendly prioritize helping players, then other NPCs, then wandering
                    // neutral prioritize wandering, then attacking hostile NPCs
                    // repeat for every NPC
                    // await NPC team for behaviors/actions and NPCs
                    // make sure to include some randomness so NPCs don't all act at once
                    // don't allow for NPCs to leave tasks when engaged (e.g., attacking, helping)

                    /* CODE REVIEW: Rylan (PR #16)
                     * We don't have NonPlayers in GameState.

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
                                foreach (Player player in npc.GetRoom().Players)
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
                                    Player target = potentialTargets[new Random().Next(0, potentialTargets.Count - 1)];
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
                        }
                    }
                    */
                }
                catch (Exception ex)
                {
                    GameState.Log(DebugLevel.Error, $"Error during NPC update: {ex.Message}");
                }

                await Task.Delay(interval, ct);
            }
            GameState.Log(DebugLevel.Alert, "NPC thread stopping.");
        }

        #endregion --- Thread Methods ---

    }
}
    
