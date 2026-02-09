using System.Reflection.Metadata.Ecma335;
using RPGFramework.Enums;
using RPGFramework.Geography;
using RPGFramework.Persistence;
using RPGFramework.Workflows;

namespace RPGFramework.Commands
{
    internal class BuilderCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return
            [
                new RoomBuilderCommand(),
                new ExitBuilderCommand(),
                new FindBuilderCommand(),
                new AreaBuilderCommand(),   // added /area
                // Add more builder commands here as needed
            ];
        }
    }

    #region ---Item Spawn---
    internal class ItemsSpawn : ICommand
    {
        public string Name => "/spawnitem";

        public IEnumerable<string> Aliases => [];
        public string Help => "";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
            {
                return false;
            }

            if (parameters.Count < 2)
            {
                player.WriteLine("No item");
                return false;
            }

            if (!GameState.Instance.ItemCatalog.TryGetValue(parameters[1], out var i) || i == null)
            {
                player.WriteLine("No item");
                return false;
            }

            Item? i2 = Utility.Clone(i);
            player.GetRoom().Items.Add(i2!);
            return true;
        }
        #endregion
    }


    #region --- Room Builder ---
    /// <summary>
    /// /room command for building and editing rooms.
    /// </summary>
    internal class RoomBuilderCommand : ICommand
    {
        public string Name => "/room";

        public IEnumerable<string> Aliases => [];
        public string Help => "";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
            {
                return false;
            }

            // All room commands require at least Builder role, no need to check each time
            if (Utility.CheckPermission(player, PlayerRole.Builder) == false)
            {
                player.WriteLine("You do not have permission to do that.");
                return false;
            }

            if (parameters.Count < 2)
            {
                ShowHelp(player);
                return false;
            }

            // Decide what to do based on the second parameter
            switch (parameters[1].ToLower())
            {
                case "create":
                    return RoomCreate(player, parameters);
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
                case "delete":
                    return RoomDelete(player, parameters);
                case "area":
                    return RoomArea(player, parameters);
                default:
                    ShowHelp(player);
                    break;
                case "arealist":
                    return RoomSetArea(player, parameters);

            }

            return true;
        }

        #region RoomSet Method
        private static bool RoomSet(Player player, List<string> parameters)
        {
            if (parameters.Count < 3)
            {
                return ShowHelp(player);
            }

            switch (parameters[2].ToLower())
            {
                case "description":
                    return RoomSetDescription(player, parameters);
                case "name":
                    return RoomSetName(player, parameters);
                // As we add more settable properties, we can expand this switch
                case "icon":
                    return RoomSetIcon(player, parameters);
                case "tags":
                    RoomSetTags(player, parameters);
                    break;
                case "exit":
                    RoomSetExit(player, parameters);
                    break;
                default:
                    return ShowHelp(player);
            }
            return false;
        }
        #endregion

        #region ShowHelp Method
        private static bool ShowHelp(Player player)
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
            player.WriteLine("/room area exit <areaId> '<exit description>'  - Create a cross-area exit to specified area (creates a new room in target area).");
            player.WriteLine("/room area list [[areaId]]               - List all rooms in an area");
            player.WriteLine("/room set area description <areaId>:<roomId> '<desc>'");

            //to see tags and desc and name etc, just do /room <name of thing> and nothing after

            return false;
        }
        #endregion

        #region RoomCreate Method
        private static bool RoomCreate(Player player, List<string> parameters)
        {
            // 0: /room
            // 1: create
            // 2: name
            // 3: description
            // 4: exit direction
            // 5: exit description
            if (parameters.Count < 6)
            {
                player.WriteLine("Usage: /room create '<name>' '<description>' <exit direction> '<exit description>'");
                return false;
            }

            if (!Enum.TryParse(parameters[4], true, out Direction exitDirection))
            {
                player.WriteLine("Invalid exit direction.");
                return false;
            }

            try
            {
                // Ensure room is created in the player's current area
                Room room = Room.CreateRoom(player.AreaId, parameters[2], parameters[3]);

                player.GetRoom().AddExits(player, exitDirection, parameters[5], room);
                player.WriteLine("Room created.");
                return true;
            }
            catch (Exception ex)
            {
                player.WriteLine($"Error creating room: {ex.Message}");
                player.WriteLine(ex.StackTrace ?? "");
            }
            return false;
        }
        #endregion

        #region RoomSetDescription Method
        private static bool RoomSetDescription(Player player, List<string> parameters)
        {
            if (parameters.Count < 4)
            {
                player.WriteLine(player.GetRoom().Description);
                return false;
            }
            player.GetRoom().Description = parameters[3];
            player.WriteLine("Room description set.");
            return true;
        }
        #endregion

        #region RoomSetIcon Method
        private static bool RoomSetIcon(Player player, List<string> parameters)
        {
            if (parameters.Count < 4)
            {
                player.WriteLine($"Current room icon: {player.GetRoom().MapIcon}");
                return false;
            }

            player.GetRoom().MapIcon = parameters[3];
            player.WriteLine($"Room icon set to: {player.GetRoom().MapIcon}");
            return true;
        }
        #endregion

        #region RoomSetName Method
        private static bool RoomSetName(Player player, List<string> parameters)
        {
            if (parameters.Count < 4)
            {
                player.WriteLine(player.GetRoom().Name);
                return false;
            }

            player.GetRoom().Name = parameters[3];
            player.WriteLine("Room name set.");
            return true;
        }
        #endregion

        #region RoomDelete Method
        private static bool RoomDelete(Player player, List<string> parameters)
        {
            Room currentRoom = player.GetRoom();

            // Prevent deletion of the current room if it's the only one or critical (optional safety)
            if (currentRoom == null)
            {
                player.WriteLine("No room to delete.");
                return false;
            }

            // Optional: Confirm deletion (if your system supports confirmation)
            if (parameters.Count < 3 || parameters[2] != "confirm")
            {
                player.WriteLine("To delete this room, use: /room delete confirm");
                return false;
            }

            try
            {
                // Teleport player to safe location (LocationId = 0)
                player.LocationId = 0;
                player.WriteLine("You have been moved to the safe room.");

                // Delete the room from storage (adjust based on your Room management)
                Room.DeleteRoom(currentRoom); 
                player.WriteLine("Room deleted.");
                return true;
            }
            catch (Exception ex)
            {
                player.WriteLine($"Error deleting room: {ex.Message}");
            }
            return false;
        }
        #endregion

        #region RoomSetExitDest Method
        public static bool RoomSetExitDest(Player player, List<string> parameters, Room currentRoom, Exit exit)
        {
            // dest requires the exit to be in the current room
            if (exit.SourceRoomId != currentRoom.Id)
            {
                player.WriteLine("You can only change the destination of exits that belong to the current room.");
                return false;
            }

            if (parameters.Count < 6)
            {
                player.WriteLine("Usage: /room set exit dest <exitId> <roomId>   (or <areaId>:<roomId>)");
                return false;
            }

            // parse new destination
            // I moved the splitting code to Room.TryParseId
            if (!Room.TryParseId(parameters[5], currentRoom.AreaId, out int newRoomId, out int newAreaId))
            {
                player.WriteLine("Invalid destination format. Use <destRoomId> or <areaId>:<destRoomId>.");
                return false;
            }

            // Find the target area/room objects, exit if they don't exist
            if (!GameState.Instance.Areas.TryGetValue(newAreaId, out Area? destArea)
                || !destArea.Rooms.TryGetValue(newRoomId, out Room? destRoom))
            {
                player.WriteLine($"Destination room not found (Area: {newAreaId}, Room: {newRoomId}).");
                return false;
            }

            // Ensure new destination doesn't already have an exit in opposite direction pointing back to this source
            Direction opposite = Navigation.GetOppositeDirection(exit.ExitDirection);
            //if (destRoom.GetExits().Any(e => e.ExitDirection == opposite))
            if (Room.CheckForExit(destRoom, opposite))
            {
                player.WriteLine("New destination room already has an exit in the opposite direction.");
                return false;
            }

            // Remove old return exit if present
            Exit? oldReturn = Room.FindReturnExit(exit.SourceRoomId, currentRoom.AreaId, exit.DestinationRoomId);
            if (oldReturn != null)
            {
                if (!GameState.Instance.Areas.TryGetValue(exit.DestinationAreaId, out Area? oldDestArea)
                    || !oldDestArea.Rooms.TryGetValue(exit.DestinationRoomId, out Room? oldDestRoom))
                {
                    player.WriteLine("Find a return exit, but couldn't locate area/room. Inconsistent data somewhere.");
                    return false;
                }

                oldDestRoom.ExitIds.Remove(oldReturn.Id);
                Exit.Delete(oldReturn);

                // CODE REVIEW: Ashten - Moved this functionality to Exit.Delete
                // remove from the area's exit map and from the destination room ExitIds
                /*foreach (var kvp in GameState.Instance.Areas)
                {
                    if (kvp.Value.Rooms.TryGetValue(oldReturn.SourceRoomId, out Room? value1))
                    {
                        kvp.Value.Exits.Remove(oldReturn.Id);
                        value1.ExitIds.Remove(oldReturn.Id);
                        break;
                    }
                }*/
            }

            // Update source exit to point to new destination
            exit.DestinationRoomId = newRoomId;
            exit.DestinationAreaId = newAreaId;

            // Add new return exit in new destination's area
            var newReturn = new Exit
            {
                Id = Exit.GetNextId(newAreaId),
                SourceAreaId = newAreaId,
                SourceRoomId = newRoomId,
                DestinationRoomId = exit.SourceRoomId,
                ExitDirection = opposite,
                // Mirror type and defaults
                ExitType = exit.ExitType,
                Description = exit.Description?.Replace(exit.ExitDirection.ToString(), opposite.ToString()) ?? ""
            };
            newReturn.ApplyDefaultsForType();
            destArea.Exits.Add(newReturn.Id, newReturn);
            destRoom.ExitIds.Add(newReturn.Id);

            player.WriteLine($"Exit {exit.Id} destination changed to Area {newAreaId} Room {newRoomId}.");
            return true;
        }
        #endregion

        #region RoomSetExitDir Method
        private static bool RoomSetExitDir(Player player, List<string> parameters, Exit exit)
        {
            Room currentRoom = player.GetRoom();
            // dir requires the exit to be in the current room (we only allow changing direction for exits in your current room)
            if (exit.SourceRoomId != currentRoom.Id) // No need to check, exit only found in our area || exitAreaId != currentRoom.AreaId)
            {
                player.WriteLine("You can only change the direction of exits that belong to the current room.");
                return false;
            }

            if (parameters.Count < 6)
            {
                player.WriteLine("Usage: /room set exit dir <exitId> <direction>");
                return false;
            }

            if (!Enum.TryParse(parameters[5], true, out Direction newDir))
            {
                player.WriteLine("Invalid direction.");
                return false;
            }

            // Validate no duplicate in current room (except this exit)
            if (currentRoom.GetExits().Any(e => e.ExitDirection == newDir && e.Id != exit.Id))
            {
                player.WriteLine("There is already an exit in that direction from this room.");
                return false;
            }

            // Validate destination room doesn't already have an exit in the opposite direction (except the return exit we'll update)
            Direction opposite = Navigation.GetOppositeDirection(newDir);
            Exit? returnExit = Room.FindReturnExit(exit.SourceRoomId, currentRoom.AreaId, exit.DestinationRoomId);

            var destRoom = GameState.Instance.Areas[exit.DestinationAreaId].Rooms[exit.DestinationRoomId];
            if (returnExit != null && Room.CheckForExit(destRoom, opposite, returnExit))
            {
                player.WriteLine("Destination room already has an exit using the opposite direction.");
                return false;
            }
         

            // Update directions
            Direction oldDir = exit.ExitDirection;
            exit.ExitDirection = newDir;

            if (returnExit != null)
            {
                returnExit.ExitDirection = opposite;

                // Try to keep description consistent (replace old direction name with new one if present)
                if (!string.IsNullOrEmpty(returnExit.Description))
                {
                    returnExit.Description = returnExit.Description.Replace(oldDir.ToString(), opposite.ToString());
                }
            }

            if (!string.IsNullOrEmpty(exit.Description))
            {
                exit.Description = exit.Description.Replace(oldDir.ToString(), newDir.ToString());
            }

            player.WriteLine($"Exit {exit.Id} direction set to {newDir}.");
            return true;
        }
        #endregion

        #region RoomSetExitType Method
        public static bool RoomSetExitType(Player player, List<string> parameters, Room currentRoom, Exit exit)
        {
            // type requires the exit to be in the current room
            if (exit.SourceRoomId != currentRoom.Id)
            {
                player.WriteLine("You can only change the type of exits that belong to the current room.");
                return false;
            }

            if (parameters.Count < 6)
            {
                player.WriteLine("Usage: /room set exit type <exitId> <Open|Door|LockedDoor|Impassable>");
                return false;
            }

            if (!Enum.TryParse(parameters[5], true, out ExitType newType))
            {
                player.WriteLine("Invalid exit type.");
                return false;
            }

            exit.ExitType = newType;
            exit.ApplyDefaultsForType();

            // Update mirrored return exit type if present
            var returnExit = Room.FindReturnExit(exit.SourceRoomId, exit.SourceAreaId, exit.DestinationRoomId);
            if (returnExit != null)
            {
                returnExit.ExitType = newType;
                returnExit.ApplyDefaultsForType();
            }

            player.WriteLine($"Exit {exit.Id} type set to {newType}.");
            return true;
        }
        #endregion


        #region RoomSetExitOpen Method
        public static bool RoomSetExitOpen(Player player, List<string> parameters, Exit exit)
        {
            if (parameters.Count < 6)
            {
                player.WriteLine("Usage: /room set exit open <exitId> <open|close>");
                return false;
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
                return false;
            }

            // Allow toggling doors even if they are not in the current room
            if (exit.ExitType != ExitType.Door && exit.ExitType != ExitType.LockedDoor)
            {
                player.WriteLine("This exit cannot be opened or closed (only doors can be toggled).");
                return false;
            }

            exit.IsOpen = setOpen.Value;

            // Update return exit state if present and if its type supports toggling
            var returnExit = Room.FindReturnExit(exit.SourceRoomId, exit.SourceAreaId, exit.DestinationRoomId);
            if (returnExit != null && (returnExit.ExitType == ExitType.Door || returnExit.ExitType == ExitType.LockedDoor))
            {
                returnExit.IsOpen = setOpen.Value;
            }

            player.WriteLine($"Exit {exit.Id} {(setOpen.Value ? "opened" : "closed")}.");
            return true;
        }
        #endregion

        #region RoomSetExit Method
        // CODE REVIEW: Ashten PR #18
        // This is a big method with a lot of logic - consider breaking it into smaller helper methods for each subcommand.
        // Some things like FindReturnExit might make sense as part of the Exit class so others could use it too.
        // When searching for an exit id, keep in mind that they IDs are NOT unique across areas.
        // Overall a great set of features for exit management!
        // You can delete this once you've read it. Let me know if you have any questions.

        /// <summary>
        /// /room set exit ...
        /// Supports:
        ///   /room set exit dir <exitId> <direction>
        ///   /room set exit dest <exitId> <roomId>   (or <areaId>:<roomId>)
        ///   /room set exit type <exitId> <Open|Door|LockedDoor|Impassable>
        ///   /room set exit open <exitId> <open|close>
        /// All require Builder permission.
        /// </summary>
        private static bool RoomSetExit(Player player, List<string> parameters)
        {
            if (parameters.Count < 4)
            {
                return ShowHelp(player);
            }            

            var subCommand = parameters[3].ToLower();
            Room currentRoom = player.GetRoom();

            // CODE REVIEW: Ashten - Shouldn't need this check since GetRoom() should never return null for a Player.
            if (currentRoom == null)
            {
                player.WriteLine("You are not in a valid room.");
                return false;
            }

            // Expect exit id for all subcommands
            if (parameters.Count < 5)
            {
                player.WriteLine("Usage: /room set exit <dir|dest|type|open> <exitId> <...>");
                return false;
            }

            // Allow specifying the exit with an optional area prefix: <areaId>:<exitId> or just <exitId>
            int exitAreaId = player.AreaId;
            int exitId;
            string exitParam = parameters[4];
            if (exitParam.Contains(":"))
            {
                var parts = exitParam.Split(':');
                if (parts.Length != 2
                    || !int.TryParse(parts[0], out exitAreaId)
                    || !int.TryParse(parts[1], out exitId))
                {
                    player.WriteLine("Invalid exit id format. Use <exitId> or <areaId>:<exitId>.");
                    return false;
                }
            }
            else
            {
                if (!int.TryParse(exitParam, out exitId))
                {
                    player.WriteLine("Invalid exit id.");
                    return false;
                }
            }

            // Find the exit in the specified area (allow builders to operate on exits in other areas by specifying area)
            if (!GameState.Instance.Areas.TryGetValue(exitAreaId, out Area? exitArea))
            {
                player.WriteLine($"Area {exitAreaId} not found.");
                return false;
            }

            Exit? exit = exitArea.Exits.GetValueOrDefault(exitId);

            if (exit == null)
            {
                player.WriteLine($"Exit id {exitId} not found in Area {exitAreaId}.");
                return false;
            }

            try
            {
                switch (subCommand)
                {
                    case "dir":
                        return RoomSetExitDir(player, parameters, exit);                       
                    case "dest":
                        return RoomSetExitDest(player, parameters, currentRoom, exit);
                    case "type":
                        return RoomSetExitType(player, parameters, currentRoom, exit);
                    case "open":
                        return RoomSetExitOpen(player, parameters, exit);
                    default:
                        return ShowHelp(player);
                }
            }
            catch (Exception ex)
            {
                player.WriteLine($"Error updating exit: {ex.Message}");
            }

            return false;
        }
        #endregion

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
            room.Tags = [.. tagInput.Split([','], StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim())];

            player.WriteLine("Room tags set: " + string.Join(", ", room.Tags));
        }
        private static bool RoomAreaList(Player player, List<string> parameters)
        {
            int areaId = player.AreaId;

            if (parameters.Count >= 4 && !int.TryParse(parameters[3], out areaId))
            {
                player.WriteLine("Invalid area id.");
                return false;
            }

            if (!GameState.Instance.Areas.TryGetValue(areaId, out Area? area))
            {
                player.WriteLine($"Area {areaId} not found.");
                return false;
            }

            if (area.Rooms.Count == 0)
            {
                player.WriteLine($"Area {areaId} has no rooms.");
                return true;
            }

            player.WriteLine($"Rooms in Area {areaId}:");

            foreach (var room in area.Rooms.Values.OrderBy(r => r.Id))
            {
                int exitCount = room.ExitIds?.Count ?? 0;
                player.WriteLine($"  [{room.Id}] {room.Name} (Exits: {exitCount})");
            }

            return true;
        }
        private static bool RoomSetArea(Player player, List<string> parameters)
        {
            // /room set area description <areaId>:<roomId> '<desc>'
            if (parameters.Count < 6)
            {
                player.WriteLine("Usage: /room set area description <areaId>:<roomId> '<description>'");
                return false;
            }

            if (parameters[3].ToLower() != "description")
            {
                ShowHelp(player);
                return false;
            }


            if (!Room.TryParseId(parameters[4], player.AreaId, out int roomId, out int areaId))
            {
                player.WriteLine("Invalid room id format. Use <roomId> or <areaId>:<roomId>.");
                return false;
            }

            if (!GameState.Instance.Areas.TryGetValue(areaId, out Area? area)
                || !area.Rooms.TryGetValue(roomId, out Room? room))
            {
                player.WriteLine($"Room not found (Area {areaId}, Room {roomId}).");
                return false;
            }

            room.Description = parameters[5];
            player.WriteLine($"Room {areaId}:{roomId} description updated.");
            return true;
        }
        #region RoomAdd Method
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
            if (destParam.Contains(':'))
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
            if (!GameState.Instance.Areas.TryGetValue(destAreaId, out Area? value)
                || !value.Rooms.TryGetValue(destRoomId, out Room? destRoom))
            {
                player.WriteLine($"Destination room not found (Area: {destAreaId}, Room: {destRoomId}).");
                return;
            }

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
        #endregion

        #region RoomRemove Method
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

            Room currentRoom = player.GetRoom();
            if (currentRoom == null)
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

                exitToRemove = currentRoom.GetExits().FirstOrDefault(e => e.Id == exitId);
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

                exitToRemove = currentRoom.GetExits().FirstOrDefault(e => e.ExitDirection == direction);
                if (exitToRemove == null)
                {
                    player.WriteLine($"No exit going {direction} found in this room.");
                    return;
                }
            }

            try
            {
                // Remove the exit from its area's exit dictionary and from the source room's ExitIds
                if (GameState.Instance.Areas.TryGetValue(currentRoom.AreaId, out Area? value)
                    && value.Exits.ContainsKey(exitToRemove.Id))
                {
                    value.Exits.Remove(exitToRemove.Id);
                }

                currentRoom.ExitIds.Remove(exitToRemove.Id);

                // Attempt to find and remove the return exit (if any) in the destination room's area
                int destRoomId = exitToRemove.DestinationRoomId; 
                int destAreaId = -1;

                // CODE REVIEW: Ashten - I don't think we should be looping through 
                // all areas. There might be several areas that contain a room with the given id.
                // this points to missing information in an exit. We should store the DestionAreaId as well as room
                // otherwise, there's no way to move between areas.
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
        #endregion

        private static void ShowCommand(Player player, List<string> parameters)
        {



            Room r = player.GetRoom();
            player.WriteLine($"Name: {r.Name}");
            player.WriteLine($"Id: {r.Id}");
            player.WriteLine($"Area Id: {r.AreaId}");
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
                Room? destRoom = null;

                // CODE REVIEW: Ashten - Since we are storing destination area and room, is there some reason
                // We need to loop through every room in the game?
                // I think we can just do:
                if (GameState.Instance.Areas.TryGetValue(e.DestinationAreaId, out Area? destArea)
                    && destArea != null)
                {
                    _ = destArea.Rooms.TryGetValue(e.DestinationRoomId, out destRoom);
                }

                /*
                 * // Resolve destination room by searching all areas (supports cross-area exits)

                int destAreaId = -1;



                foreach (var kvp in GameState.Instance.Areas)
                {
                    if (kvp.Value.Rooms.TryGetValue(e.DestinationRoomId, out Room? value))
                    {
                        destAreaId = kvp.Key;
                        destRoom = value;
                        break;
                    }
                }
                */

                string destName = destRoom != null ? destRoom.Name : "Unknown";
                // Show area:room id when available to avoid ambiguity across areas
                //string destId = destAreaId != -1 ? $"{destAreaId}:{e.DestinationRoomId}" : e.DestinationRoomId.ToString();
                string destId = $"{e.DestinationAreaId}:{e.DestinationRoomId}";

                // Include open/closed info
                string openState = e.IsOpen ? "Open" : "Closed";

                // Show direction alongside exit name (if present)
                string namePart = string.IsNullOrWhiteSpace(e.Name) ? "" : $" '{e.Name}'";
                player.WriteLine($"{e.ExitDirection}{namePart} -> {destName} (Id: {destId}) [[{e.ExitType}]] [[{openState}]] : {e.Description}");
            }//this code above very specifically needs [[ ]] instead of [ ]
        }

        #region --- New: /room area exit ---
        private static bool RoomArea(Player player, List<string> parameters)
        {
            // Usage: /room area exit <areaId> '<exit description>'
            if (!Utility.CheckPermission(player, PlayerRole.Builder))
            {
                player.WriteLine("You do not have permission to do that.");
                return false;
            }

            if (parameters.Count < 4)
            {
                ShowHelp(player);
                return false;
            }

            if (parameters[2].ToLower() != "exit")
            {
                ShowHelp(player);
                return false;
            }

            if (!int.TryParse(parameters[3], out int targetAreaId))
            {
                player.WriteLine("Invalid area id.");
                return false;
            }

            string exitDescription = parameters.Count > 4 ? parameters[4] : "A strange portal.";

            if (!GameState.Instance.Areas.TryGetValue(targetAreaId, out Area? targetArea))
            {
                player.WriteLine($"Area {targetAreaId} not found.");
                return false;
            }

            try
            {
                // Create a new room inside the target area
                Room newRoom = Room.CreateRoom(targetAreaId, $"Crossing to Area {targetAreaId}", $"A constructed crossing into area {targetAreaId}.");

                // If the target area has a room 0, create an in-area exit from room 0 west to newRoom
                if (targetArea.Rooms.TryGetValue(0, out Room? room0))
                {
                    // Add an exit from target area's room 0 west to the new room (return exit created inside the target area)
                    room0.AddExits(player, Direction.West, $"A passage to {newRoom.Name}", newRoom, returnExit: true);
                }

                // Add an exit from the current room to the newly created room (cross-area)
                // Create a return exit so players can travel back and forth between areas.
                Room current = player.GetRoom();
                // Use West direction for the created exit as requested
                current.AddExits(player, Direction.West, exitDescription, newRoom, returnExit: true);

                player.WriteLine($"Area exit created to Area {targetAreaId} Room {newRoom.Id}. Players can travel between the areas both ways.");
                return true;
            }
            catch (Exception ex)
            {
                player.WriteLine($"Error creating area exit: {ex.Message}");
                return false;
            }

        }
        #endregion
    }
    #endregion

    #region --- Exit Builder ---
    /// <summary>
    /// /room command for building and editing rooms.
    /// </summary>
    /// Change all room commands to exit.
    internal class ExitBuilderCommand : ICommand
    {
        public string Name => "/exit";

        public IEnumerable<string> Aliases => [];
        public string Help => "";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
            {
                return false;
            }

            // All /exit commands require admin, no need to check further down
            if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
            {
                player.WriteLine("You do not have permission to do that.");
                return false;
            }

            if (parameters.Count < 2)
            {
                ShowHelp(player);
                return false;
            }

            // Decide what to do based on the second parameter
            switch (parameters[1].ToLower())
            {
                case "create":
                    return ExitCreate(player, parameters);
                case "set":
                    return ExitSet(player, parameters);
                case "show":
                    return ShowCommand(player);
                default:
                    ShowHelp(player);
                    break;
            }

            return true;
        }

        #region ExitSet Method
        private static bool ExitSet(Player player, List<string> parameters)
        {
            if (parameters.Count < 5)
            {
                ShowHelp(player);
                return false;
            }
            switch (parameters[3].ToLower())
            {
                case "description":
                    return ExitSetDescription(player, parameters);
                case "name":
                    return ExitSetName(player, parameters);
                case "icon":
                    return ExitSetIcon(player, parameters);
                // As we add more settable properties, we can expand this switch
                default:
                    ShowHelp(player);
                    break;
            }
            return false;
        }
        #endregion

        #region ShowHelp Method
        private static void ShowHelp(Player player)
        {
            player.WriteLine("Usage: ");
            player.WriteLine("/room description '<set room desc to this>'");
            player.WriteLine("/room name '<set room name to this>'");
            player.WriteLine("/room create '<name>' '<description>' <exit direction> '<exit description>'");
        }
        #endregion

        #region ExitCreate Method
        private static bool ExitCreate(Player player, List<string> parameters)
        {
            if (!Utility.CheckPermission(player, PlayerRole.Admin))
            {
                player.WriteLine("You do not have permission to do that.");
                return false;
            }

            // 0: /room
            // 1: create
            // 2: name
            // 3: description
            // 4: exit direction
            // 5: exit description
            if (parameters.Count < 6)
            {
                ShowHelp(player);
                return false;
            }

            if (!Enum.TryParse(parameters[4], true, out Direction exitDirection))
            {
                player.WriteLine("Invalid exit direction.");
                return false;
            }

            try
            {
                // TODO Before creating the room, ensure there's no existing exit in that direction
                // Also, shouuld exit create be creating a room? maybe this is a copy of room create right now?
                Room room = Room.CreateRoom(player.AreaId, parameters[2], parameters[3]);

                player.GetRoom().AddExits(player, exitDirection, parameters[5], room);
                player.WriteLine("Room exit created.");
                return true;
            }
            catch (Exception ex)
            {
                player.WriteLine($"Error creating exit: {ex.Message}");
                player.WriteLine(ex.StackTrace ?? "");
            }
            return false;
        }
        #endregion

        #region ExitSetIcon Method
        private static bool ExitSetIcon(Player player, List<string> parameters)
        {
            if (parameters.Count < 4)
            {
                player.WriteLine($"Current room icon: {player.GetRoom().MapIcon}");
                return false;
            }

            // TODO: Should we check that it's a single character? Maybe we don't care.
            player.GetRoom().MapIcon = parameters[3];            
            player.WriteLine($"Room icon set to: {player.GetRoom().MapIcon}");
            return true;
        }
        #endregion

        #region ExitSetName Method
        private static bool ExitSetName(Player player, List<string> parameters)
        {
            if (parameters.Count < 5)
            {
                player.WriteLine(player.GetRoom().Name);
                return false;
            }

            Room r = player.GetRoom();
            Exit? e = r.GetExitByName(parameters[2]);
            if (e != null)
            {
                e.Name = parameters[4];
                player.WriteLine("Exit name set.");
                return true;
            }

            player.WriteLine($"Could not find exit {parameters[2]}.");
            return false;
        }
        #endregion

        #region ExitSetDescription Method
        private static bool ExitSetDescription(Player player, List<string> parameters)
        {
            if (parameters.Count < 5)
            {
                player.WriteLine(player.GetRoom().Name);
                return false;
            }

            Room r = player.GetRoom();
            Exit? e = r.GetExitByName(parameters[2]);
            if (e != null)
            {
                e.Description = parameters[4];
                player.WriteLine("Exit description set.");
                return true;
            }

            player.WriteLine($"Could not set description. {parameters[2]}.");
            return false;
        }
        #endregion

        #region ShowCommand Method
        private static bool ShowCommand(Player player)
        {
            Room r = player.GetRoom();
            player.WriteLine($"Name: {r.Name}");
            player.WriteLine($"Id: {r.Id}");
            player.WriteLine($"Area Id: {r.AreaId}");
            player.WriteLine($"Description: {r.Description}");

            // Show exits in the current room
            var exits = r.GetExits();
            if (exits == null || exits.Count == 0)
            {
                player.WriteLine("Exits: None");
                return true;
            }

            player.WriteLine("Exits:");
            foreach (var e in exits)
            {
                // Try to resolve destination room (rooms are stored per-area)
                Room? destRoom = null;
                if (GameState.Instance.Areas.TryGetValue(r.AreaId, out Area? value)
                    && value.Rooms.TryGetValue(e.DestinationRoomId, out Room? value1))
                {
                    destRoom = value1;
                }

                string destName = destRoom != null ? destRoom.Name : "Unknown";
                string destId = e.DestinationRoomId.ToString();

                // Include open/closed info
                string openState = e.IsOpen ? "Open" : "Closed";

                player.WriteLine($"{e.ExitDirection} -> {destName} (Id: {destId}) [[{e.ExitType}]] [[{openState}]] : {e.Description}");
            }//this code above very specifically needs [[ ]] instead of [ ]
            return true;
        }

        #endregion


    }
    #endregion

    #region --- Find Builder ---
    /// <summary>
    /// /find command for searching rooms and exits.
    /// </summary>
    internal class FindBuilderCommand : ICommand
    {
        public string Name => "/find";

        public IEnumerable<string> Aliases => ["/search", "/locate"];
        public string Help => "Search for rooms and exits by text.\nUsage: /find room '<text>' - Search room names and descriptions\n       /find exit '<text>' - Search exit descriptions and names";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
            {
                return false;
            }

            // All find commands require at least Builder role
            if (Utility.CheckPermission(player, PlayerRole.Builder) == false)
            {
                player.WriteLine("You do not have permission to do that.");
                return false;
            }

            if (parameters.Count < 2)
            {
                ShowHelp(player);
                return false;
            }

            // Decide what to search based on the second parameter
            switch (parameters[1].ToLower())
            {
                case "room":
                case "rooms":
                    return FindRoom(player, parameters);
                case "exit":
                case "exits":
                    return FindExit(player, parameters);
                case "help":
                case "?":
                    ShowHelp(player);
                    return true;
                default:
                    ShowHelp(player);
                    return false;
            }
        }

        #region FindRoom Method
        /// <summary>
        /// Search room names and descriptions in the current area (or all areas if specified).
        /// Usage: /find room '<search text>' [all]
        /// </summary>
        private static bool FindRoom(Player player, List<string> parameters)
        {
            if (parameters.Count < 3)
            {
                player.WriteLine("Usage: /find room '<search text>' [all]");
                player.WriteLine("Searches room names and descriptions in current area.");
                player.WriteLine("Add 'all' at the end to search all areas.");
                return false;
            }

            // Extract search text (handling quoted text)
            string searchText = ExtractSearchText(parameters, 2);
            if (string.IsNullOrWhiteSpace(searchText))
            {
                player.WriteLine("Please provide text to search for.");
                return false;
            }

            bool searchAllAreas = parameters.Any(p => p.Equals("all", StringComparison.OrdinalIgnoreCase));
            searchText = searchText.ToLowerInvariant();

            int matchCount = 0;
            int areaId = player.AreaId;

            player.WriteLine($"Searching for rooms containing: '{searchText}'");

            if (searchAllAreas)
            {
                player.WriteLine("Searching ALL areas...");
                // Search all areas
                foreach (var areaKvp in GameState.Instance.Areas)
                {
                    matchCount += SearchRoomsInArea(player, areaKvp.Key, areaKvp.Value, searchText);
                }
            }
            else
            {
                // Search only current area
                if (!GameState.Instance.Areas.TryGetValue(areaId, out Area? currentArea))
                {
                    player.WriteLine($"Current area {areaId} not found.");
                    return false;
                }

                player.WriteLine($"Searching in Area {areaId}...");
                matchCount = SearchRoomsInArea(player, areaId, currentArea, searchText);
            }

            player.WriteLine($"Found {matchCount} matching room(s).");
            return true;
        }

        /// <summary>
        /// Helper method to search rooms in a specific area.
        /// </summary>
        private static int SearchRoomsInArea(Player player, int areaId, Area area, string searchText)
        {
            int matchCount = 0;
            bool areaHeaderPrinted = false;

            foreach (var roomKvp in area.Rooms)
            {
                Room room = roomKvp.Value;
                bool nameMatch = room.Name?.ToLowerInvariant().Contains(searchText) ?? false;
                bool descMatch = room.Description?.ToLowerInvariant().Contains(searchText) ?? false;

                if (nameMatch || descMatch)
                {
                    if (!areaHeaderPrinted)
                    {
                        player.WriteLine($"--- Area {areaId} ---");
                        areaHeaderPrinted = true;
                    }

                    matchCount++;
                    string matchType = nameMatch && descMatch ? "Name & Description" :
                                      nameMatch ? "Name" : "Description";

                    // Get exit count
                    int exitCount = room.ExitIds?.Count ?? 0;

                    player.WriteLine($"  [{areaId}:{room.Id}] {room.Name}");
                    player.WriteLine($"    Match: {matchType}, Exits: {exitCount}");

                    // Show a preview of the matching text
                    if (nameMatch)
                    {
                        string namePreview = GetPreviewText(room.Name, searchText);
                        player.WriteLine($"    Name: ...{namePreview}...");
                    }
                    if (descMatch && !nameMatch) // Only show description preview if name didn't match
                    {
                        string descPreview = GetPreviewText(room.Description, searchText);
                        player.WriteLine($"    Description: ...{descPreview}...");
                    }
                }
            }

            return matchCount;
        }
        #endregion

        #region FindExit Method
        /// <summary>
        /// Search exit descriptions and names in the current area (or all areas if specified).
        /// Usage: /find exit '<search text>' [all]
        /// </summary>
        private static bool FindExit(Player player, List<string> parameters)
        {
            if (parameters.Count < 3)
            {
                player.WriteLine("Usage: /find exit '<search text>' [all]");
                player.WriteLine("Searches exit names and descriptions in current area.");
                player.WriteLine("Add 'all' at the end to search all areas.");
                return false;
            }

            // Extract search text (handling quoted text)
            string searchText = ExtractSearchText(parameters, 2);
            if (string.IsNullOrWhiteSpace(searchText))
            {
                player.WriteLine("Please provide text to search for.");
                return false;
            }

            bool searchAllAreas = parameters.Any(p => p.Equals("all", StringComparison.OrdinalIgnoreCase));
            searchText = searchText.ToLowerInvariant();

            int matchCount = 0;
            int areaId = player.AreaId;

            player.WriteLine($"Searching for exits containing: '{searchText}'");

            if (searchAllAreas)
            {
                player.WriteLine("Searching ALL areas...");
                // Search all areas
                foreach (var areaKvp in GameState.Instance.Areas)
                {
                    matchCount += SearchExitsInArea(player, areaKvp.Key, areaKvp.Value, searchText);
                }
            }
            else
            {
                // Search only current area
                if (!GameState.Instance.Areas.TryGetValue(areaId, out Area? currentArea))
                {
                    player.WriteLine($"Current area {areaId} not found.");
                    return false;
                }

                player.WriteLine($"Searching in Area {areaId}...");
                matchCount = SearchExitsInArea(player, areaId, currentArea, searchText);
            }

            player.WriteLine($"Found {matchCount} matching exit(s).");
            return true;
        }

        /// <summary>
        /// Helper method to search exits in a specific area.
        /// </summary>
        private static int SearchExitsInArea(Player player, int areaId, Area area, string searchText)
        {
            int matchCount = 0;
            bool areaHeaderPrinted = false;

            foreach (var exitKvp in area.Exits)
            {
                Exit exit = exitKvp.Value;
                bool nameMatch = exit.Name?.ToLowerInvariant().Contains(searchText) ?? false;
                bool descMatch = exit.Description?.ToLowerInvariant().Contains(searchText) ?? false;

                if (nameMatch || descMatch)
                {
                    if (!areaHeaderPrinted)
                    {
                        player.WriteLine($"--- Area {areaId} ---");
                        areaHeaderPrinted = true;
                    }

                    matchCount++;

                    // Try to get source and destination room info
                    string sourceRoomName = "Unknown";
                    string destRoomName = "Unknown";

                    if (area.Rooms.TryGetValue(exit.SourceRoomId, out Room? sourceRoom))
                    {
                        sourceRoomName = sourceRoom.Name;
                    }

                    if (GameState.Instance.Areas.TryGetValue(exit.DestinationAreaId, out Area? destArea) &&
                        destArea.Rooms.TryGetValue(exit.DestinationRoomId, out Room? destRoom))
                    {
                        destRoomName = destRoom.Name;
                    }

                    string matchType = nameMatch && descMatch ? "Name & Description" :
                                      nameMatch ? "Name" : "Description";

                    player.WriteLine($"  Exit [{areaId}:{exit.Id}] {exit.ExitDirection}");
                    player.WriteLine($"    From: [{areaId}:{exit.SourceRoomId}] {sourceRoomName}");
                    player.WriteLine($"    To: [{exit.DestinationAreaId}:{exit.DestinationRoomId}] {destRoomName}");
                    player.WriteLine($"    Match: {matchType}, Type: {exit.ExitType}, Open: {exit.IsOpen}");

                    // Show a preview of the matching text
                    if (nameMatch)
                    {
                        string namePreview = GetPreviewText(exit.Name, searchText);
                        player.WriteLine($"    Name: ...{namePreview}...");
                    }
                    if (descMatch)
                    {
                        string descPreview = GetPreviewText(exit.Description, searchText);
                        player.WriteLine($"    Description: ...{descPreview}...");
                    }
                }
            }

            return matchCount;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Extract search text from parameters, handling quoted strings.
        /// </summary>
        private static string ExtractSearchText(List<string> parameters, int startIndex)
        {
            // Check if the parameter at startIndex starts with a quote
            if (startIndex < parameters.Count && parameters[startIndex].StartsWith('"'))
            {
                // Join parameters from startIndex and remove surrounding quotes
                string joined = string.Join(" ", parameters.Skip(startIndex));
                if (joined.StartsWith('"') && joined.EndsWith('"'))
                {
                    return joined[1..^1]; // Remove surrounding quotes
                }
                return joined.TrimStart('"'); // Just remove starting quote if no ending quote
            }

            // Simple case: just use the parameter at startIndex
            return startIndex < parameters.Count ? parameters[startIndex] : "";
        }

        /// <summary>
        /// Get a preview of text containing the search term.
        /// Shows ~40 characters around the match.
        /// </summary>
        private static string GetPreviewText(string? text, string searchText)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            string lowerText = text.ToLowerInvariant();
            int matchIndex = lowerText.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);

            if (matchIndex < 0)
                return "";

            int start = Math.Max(0, matchIndex - 20);
            int length = Math.Min(40 + searchText.Length, text.Length - start);
            string preview = text.Substring(start, length);

            // Add ellipsis if we didn't get the full text
            if (start > 0)
                preview = "..." + preview;
            if (start + length < text.Length)
                preview = preview + "...";

            return preview;
        }

        /// <summary>
        /// Show help for the /find command.
        /// </summary>
        private static void ShowHelp(Player player)
        {
            player.WriteLine("=== Find Command Help ===");
            player.WriteLine("/find room '<text>' [all] - Search room names and descriptions");
            player.WriteLine("  Adds 'all' to search all areas instead of just current area.");
            player.WriteLine("");
            player.WriteLine("/find exit '<text>' [all] - Search exit descriptions and names");
            player.WriteLine("  Adds 'all' to search all areas instead of just current area.");
            player.WriteLine("");
            player.WriteLine("Examples:");
            player.WriteLine("  /find room 'castle'      - Search for 'castle' in current area rooms");
            player.WriteLine("  /find exit 'door' all    - Search for 'door' in exits in all areas");
            player.WriteLine("  /find room 'dark cave'   - Search for phrase 'dark cave'");
        }
        #endregion
    }
    #endregion

    #region --- Area Builder ---
    /// <summary>
    /// /area command for building and editing areas.
    /// Supports:
    ///   /area create '<name>' '<description>' [<roomIdToMove>]
    ///   /area move <roomId> <areaId>
    ///   /area show <areaId>
    ///   /area list
    /// The create and move operations will update room AreaId and relocate the room & its source exits to the new area.
    /// </summary>
    internal class AreaBuilderCommand : ICommand
    {
        public string Name => "/area";

        public IEnumerable<string> Aliases => Array.Empty<string>();
       public string Help => "/area create '<name>' '<description>' [[<roomIdToMove>]]";

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

            switch (parameters[1].ToLower())
            {
                case "create":
                    AreaCreate(player, parameters);
                    break;
                case "move":
                    AreaMove(player, parameters);
                    break;
                case "show":
                    AreaShow(player, parameters);
                    break;
                case "list":
                    AreaList(player);
                    break;
                default:
                    WriteUsage(player);
                    break;
            }

            return true;
        }

        private static void WriteUsage(Player player)
        {
            player.WriteLine("Usage:");
            player.WriteLine("/area create '<name>' '<description>' [[<roomIdToMove>]]");
            player.WriteLine("/area move <roomId> <areaId>");
            player.WriteLine("/area show <areaId>");
            player.WriteLine("/area list");
        }

        private static void AreaList(Player player)
        {
            if (GameState.Instance.Areas.Count == 0)
            {
                player.WriteLine("No areas loaded.");
                return;
            }

            foreach (var kvp in GameState.Instance.Areas.OrderBy(k => k.Key))
            {
                player.WriteLine($"Area {kvp.Key}: {kvp.Value.Name} - {kvp.Value.Description} (Rooms: {kvp.Value.Rooms.Count})");
            }
        }

        private static void AreaShow(Player player, List<string> parameters)
        {
            if (parameters.Count < 3 || !int.TryParse(parameters[2], out int areaId))
            {
                player.WriteLine("Usage: /area show <areaId>");
                return;
            }

            if (!GameState.Instance.Areas.ContainsKey(areaId))
            {
                player.WriteLine($"Area {areaId} not found.");
                return;
            }

            var a = GameState.Instance.Areas[areaId];
            player.WriteLine($"Area {a.Id}: {a.Name}");
            player.WriteLine($"Description: {a.Description}");
            player.WriteLine($"Rooms: {a.Rooms.Count}");
            player.WriteLine($"Exits: {a.Exits.Count}");
        }

        private static void AreaCreate(Player player, List<string> parameters)
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

            string name = parameters[2];
            string description = parameters[3];

            int newAreaId = GetNextAreaId();

            var newArea = new Area
            {
                Id = newAreaId,
                Name = name,
                Description = description
            };

            GameState.Instance.Areas.Add(newAreaId, newArea);
            player.WriteLine($"Area created: Id {newAreaId} Name '{name}'");

            // Optional: move a room into the new area (parameters[4])
            if (parameters.Count >= 5 && int.TryParse(parameters[4], out int roomToMoveId))
            {
                if (!TryFindRoom(roomToMoveId, out int srcAreaId, out Room srcRoom))
                {
                    player.WriteLine($"Room id {roomToMoveId} not found in any area.");
                    return;
                }

                try
                {
                    int assignedNewRoomId = MoveRoomToArea(srcAreaId, srcRoom.Id, newAreaId);
                    player.WriteLine($"Room moved to new area. Old: Area {srcAreaId} Room {roomToMoveId} -> New: Area {newAreaId} Room {assignedNewRoomId}");
                }
                catch (Exception ex)
                {
                    player.WriteLine($"Error moving room: {ex.Message}");
                }
            }
        }

        private static void AreaMove(Player player, List<string> parameters)
        {
            if (!Utility.CheckPermission(player, PlayerRole.Builder))
            {
                player.WriteLine("You do not have permission to do that.");
                player.WriteLine("Your Role is: " + player.PlayerRole.ToString());
                return;
            }

            if (parameters.Count < 4
                || !int.TryParse(parameters[2], out int roomId)
                || !int.TryParse(parameters[3], out int targetAreaId))
            {
                WriteUsage(player);
                return;
            }

            if (!GameState.Instance.Areas.ContainsKey(targetAreaId))
            {
                player.WriteLine($"Target area {targetAreaId} does not exist.");
                return;
            }

            if (!TryFindRoom(roomId, out int srcAreaId, out Room srcRoom))
            {
                player.WriteLine($"Room id {roomId} not found in any area.");
                return;
            }

            try
            {
                int newRoomId = MoveRoomToArea(srcAreaId, srcRoom.Id, targetAreaId);
                player.WriteLine($"Room moved: Old Area {srcAreaId} Room {roomId} -> New Area {targetAreaId} Room {newRoomId}");
            }
            catch (Exception ex)
            {
                player.WriteLine($"Error moving room: {ex.Message}");
            }
        }

        /// <summary>
        /// Locate a room by id across all areas. Returns source area id and room instance if found.
        /// </summary>
        private static bool TryFindRoom(int roomId, out int areaId, out Room room)
        {
            foreach (var kvp in GameState.Instance.Areas)
            {
                if (kvp.Value.Rooms.ContainsKey(roomId))
                {
                    areaId = kvp.Key;
                    room = kvp.Value.Rooms[roomId];
                    return true;
                }
            }

            areaId = -1;
            room = null!;
            return false;
        }

        /// <summary>
        /// Move a room (and its source exits) from srcAreaId to destAreaId.
        /// This assigns a new room id in the destination area and updates exits, players and return-exit destinations.
        /// Returns the new room id assigned in the destination area.
        /// </summary>
        private static int MoveRoomToArea(int srcAreaId, int srcRoomId, int destAreaId)
        {
            if (srcAreaId == destAreaId)
                throw new InvalidOperationException("Source and destination area are the same.");

            if (!GameState.Instance.Areas.ContainsKey(srcAreaId))
                throw new KeyNotFoundException($"Source area {srcAreaId} not found.");
            if (!GameState.Instance.Areas.ContainsKey(destAreaId))
                throw new KeyNotFoundException($"Destination area {destAreaId} not found.");

            var srcArea = GameState.Instance.Areas[srcAreaId];
            var destArea = GameState.Instance.Areas[destAreaId];

            if (!srcArea.Rooms.ContainsKey(srcRoomId))
                throw new KeyNotFoundException($"Room {srcRoomId} not found in area {srcAreaId}.");

            // Take the room instance
            Room room = srcArea.Rooms[srcRoomId];

            // Assign a new room id in destination area
            int newRoomId = Room.GetNextId(destAreaId);

            // Remove room from source area's room map
            srcArea.Rooms.Remove(srcRoomId);

            // Update room id and area
            int oldRoomId = room.Id;
            room.Id = newRoomId;
            room.AreaId = destAreaId;

            // Add to destination area's room map
            destArea.Rooms.Add(newRoomId, room);

            // Move any exits that originate from this room into the destination area.
            // For each source exit, we remove from srcArea.Exits and add to destArea.Exits with a new exit id.
            var sourceExits = srcArea.Exits.Values.Where(e => e.SourceRoomId == oldRoomId).ToList();
            var oldToNewExitId = new Dictionary<int, int>();

            foreach (var exit in sourceExits)
            {
                // remove from source area
                srcArea.Exits.Remove(exit.Id);

                int oldExitId = exit.Id;
                int newExitId = Exit.GetNextId(destAreaId);

                // update exit's fields
                exit.Id = newExitId;
                exit.SourceRoomId = newRoomId;

                // add to dest area
                destArea.Exits.Add(exit.Id, exit);

                // update room's ExitIds list (replace old id with new)
                room.ExitIds.Remove(oldExitId);
                room.ExitIds.Add(newExitId);

                oldToNewExitId[oldExitId] = newExitId;

                // Update return exits in other areas that pointed back to the old room id:
                foreach (var kvp in GameState.Instance.Areas)
                {
                    var area = kvp.Value;
                    // find return exits that had DestinationRoomId == oldRoomId and SourceRoomId == exit.DestinationRoomId
                    var returnExits = area.Exits.Values.Where(e =>
                        e.DestinationRoomId == oldRoomId && e.SourceRoomId == exit.DestinationRoomId).ToList();

                    foreach (var ret in returnExits)
                    {
                        ret.DestinationRoomId = newRoomId;
                    }
                }
            }

            // Update any exits across all areas that had DestinationRoomId == oldRoomId (incoming links) to point to newRoomId.
            foreach (var kvp in GameState.Instance.Areas)
            {
                var area = kvp.Value;
                foreach (var incoming in area.Exits.Values.Where(e => e.DestinationRoomId == oldRoomId).ToList())
                {
                    incoming.DestinationRoomId = newRoomId;
                }
            }

            // Move any players that were in the room (preserve their LocationId with the new room id and set AreaId)
            foreach (var player in GameState.Instance.Players.Values)
            {
                if (!player.IsOnline) continue;
                if (player.AreaId == srcAreaId && player.LocationId == srcRoomId)
                {
                    player.AreaId = destAreaId;
                    player.LocationId = newRoomId;
                }
            }

            return newRoomId;
        }

        private static int GetNextAreaId()
        {
            if (GameState.Instance.Areas.Count == 0) return 0;
            return GameState.Instance.Areas.Keys.Max() + 1;
        }
    }
    #endregion
}
