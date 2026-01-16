using RPGFramework.Enums;
using RPGFramework.Geography;

namespace RPGFramework.Commands
{
    internal class BuilderCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return new List<ICommand>
            {
                new RoomBuilderCommand(),
                // Add more builder commands here as needed
            };
        }
    }

    /// <summary>
    /// /room command for building and editing rooms.
    /// </summary>
    internal class RoomBuilderCommand : ICommand
    {
        public string Name => "/room";

        public IEnumerable<string> Aliases => Array.Empty<string>();

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
            {
                return false;
            }

            if (parameters.Count < 2)
            {
                WriteUsage(player);
                return false;
            }

            // Decide what to do based on the second parameter
            switch (parameters[1].ToLower())
            {
                case "create":
                    RoomCreate(player, parameters);
                    break;
                case "set":
                    // We'll move setting name and description into this
                    RoomSet(player, parameters);
                    break;
                case "show":
                    ShowCommand(player, parameters);
                    break;
                case "add":
                    RoomAdd(player, parameters);
                    break;
                case "remove":
                    RoomRemove(player, parameters);
                    break;
                default:
                    WriteUsage(player);
                    break;
                case "delete":
                    RoomDelete(player, parameters);
                    break;
            }

            return true;
        }

        private static void RoomSet(Player player, List<string> parameters)
        {
            if (parameters.Count < 3)
            {
                WriteUsage(player);
                return;
            }
            switch (parameters[2].ToLower())
            {
                case "description":
                    RoomSetDescription(player, parameters);
                    break;
                case "name":
                    RoomSetName(player, parameters);
                    break;
                // As we add more settable properties, we can expand this switch
                default:
                    WriteUsage(player);
                    break;
                case "icon":
                    RoomSetIcon(player, parameters);
                    break;
                case "tags":
                    RoomSetTags(player, parameters);
                    break;
                case "exit":
                    RoomSetExit(player, parameters);
                    break;
            }
        }

        private static void WriteUsage(Player player)
        {
            player.WriteLine("Usage: ");
            player.WriteLine("/room set description '<set room desc to this>'");
            player.WriteLine("/room set name '<set room name to this>'");
            player.WriteLine("/room create '<name>' '<description>' <exit direction> '<exit description>'");
            player.WriteLine("/room add <direction> <destRoomId> '<exit description>'");
            player.WriteLine("  or: /room add <direction> <areaId>:<roomId> '<exit description>'");
            player.WriteLine("/room remove <direction>");
            player.WriteLine("/room remove id <exitId>");
            player.WriteLine("/room delete");
            player.WriteLine("/room set icon '<Icon>'");
            player.WriteLine("/room set color '<color>'");
            player.WriteLine("/room set tags '<tag, tag, tag>'");
            player.WriteLine("/room set exit dir <exitId> <direction>   - Changes direction (validates duplicates, updates return exit)");
            player.WriteLine("/room set exit dest <exitId> <roomId>     - Change destination (use <areaId>:<roomId> to specify area)");
            player.WriteLine("/room set exit type <exitId> <Open|Door|LockedDoor|Impassable> - Change exit type");
            player.WriteLine("/room set exit open <exitId> <open|close> - Open or close this exit (doors only)");
            //to see tags and desc and name etc, just do /room <name of thing> and nothing after
        }

        private static void RoomCreate(Player player, List<string> parameters)
        {
            if (!Utility.CheckPermission(player, PlayerRole.Player))
            {
                player.WriteLine("You do not have permission to do that.");
                player.WriteLine("Your Role is: " + player.PlayerRole.ToString());
                return;
            }

            // 0: /room
            // 1: create
            // 2: name
            // 3: description
            // 4: exit direction
            // 5: exit description
            if (parameters.Count < 6)
            {
                player.WriteLine("Usage: /room create '<name>' '<description>' <exit direction> '<exit description>'");
                return;
            }

            if (!Enum.TryParse(parameters[4], true, out Direction exitDirection))
            {
                player.WriteLine("Invalid exit direction.");
                return;
            }

            try
            {
                Room room = Room.CreateRoom(player.AreaId, parameters[2], parameters[3]);

                player.GetRoom().AddExits(player, exitDirection, parameters[5], room);
                player.WriteLine("Room created.");
            }
            catch (Exception ex)
            {
                player.WriteLine($"Error creating room: {ex.Message}");
                player.WriteLine(ex.StackTrace);
            }
        }

        private static void RoomSetDescription(Player player, List<string> parameters)
        {
            if (!Utility.CheckPermission(player, PlayerRole.Admin))
            {
                player.WriteLine("You do not have permission to do that.");
                return;
            }

            if (parameters.Count < 4)
            {
                player.WriteLine(player.GetRoom().Description);
            }
            else
            {
                player.GetRoom().Description = parameters[3];
                player.WriteLine("Room description set.");
            }
        }

        private static void RoomSetIcon(Player player, List<string> parameters)
        {
            if (!Utility.CheckPermission(player, PlayerRole.Admin))
            {
                player.WriteLine("You do not have permission to do that.");
                return;
            }

            if (parameters.Count < 4)
            {
                player.WriteLine($"Current room icon: {player.GetRoom().MapIcon}");
            }
            else

                player.GetRoom().MapIcon = parameters[3];
            player.WriteLine($"Room icon set to: {player.GetRoom().MapIcon}");
            return;
        }

        private static void RoomSetName(Player player, List<string> parameters)
        {
            if (parameters.Count < 4)
            {
                player.WriteLine(player.GetRoom().Name);
            }
            else
            {
                player.GetRoom().Name = parameters[3];
                player.WriteLine("Room name set.");
            }
        }
        private static void RoomDelete(Player player, List<string> parameters)

        {
            if (!Utility.CheckPermission(player, PlayerRole.Admin))
            {
                player.WriteLine("You do not have permission to do that.");
                return;
            }

            Room currentRoom = player.GetRoom();

            // Prevent deletion of the current room if it's the only one or critical (optional safety)
            if (currentRoom == null)
            {
                player.WriteLine("No room to delete.");
                return;
            }

            // Optional: Confirm deletion (if your system supports confirmation)
            if (parameters.Count < 3 || parameters[2] != "confirm")
            {
                player.WriteLine("To delete this room, use: /room delete confirm");
                return;
            }

            try
            {
                // Teleport player to safe location (LocationId = 0)
                player.LocationId = 0;
                player.WriteLine("You have been moved to the safe room.");

                // Delete the room from storage (adjust based on your Room management)
                Room.DeleteRoom(currentRoom); // Assuming you have such a method

                player.WriteLine("Room deleted.");
            }
            catch (Exception ex)
            {
                player.WriteLine($"Error deleting room: {ex.Message}");
            }
        }


        private static void RoomSetTags(Player player, List<string> parameters)
        {
            Room room = player.GetRoom();

            if (parameters.Count < 4)
            {
                if (room.Tags.Count == 0)
                {
                    player.WriteLine("This room has no tags.");
                }
                else
                {
                    player.WriteLine("Room tags: " + string.Join(", ", room.Tags));
                }
                return;
            }

            string tagInput = string.Join(" ", parameters.Skip(3)).Trim('"').ToLowerInvariant();

            // Wipe tags if user types "none", "clear", or "empty"
            if (tagInput == "none" || tagInput == "clear" || tagInput == "empty")
            {
                room.Tags.Clear();
                player.WriteLine("All room tags have been removed.");
                return;
            }

            // Otherwise, set new tags
            room.Tags = tagInput.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(t => t.Trim())
                                .ToList();

            player.WriteLine("Room tags set: " + string.Join(", ", room.Tags));
        }

        /// <summary>
        /// Add an exit from the player's current room to an existing room.
        /// Usage:
        ///   /room add <direction> <destRoomId> '<exit description>'
        ///   /room add <direction> <areaId>:<destRoomId> '<exit description>'
        /// Optional: append "noreturn" to not create a return exit.
        /// Requires Builder role or higher.
        /// </summary>
        private static void RoomAdd(Player player, List<string> parameters)
        {
            if (!Utility.CheckPermission(player, PlayerRole.Builder))
            {
                player.WriteLine("You do not have permission to do that.");
                player.WriteLine("Your Role is: " + player.PlayerRole.ToString());
                return;
            }

            // Need at least: /room add <direction> <dest> '<description>'
            if (parameters.Count < 5)
            {
                player.WriteLine("Usage: /room add <direction> <destRoomId> '<exit description>'");
                player.WriteLine("  or: /room add <direction> <areaId>:<destRoomId> '<exit description>'");
                return;
            }

            if (!Enum.TryParse(parameters[2], true, out Direction direction))
            {
                player.WriteLine("Invalid exit direction.");
                return;
            }

            // Parse destination (areaId:roomId) or just roomId (same area as player)
            int destAreaId = player.AreaId;
            int destRoomId;
            string destParam = parameters[3];
            if (destParam.Contains(":"))
            {
                string[] parts = destParam.Split(':');
                if (parts.Length != 2
                    || !int.TryParse(parts[0], out destAreaId)
                    || !int.TryParse(parts[1], out destRoomId))
                {
                    player.WriteLine("Invalid destination format. Use <destRoomId> or <areaId>:<destRoomId>.");
                    return;
                }
            }
            else
            {
                if (!int.TryParse(destParam, out destRoomId))
                {
                    player.WriteLine("Invalid destination room id.");
                    return;
                }
            }

            // Join remaining parameters for description (parameters[4]) - keep as given
            string exitDescription = parameters[4];

            // Optional flag to prevent creating a return exit
            bool returnExit = !parameters.Any(p => p.Equals("noreturn", StringComparison.OrdinalIgnoreCase));

            // Validate destination room exists
            if (!GameState.Instance.Areas.ContainsKey(destAreaId)
                || !GameState.Instance.Areas[destAreaId].Rooms.ContainsKey(destRoomId))
            {
                player.WriteLine($"Destination room not found (Area: {destAreaId}, Room: {destRoomId}).");
                return;
            }

            Room destRoom = GameState.Instance.Areas[destAreaId].Rooms[destRoomId];

            try
            {
                Room current = player.GetRoom();
                if (current == null)
                {
                    player.WriteLine("You are not in a valid room.");
                    return;
                }

                // Make sure there's no existing exit in that direction
                if (current.GetExits().Any(e => e.ExitDirection == direction))
                {
                    player.WriteLine("There is already an exit in that direction from this room.");
                    return;
                }

                current.AddExits(player, direction, exitDescription, destRoom, returnExit);
                player.WriteLine($"Exit added: {direction} -> Area {destAreaId} Room {destRoomId}.");
            }
            catch (Exception ex)
            {
                player.WriteLine($"Error adding exit: {ex.Message}");
            }
        }

        /// <summary>
        /// Remove an exit from the player's current room.
        /// Usage:
        ///   /room remove <direction>
        ///   /room remove id <exitId>
        /// Requires Builder role or higher.
        /// This will attempt to remove the mirrored return exit as well.
        /// </summary>
        private static void RoomRemove(Player player, List<string> parameters)
        {
            if (!Utility.CheckPermission(player, PlayerRole.Builder))
            {
                player.WriteLine("You do not have permission to do that.");
                player.WriteLine("Your Role is: " + player.PlayerRole.ToString());
                return;
            }

            if (parameters.Count < 3)
            {
                player.WriteLine("Usage: /room remove <direction>");
                player.WriteLine("   or: /room remove id <exitId>");
                return;
            }

            Room current = player.GetRoom();
            if (current == null)
            {
                player.WriteLine("You are not in a valid room.");
                return;
            }

            Exit? exitToRemove = null;

            // Remove by explicit exit id
            if (parameters[2].Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                if (parameters.Count < 4 || !int.TryParse(parameters[3], out int exitId))
                {
                    player.WriteLine("Invalid exit id.");
                    return;
                }

                exitToRemove = current.GetExits().FirstOrDefault(e => e.Id == exitId);
                if (exitToRemove == null)
                {
                    player.WriteLine($"No exit with id {exitId} found in this room.");
                    return;
                }
            }
            else
            {
                // Treat parameters[2] as direction
                if (!Enum.TryParse(parameters[2], true, out Direction direction))
                {
                    player.WriteLine("Invalid exit direction.");
                    return;
                }

                exitToRemove = current.GetExits().FirstOrDefault(e => e.ExitDirection == direction);
                if (exitToRemove == null)
                {
                    player.WriteLine($"No exit going {direction} found in this room.");
                    return;
                }
            }

            try
            {
                // Remove the exit from its area's exit dictionary and from the source room's ExitIds
                if (GameState.Instance.Areas.ContainsKey(current.AreaId)
                    && GameState.Instance.Areas[current.AreaId].Exits.ContainsKey(exitToRemove.Id))
                {
                    GameState.Instance.Areas[current.AreaId].Exits.Remove(exitToRemove.Id);
                }

                current.ExitIds.Remove(exitToRemove.Id);

                // Attempt to find and remove the return exit (if any) in the destination room's area
                int destRoomId = exitToRemove.DestinationRoomId;
                int destAreaId = -1;
                foreach (var kvp in GameState.Instance.Areas)
                {
                    if (kvp.Value.Rooms.ContainsKey(destRoomId))
                    {
                        destAreaId = kvp.Key;
                        break;
                    }
                }

                if (destAreaId != -1)
                {
                    var destArea = GameState.Instance.Areas[destAreaId];
                    // Find any exit that goes from the destination back to this source
                    var returnExit = destArea.Exits.Values.FirstOrDefault(e =>
                        e.SourceRoomId == destRoomId
                        && e.DestinationRoomId == exitToRemove.SourceRoomId);

                    if (returnExit != null)
                    {
                        destArea.Exits.Remove(returnExit.Id);

                        // Also remove the id from the destination room's ExitIds list if present
                        var destRoom = destArea.Rooms[destRoomId];
                        destRoom.ExitIds.Remove(returnExit.Id);
                    }
                }

                player.WriteLine("Exit removed.");
            }
            catch (Exception ex)
            {
                player.WriteLine($"Error removing exit: {ex.Message}");
            }
        }

        private static void ShowCommand(Player player, List<string> parameters)
        {
            Room r = player.GetRoom();
            player.WriteLine($"Name: {r.Name}");
            player.WriteLine($"Id: {r.Id.ToString()}");
            player.WriteLine($"Area Id: {r.AreaId.ToString()}");
            player.WriteLine($"Description: {r.Description}");

            // Show exits in the current room
            var exits = r.GetExits();
            if (exits == null || exits.Count == 0)
            {
                player.WriteLine("Exits: None");
                return;
            }

            player.WriteLine("Exits:");
            foreach (var e in exits)
            {
                // Try to resolve destination room (rooms are stored per-area)
                Room? destRoom = null;
                if (GameState.Instance.Areas.ContainsKey(r.AreaId)
                    && GameState.Instance.Areas[r.AreaId].Rooms.ContainsKey(e.DestinationRoomId))
                {
                    destRoom = GameState.Instance.Areas[r.AreaId].Rooms[e.DestinationRoomId];
                }

                string destName = destRoom != null ? destRoom.Name : "Unknown";
                string destId = e.DestinationRoomId.ToString();

                // Include open/closed info
                string openState = e.IsOpen ? "Open" : "Closed";

                player.WriteLine($"{e.ExitDirection} -> {destName} (Id: {destId}) [[{e.ExitType}]] [[{openState}]] : {e.Description}");
            }//this code above very specifically needs [[ ]] instead of [ ]
        }

        /// <summary>
        /// /room set exit ...
        /// Supports:
        ///   /room set exit dir <exitId> <direction>
        ///   /room set exit dest <exitId> <roomId>   (or <areaId>:<roomId>)
        ///   /room set exit type <exitId> <Open|Door|LockedDoor|Impassable>
        ///   /room set exit open <exitId> <open|close>
        /// All require Builder permission.
        /// </summary>
        private static void RoomSetExit(Player player, List<string> parameters)
        {
            if (!Utility.CheckPermission(player, PlayerRole.Builder))
            {
                player.WriteLine("You do not have permission to do that.");
                player.WriteLine("Your Role is: " + player.PlayerRole.ToString());
                return;
            }

            if (parameters.Count < 4)
            {
                WriteUsage(player);
                return;
            }

            var sub = parameters[3].ToLower();
            Room current = player.GetRoom();
            if (current == null)
            {
                player.WriteLine("You are not in a valid room.");
                return;
            }

            // Expect exit id for all subcommands
            if (parameters.Count < 5 || !int.TryParse(parameters[4], out int exitId))
            {
                player.WriteLine("Usage: /room set exit <dir|dest|type|open> <exitId> <...>");
                return;
            }

            // Find the exit across all areas (allow builders to operate on exits outside current room)
            Exit? exit = null;
            int exitAreaId = -1;
            foreach (var kvp in GameState.Instance.Areas)
            {
                if (kvp.Value.Exits.ContainsKey(exitId))
                {
                    exit = kvp.Value.Exits[exitId];
                    exitAreaId = kvp.Key;
                    break;
                }
            }

            if (exit == null)
            {
                player.WriteLine($"Exit id {exitId} not found.");
                return;
            }

            // Helper: try find the return exit (if any)
            Exit? FindReturnExit(int sourceRoomId, int destRoomId)
            {
                foreach (var kvp in GameState.Instance.Areas)
                {
                    if (kvp.Value.Rooms.ContainsKey(destRoomId))
                    {
                        return kvp.Value.Exits.Values.FirstOrDefault(e =>
                            e.SourceRoomId == destRoomId && e.DestinationRoomId == sourceRoomId);
                    }
                }
                return null;
            }

            try
            {
                switch (sub)
                {
                    case "dir":
                        {
                            // dir requires the exit to be in the current room (we only allow changing direction for exits in your current room)
                            if (exit.SourceRoomId != current.Id || exitAreaId != current.AreaId)
                            {
                                player.WriteLine("You can only change the direction of exits that belong to the current room.");
                                return;
                            }

                            if (parameters.Count < 6)
                            {
                                player.WriteLine("Usage: /room set exit dir <exitId> <direction>");
                                return;
                            }

                            if (!Enum.TryParse(parameters[5], true, out Direction newDir))
                            {
                                player.WriteLine("Invalid direction.");
                                return;
                            }

                            // Validate no duplicate in current room (except this exit)
                            if (current.GetExits().Any(e => e.ExitDirection == newDir && e.Id != exit.Id))
                            {
                                player.WriteLine("There is already an exit in that direction from this room.");
                                return;
                            }

                            // Validate destination room doesn't already have an exit in the opposite direction (except the return exit we'll update)
                            Direction opposite = Navigation.GetOppositeDirection(newDir);
                            Exit? returnExit = FindReturnExit(exit.SourceRoomId, exit.DestinationRoomId);

                            // Find destination area's room and check its exits
                            int destAreaId = -1;
                            foreach (var kvp in GameState.Instance.Areas)
                            {
                                if (kvp.Value.Rooms.ContainsKey(exit.DestinationRoomId))
                                {
                                    destAreaId = kvp.Key;
                                    break;
                                }
                            }

                            if (destAreaId != -1)
                            {
                                var destRoom = GameState.Instance.Areas[destAreaId].Rooms[exit.DestinationRoomId];
                                // If some other exit (not the returnExit) already uses that opposite direction, fail.
                                if (destRoom.GetExits().Any(e => e.ExitDirection == opposite && (returnExit == null || e.Id != returnExit.Id)))
                                {
                                    player.WriteLine("Destination room already has an exit using the opposite direction.");
                                    return;
                                }
                            }

                            // Update directions
                            Direction oldDir = exit.ExitDirection;
                            exit.ExitDirection = newDir;

                            // Update return exit direction if present
                            var ret = FindReturnExit(exit.SourceRoomId, exit.DestinationRoomId);
                            if (ret != null)
                            {
                                ret.ExitDirection = opposite;

                                // Try to keep description consistent (replace old direction name with new one if present)
                                if (!string.IsNullOrEmpty(ret.Description))
                                {
                                    ret.Description = ret.Description.Replace(oldDir.ToString(), opposite.ToString());
                                }
                            }

                            if (!string.IsNullOrEmpty(exit.Description))
                            {
                                exit.Description = exit.Description.Replace(oldDir.ToString(), newDir.ToString());
                            }

                            player.WriteLine($"Exit {exitId} direction set to {newDir}.");
                            break;
                        }
                    case "dest":
                        {
                            // dest requires the exit to be in the current room
                            if (exit.SourceRoomId != current.Id || exitAreaId != current.AreaId)
                            {
                                player.WriteLine("You can only change the destination of exits that belong to the current room.");
                                return;
                            }

                            if (parameters.Count < 6)
                            {
                                player.WriteLine("Usage: /room set exit dest <exitId> <roomId>   (or <areaId>:<roomId>)");
                                return;
                            }

                            // parse new destination
                            int newAreaId = current.AreaId;
                            int newRoomId;
                            string destParam = parameters[5];
                            if (destParam.Contains(":"))
                            {
                                var parts = destParam.Split(':');
                                if (parts.Length != 2
                                    || !int.TryParse(parts[0], out newAreaId)
                                    || !int.TryParse(parts[1], out newRoomId))
                                {
                                    player.WriteLine("Invalid destination format. Use <destRoomId> or <areaId>:<destRoomId>.");
                                    return;
                                }
                            }
                            else
                            {
                                if (!int.TryParse(destParam, out newRoomId))
                                {
                                    player.WriteLine("Invalid destination room id.");
                                    return;
                                }
                            }

                            if (!GameState.Instance.Areas.ContainsKey(newAreaId)
                                || !GameState.Instance.Areas[newAreaId].Rooms.ContainsKey(newRoomId))
                            {
                                player.WriteLine($"Destination room not found (Area: {newAreaId}, Room: {newRoomId}).");
                                return;
                            }

                            // Ensure new destination doesn't already have an exit in opposite direction pointing back to this source
                            Direction opposite = Navigation.GetOppositeDirection(exit.ExitDirection);
                            var newDestRoom = GameState.Instance.Areas[newAreaId].Rooms[newRoomId];
                            if (newDestRoom.GetExits().Any(e => e.ExitDirection == opposite))
                            {
                                player.WriteLine("New destination room already has an exit in the opposite direction.");
                                return;
                            }

                            // Remove old return exit if present
                            Exit? oldReturn = FindReturnExit(exit.SourceRoomId, exit.DestinationRoomId);
                            if (oldReturn != null)
                            {
                                // remove from the area's exit map and from the destination room ExitIds
                                foreach (var kvp in GameState.Instance.Areas)
                                {
                                    if (kvp.Value.Rooms.ContainsKey(oldReturn.SourceRoomId))
                                    {
                                        kvp.Value.Exits.Remove(oldReturn.Id);
                                        kvp.Value.Rooms[oldReturn.SourceRoomId].ExitIds.Remove(oldReturn.Id);
                                        break;
                                    }
                                }
                            }

                            // Update source exit to point to new destination
                            exit.DestinationRoomId = newRoomId;

                            // Add new return exit in new destination's area
                            var newReturn = new Exit();
                            newReturn.Id = Exit.GetNextId(newAreaId);
                            newReturn.SourceRoomId = newRoomId;
                            newReturn.DestinationRoomId = exit.SourceRoomId;
                            newReturn.ExitDirection = opposite;
                            // Mirror type and defaults
                            newReturn.ExitType = exit.ExitType;
                            newReturn.Description = exit.Description?.Replace(exit.ExitDirection.ToString(), opposite.ToString()) ?? "";
                            newReturn.ApplyDefaultsForType();

                            GameState.Instance.Areas[newAreaId].Exits.Add(newReturn.Id, newReturn);
                            GameState.Instance.Areas[newAreaId].Rooms[newRoomId].ExitIds.Add(newReturn.Id);

                            player.WriteLine($"Exit {exitId} destination changed to Area {newAreaId} Room {newRoomId}.");
                            break;
                        }
                    case "type":
                        {
                            // type requires the exit to be in the current room
                            if (exit.SourceRoomId != current.Id || exitAreaId != current.AreaId)
                            {
                                player.WriteLine("You can only change the type of exits that belong to the current room.");
                                return;
                            }

                            if (parameters.Count < 6)
                            {
                                player.WriteLine("Usage: /room set exit type <exitId> <Open|Door|LockedDoor|Impassable>");
                                return;
                            }

                            if (!Enum.TryParse(parameters[5], true, out ExitType newType))
                            {
                                player.WriteLine("Invalid exit type.");
                                return;
                            }

                            exit.ExitType = newType;
                            exit.ApplyDefaultsForType();

                            // Update mirrored return exit type if present
                            var returnExit = FindReturnExit(exit.SourceRoomId, exit.DestinationRoomId);
                            if (returnExit != null)
                            {
                                returnExit.ExitType = newType;
                                returnExit.ApplyDefaultsForType();
                            }

                            player.WriteLine($"Exit {exitId} type set to {newType}.");
                            break;
                        }
                    case "open":
                        {
                            if (parameters.Count < 6)
                            {
                                player.WriteLine("Usage: /room set exit open <exitId> <open|close>");
                                return;
                            }

                            string action = parameters[5].ToLower();
                            bool? setOpen = action switch
                            {
                                "open" => true,
                                "close" => false,
                                "true" => true,
                                "false" => false,
                                _ => null
                            };

                            if (!setOpen.HasValue)
                            {
                                player.WriteLine("Invalid value. Use 'open' or 'close'.");
                                return;
                            }

                            // Allow toggling doors even if they are not in the current room
                            if (exit.ExitType != ExitType.Door && exit.ExitType != ExitType.LockedDoor)
                            {
                                player.WriteLine("This exit cannot be opened or closed (only doors can be toggled).");
                                return;
                            }

                            exit.IsOpen = setOpen.Value;

                            // Update return exit state if present and if its type supports toggling
                            var returnExit = FindReturnExit(exit.SourceRoomId, exit.DestinationRoomId);
                            if (returnExit != null && (returnExit.ExitType == ExitType.Door || returnExit.ExitType == ExitType.LockedDoor))
                            {
                                returnExit.IsOpen = setOpen.Value;
                            }

                            player.WriteLine($"Exit {exitId} {(setOpen.Value ? "opened" : "closed")}.");
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                player.WriteLine($"Error updating exit: {ex.Message}");
            }
        }
    }
}
