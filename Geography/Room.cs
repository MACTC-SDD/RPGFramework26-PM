using RPGFramework.Display;
using RPGFramework.Enums;
using System.ComponentModel;
using System.Numerics;

namespace RPGFramework.Geography
{
    internal class Room
    {
        #region --- Properties ---
        // Unique identifier for the room
        public int Id { get; set; } = 0;

        // What area this belongs to 
        public int AreaId { get; set; } = 0;

        // Description of the room
        public string Description { get; set; } = "";
        public List<Item> Items { get; set; } = [];


        // Icon to display on map
        public string MapIcon { get; set; } = DisplaySettings.RoomMapIcon;
        public string MapColor { get; set; } = DisplaySettings.RoomMapIconColor;

        public int MaxMobs {  get; set; } = 1; // Maximum number of Mob NPCs allowed in the room
        public Dictionary<string, double> MobSpawnList { get; private set; } = []; // Mob name and spawn chance

        // Name of the room
        public string Name { get; set; } = "";

        public List<NonPlayer> NonPlayers { get; set; } = [];

        public List<string> Tags { get; set; } = []; // (for scripting or special behavior)

        // List of exits from the room
        public List<int> ExitIds { get; set; } = [];

        #endregion --- Properties ---

        #region --- Methods ---

        #region AddMobSpawn Method
        public bool AddMobSpawn(string mobName, double spawnChance)
        {
            if (!GameState.Instance.MobCatalog.TryGetValue(mobName, out var mob))
            {
                return false; // Mob does not exist
            }

            if (mob == null || MobSpawnList.ContainsKey(mob.Name))
            {
                return false; // Mob already in spawn list
            }

            MobSpawnList.Add(mob.Name, spawnChance);
            return true;
        }
        #endregion

        #region AddExits Method
        /// <summary>
        /// This is for creating a new exit (and return exit), not linking existing exit items.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="exitDescription"></param>
        /// <param name="destinationRoom"></param>
        /// <param name="returnExit"></param>
        public void AddExits(Player player, Direction direction, string exitDescription, Room destinationRoom, bool returnExit = true)
        {
            // Make sure there isn't already an exit in the specified direction from this room

            if (GetExits().Any(e => e.ExitDirection == direction))
            {
                player.WriteLine("There is already an exit going that direction.");
                return;
            }

            // Make sure the destination room doesn't already have an exit in the opposite direction
            if (returnExit 
                && destinationRoom.GetExits().Any(e => e.ExitDirection == Navigation.GetOppositeDirection(direction)))
            {
                player.WriteLine("The destination room already has an exit coming from the opposite direction.");
                return;
            }

            // Create a new Exit object from this room
            Exit exit = new()
            {
                Id = Exit.GetNextId(AreaId),
                // Set area ids so cross-area exits keep their origin/destination areas
                SourceAreaId = AreaId,
                SourceRoomId = Id,
                DestinationAreaId = destinationRoom.AreaId,
                DestinationRoomId = destinationRoom.Id,
                ExitDirection = direction,
                Description = exitDescription
            };
            // Keep ExitType default unless modified later.
            // Apply sensible open/close defaults based on ExitType
            exit.ApplyDefaultsForType();

            ExitIds.Add(exit.Id);
            GameState.Instance.Areas[AreaId].Exits.Add(exit.Id, exit);

            // Create a new exit from the destination room back to this room
            if (returnExit)
            {
                Exit exit1 = new()
                {
                    Id = Exit.GetNextId(destinationRoom.AreaId),
                    // set area ids for the return exit as well
                    SourceAreaId = destinationRoom.AreaId,
                    SourceRoomId = destinationRoom.Id,
                    DestinationAreaId = AreaId,
                    DestinationRoomId = Id,
                    ExitDirection = Navigation.GetOppositeDirection(direction)
                };
                exit1.Description = exitDescription.Replace(direction.ToString(), exit1.ExitDirection.ToString());
                // Mirror the exit type and defaults
                exit1.ExitType = exit.ExitType;
                exit1.ApplyDefaultsForType();

                destinationRoom.ExitIds.Add(exit1.Id);
                GameState.Instance.Areas[destinationRoom.AreaId].Exits.Add(exit1.Id, exit1);
            }
        }
        #endregion

        #region CheckForExit Methods (Static)
        /// <summary>
        /// Determines whether the specified room contains an exit in the given direction.
        /// </summary>
        /// <param name="room">The room to check for an exit. Cannot be null.</param>
        /// <param name="exitDirection">The direction in which to check for an exit.</param>
        /// <returns>true if the room contains an exit in the specified direction; otherwise, false.</returns>
        public static bool CheckForExit(Room room, Direction exitDirection)
        {
            return room.GetExits().Any(e => e.ExitDirection == exitDirection);
        }

        /// <summary>
        /// Determines whether the specified room contains an exit in the given direction, excluding the provided exit.
        /// </summary>
        /// <param name="room">The room to check for the presence of an exit.</param>
        /// <param name="exitDirection">The direction in which to check for an existing exit.</param>
        /// <param name="exit">The exit to exclude from the check. Typically, this is the exit being modified or considered for addition.</param>
        /// <returns>true if the room contains an exit in the specified direction other than the provided exit; otherwise, false.</returns>
        public static bool CheckForExit(Room room, Direction exitDirection, Exit exit)
        {
            return room.GetExits().Any(e => e.ExitDirection == exitDirection && e.Id != exit.Id);
        }
        #endregion

        #region CreateRoom Methods (Static)
        /// <summary>
        /// Create a new room object in specified area and add it to GameState Area
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Room CreateRoom(int areaId, string name, string description)
        {
            Room room = new()
            {
                Id = GetNextId(areaId),
                Name = name,
                Description = description
            };
            GameState.Instance.Areas[areaId].Rooms.Add(room.Id, room);

            return room;
        }

        public static Room CreateRoom(Area area, string name, string description)
        {
            return CreateRoom(area.Id, name, description);
        }
        #endregion

        #region DeleteRoom Methods (Static)
        /// <summary>
        /// Delete a room (and its linked exits) from the specified area
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="roomId"></param>
        public static void DeleteRoom(int areaId, int roomId)
        {
            // Remove the room from the area
            GameState.Instance.Areas[areaId].Rooms.Remove(roomId);

            // Remove all exits from the room
            List<Exit> exits = [.. GameState.Instance.Areas[areaId]
                    .Exits.Values
                    .Where(e => e.SourceRoomId == roomId || e.DestinationRoomId == roomId)];

            foreach (Exit e in exits)
            {
                GameState.Instance.Areas[areaId].Exits.Remove(e.Id);
            }
        }

        public static void DeleteRoom(Room room)
        {
            DeleteRoom(room.AreaId, room.Id);
        }
        #endregion

        #region GetExits Methods 
        /// <summary>
        /// Return a list of Exit objects that are in this room.
        /// </summary>
        /// <returns></returns>
        public List<Exit> GetExits()
        {
            // This works just like the loop in GetPlayersInRoom, but is shorter
            // This style of list maniuplation is called "LINQ"
            return [.. GameState.Instance.Areas[AreaId].Exits.Values
                .Where(e => e.SourceRoomId == Id)];
        }
        #endregion

        #region FindCharacterInRoom Method (Static)
        public static Character? FindCharacterInRoom(Room room, string name)
        {
            // Search players first
            Character? character = GetPlayersInRoom(room)
                .FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (character != null)
            {
                return character;
            }

            // Search NPCs next
            character = room.NonPlayers
                .FirstOrDefault(npc => npc.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return character;
        }
        #endregion

        #region GetExitByName/Id Methods
        /// <summary>
        /// Returns the first exit in this room that matches the specified name.
        /// </summary>
        /// <returns></returns>
        public Exit? GetExitByName(string name)
        {
            return GetExits().Find(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns the first exit in this room that matches the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Exit? GetExitById(int id)
        {
            return GetExits().Find(e => e.Id == id);
        }
        #endregion

        #region GetNextId Method (Static)
        /// <summary>
        /// Get the next available room ID for the specified area.
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public static int GetNextId(int areaId)
        {
            if (GameState.Instance.Areas[areaId].Rooms.Count == 0)
            {
                return 0;
            }

            return GameState.Instance.Areas[areaId].Rooms.Keys.Max() + 1;
        }
        #endregion

        #region GetPlayersInRoom Method (Static)
        /// <summary>
        /// Return a list of player objects that are in the specified room
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static List<Player> GetPlayersInRoom(Room room)
        {
            // Loop through GameState.ConnectedPlayers and return a list of players in the room
            List<Player> playersInRoom = [];
            foreach (Player p in GameState.Instance.Players.Values)
            {
                if (p.IsOnline 
                    && p.AreaId == room.AreaId 
                    && p.LocationId == room.Id)
                {
                    playersInRoom.Add(p);
                }
            }

            return playersInRoom;
        }
        #endregion

        #region GetCharactersInRoom Method (Static)
        /// <summary>
        /// Return a list of both players and NPCs in the specified room
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static List<Character> GetCharactersInRoom(Room room)
        { 
            return [.. GetPlayersInRoom(room).Cast<Character>(),
                .. room.NonPlayers.Cast<Character>()];
        }
        #endregion

        #region FindReturnExit Methods (Static)
        public Exit? FindReturnExit(Exit exit)
        {
            return FindReturnExit(Id, AreaId, exit.DestinationRoomId);
        }

        public static Exit? FindReturnExit(int sourceRoomId, int sourceAreaId, int destinationRoomid)
        {
            // We want to find that goes from exit.DestinationRoomId back to e.SourceRoomId
            foreach (var kvp in GameState.Instance.Areas)
            {
                if (kvp.Value.Rooms.ContainsKey(sourceRoomId))
                {
                    return kvp.Value.Exits.Values.FirstOrDefault(e =>
                        e.SourceRoomId == destinationRoomid
                        && e.DestinationRoomId == sourceRoomId
                        && e.DestinationAreaId == sourceAreaId);
                }
            }
            return null;
        }

        public Item? FindItem(string itemName)
        {
            return Items.Find(x => x.Name.ToLower() == itemName.ToLower());
        }

        public Item? FindItem(int itemId)
        {
            return Items.Find(x => x.Id == itemId);
        }


        #endregion

        #region TryParseId Method (Static)
        public static bool TryParseId(string input, int defaultArea, out int roomId, out int areaId)
        {
            roomId = -1;
            areaId = defaultArea;

            // Check for area:room format
            if (input.Contains(':'))
            {
                var parts = input.Split(':');
                if (parts.Length != 2
                    || !int.TryParse(parts[0], out areaId)
                    || !int.TryParse(parts[1], out roomId))
                {
                    return false;
                }
                return true;
            }

            // No area specified, use default
            if (!int.TryParse(input, out roomId))
            {
                return false;
            }

            return true;
        }
        #endregion

        #endregion --- Methods ---

        #region --- Methods (Events) ---

        /// <summary>
        /// When a character enters a room, do this.
        /// </summary>
        /// <param name="character"></param>
        public void EnterRoom(Character character, Room fromRoom)
        {
            // Send a message to the player
            Comm.SendToIfPlayer(character, Description);

            // Send a message to all players in the room
            Comm.SendToRoomExcept(this, $"{character.Name} enters the room.", character);
        }

        /// <summary>
        /// When a character leaves a room, do this.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="toRoom"></param>
        public void LeaveRoom(Character character, Room toRoom)
        {
           // Send a message to all players in the room
            Comm.SendToRoomExcept(this, $"{character.Name} leaves the room.", character);
        }
        #endregion
    }
}
