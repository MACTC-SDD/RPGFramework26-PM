using RPGFramework.Enums;
using RPGFramework.Geography;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Commands
{
    internal class CCCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return new List<ICommand>
            {
                new CCBuilderCommand(),
                // Add more test commands here as needed
            };
        }
    }
    internal class CCBuilderCommand : ICommand
    {
        public string Name => "/cc";

        public IEnumerable<string> Aliases => [];
        public string Help => "Character Class management commands (create, set, show, delete).\n"
            + "\n"
            + "\n";

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
                    return CCCreate(player, parameters);
                case "set":
                    return CCSet(player, parameters);

                    // We'll move setting name and description into this
                    //CCSet(player, parameters);
                    break;
                case "show":
                    //CCCommand(player);
                    break;
                case "add":
                    //CCAdd(player, parameters);
                    break;
                case "remove":
                    //CCRemove(player, parameters);
                    break;
                case "delete":
                //return CCDelete(player, parameters);
                default:
                    //CCHelp(player);
                    break;
            }

            return true;
        }

        #region RoomSet Method
        private bool CCSet(Player player, List<string> parameters)
        {
            if (parameters.Count < 5)
            {
                player.WriteLine("Usage: /cc set '<class name>' '<property>' '<value>'");
                return false;
            }
            if (!GameState.Instance.CCCatalog
                .TryGetValue(parameters[2], out CharacterClass? charClass) || charClass == null)
            {
                player.WriteLine($"Character class '{parameters[2]}' not found.");
                return false;
            }

            switch (parameters[3].ToLower())
            {
                case "description":
                    charClass.Description = parameters[4];
                    player.WriteLine("Description updated.");
                    player.WriteLine(charClass.Description);
                    return true;
                case "str":
                    if (int.TryParse(parameters[4], out int str))
                    {
                        charClass.StrengthMod = str;
                        player.WriteLine($"Strength modifier set to {str}.");
                        return true;
                    }
                    player.WriteLine("Invalid strength modifier value.");
                    return false;
                case "dex":
                    if (int.TryParse(parameters[4], out int dex))
                    {
                        charClass.DexterityMod = dex;
                        player.WriteLine($"Dexterity modifier set to {dex}.");
                        return true;
                    }
                    player.WriteLine("Invalid dexterity modifier value.");
                    return false;
                    case "con":
                        if (int.TryParse(parameters[4], out int con))
                    {
                        charClass.ConstitutionMod = con;
                        player.WriteLine($"Constitution modifier set to {con}.");
                        return true;
                    }
                        player.WriteLine("Invalid constitution modifier value.");
                    return false;
                    case "int":
                        if (int.TryParse(parameters[4], out int intel))
                    {
                        charClass.IntelligenceMod = intel;
                        player.WriteLine($"Intelligence modifier set to {intel}.");
                        return true;
                    }
                        player.WriteLine("Invalid intelligence modifier value.");
                    return false;
                    case "wis":
                        if (int.TryParse(parameters[4], out int wis))
                    {
                        charClass.WisdomMod = wis;
                        player.WriteLine($"Wisdom modifier set to {wis}.");
                        return true;
                    }
                        player.WriteLine("Invalid wisdom modifier value.");
                    return false;
                    case "cha":
                        if (int.TryParse(parameters[4], out int cha))
                    {
                        charClass.CharismaMod = cha;
                        player.WriteLine($"Charisma modifier set to {cha}.");
                        return true;
                    }
                        player.WriteLine("Invalid charisma modifier value.");
                    return false;            
                default:
                    return ShowHelp(player);
            }
        }
        #endregion

        #region ShowHelp Method
        private bool ShowHelp(Player player)
        {
            player.WriteLine(Help);
            player.WriteLine("/cc set <class name> description '<set desc to this>'");
            player.WriteLine("/cc set <class name> str <number>");
            player.WriteLine("/cc set <class name> dex <number>");
            player.WriteLine("/cc set <class name> con <number>");
            player.WriteLine("/cc set <class name> int <number>");
            player.WriteLine("/cc set <class name> wis <number>");
            player.WriteLine("/cc set <class name> cha <number>");


            //to see tags and desc and name etc, just do /room <name of thing> and nothing after

            return false;
        }
        #endregion

        #region RoomCreate Method
        private static bool CCCreate(Player player, List<string> parameters)
        {
            // 0: /cc
            // 1: create
            // 2: name
            // 3: description
            // 4: str
            // 5: int
            // 6: dex
            // 7: con
            // 8: wis
            // 9: cha
            if (parameters.Count < 10)
            {
                player.WriteLine("Usage: /cc create '<name>' '<description>' '<str>' '<int>' '<dex>' '<con>' '<wis>' '<cha>'");
                return false;
            }

            if (!int.TryParse(parameters[4], out int str))
            {
                player.WriteLine("Invalid strength value.");
                return false;
            }
            if (!int.TryParse(parameters[5], out int intel))
            {
                player.WriteLine("Invalid intelligence value.");
                return false;
            }
            if (!int.TryParse(parameters[6], out int dex))
            {
                player.WriteLine("Invalid dexterity value.");
                return false;
            }
            if (!int.TryParse(parameters[7], out int con))
            {
                player.WriteLine("Invalid constitution value.");
                return false;
            }
            if (!int.TryParse(parameters[8], out int wis))
            {
                player.WriteLine("Invalid wisdom value.");
                return false;
            }
            if (!int.TryParse(parameters[9], out int cha))
            {
                player.WriteLine("Invalid charisma value.");
                return false;
            }

            if (GameState.Instance.CCCatalog.TryGetValue(parameters[2], out CharacterClass? _))
            {
                player.WriteLine("A character class with that name already exists.");
                return false;
            }

            CharacterClass newClass = new CharacterClass
            {
                Name = parameters[2],
                Description = parameters[3],
                StrengthMod = str,
                CharismaMod = str,
                IntelligenceMod = intel,
                DexterityMod = dex,
                ConstitutionMod = con,
                WisdomMod = wis,
            };

            GameState.Instance.CCCatalog.Add(newClass.Name, newClass);
            player.WriteLine($"Character class '{newClass.Name}' created.");
            return true;
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

            // Find destination area's room and check its exits
            /*int destAreaId = -1;
            foreach (var kvp in GameState.Instance.Areas)
            {
                if (kvp.Value.Rooms.ContainsKey(exit.DestinationRoomId))
                {
                    destAreaId = kvp.Key;
                    break;
                }
            }*/


            var destRoom = GameState.Instance.Areas[exit.DestinationAreaId].Rooms[exit.DestinationRoomId];
            // If some other exit (not the returnExit) already uses that opposite direction, fail.
            // Only check this if we found a return exit.
            //if (destRoom.GetExits().Any(e => e.ExitDirection == opposite && (returnExit == null || e.Id != returnExit.Id)))
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

        private static void ShowCommand(Player player)
        {
            Room r = player.GetRoom();
            player.WriteLine($"Name: {r.Name}");
            player.WriteLine($"Id: {r.Id}");
            player.WriteLine($"Area Id: {r.AreaId}");
            player.WriteLine($"Description: {r.Description}");
        }
    }
}



