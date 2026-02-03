using RPGFramework.Enums;
using RPGFramework.Geography;

namespace RPGFramework.Geography
{
    /// <summary>
    /// Primarily respoonsible for handling move commands (n, e, s, w, u, d)
    /// </summary>
    internal class Navigation
    {

        /// <summary>
        /// Move the character in the specified direction if possible, otherwise, send error.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="direction"></param>
        public static void Move(Character character, Direction direction)
        {
            // Resolve current room safely
            Area? currentArea = null;
            Room? currentRoom = null;

            // Use two-step TryGetValue so variables are definitely assigned for the compiler
            bool areaFound = GameState.Instance.Areas.TryGetValue(character.AreaId, out currentArea);
            bool roomFound = areaFound && currentArea.Rooms.TryGetValue(character.LocationId, out currentRoom);

            if (!areaFound || !roomFound)
            {
                // Try a reasonable fallback for current room (room 0 or first room in the area)
                if (currentArea != null)
                {
                    currentRoom = currentArea.Rooms.TryGetValue(0, out var r0) ? r0 : currentArea.Rooms.Values.FirstOrDefault();
                    if (currentRoom != null)
                    {
                        character.LocationId = currentRoom.Id;
                    }
                }

                if (currentRoom == null)
                {
                    if (character is Player p)
                    {
                        p.WriteLine("Your current location could not be resolved. Movement aborted.");
                    }
                    return;
                }
            }

            Exit? exit = currentRoom.GetExits().FirstOrDefault(e => e.ExitDirection == direction);

            // If invalid exit, send error message (if player)
            if (exit == null)
            {
                if (character is Player p)
                {
                    p.WriteLine("You can't go that way.");
                }
                return;
            }

            // Block impassable exits
            if (exit.ExitType == ExitType.Impassable)
            {
                if (character is Player p)
                {
                    p.WriteLine("You can't go that way.");
                }
                return;
            }

            // Block closed exits
            if (!exit.IsOpen)
            {
                if (character is Player p)
                {
                    // Provide a slightly different message for closed doors
                    p.WriteLine("The way is closed.");
                }
                return;
            }

            // If this exit crosses areas, perform the move immediately (no confirmation).
            // Keep return exits intact so travel is bidirectional.
            if (exit.DestinationAreaId != exit.SourceAreaId && character is Player pChar)
            {
                PerformMoveDirect(character, exit);
                pChar.WriteLine($"You travel to Area {exit.DestinationAreaId} Room {exit.DestinationRoomId}.");
                return;
            }

            // Same-area move: resolve destination safely and pick sensible fallback(s)
            Room? destinationRoom = null;
            if (GameState.Instance.Areas.TryGetValue(character.AreaId, out var areaForMove))
            {
                areaForMove.Rooms.TryGetValue(exit.DestinationRoomId, out destinationRoom);
            }

            // If destination not found in the player's current area, prefer moving to a room in the current area (per your request)
            if (destinationRoom == null)
            {
                // Prefer room 0 in current area
                if (areaForMove != null && areaForMove.Rooms.TryGetValue(0, out var r0))
                {
                    destinationRoom = r0;
                }
                // Otherwise pick the first available room in current area
                if (destinationRoom == null && areaForMove != null)
                {
                    destinationRoom = areaForMove.Rooms.Values.FirstOrDefault();
                }
            }

            // If still not found, try to find any area that has the requested room id
            if (destinationRoom == null)
            {
                foreach (var kvp in GameState.Instance.Areas)
                {
                    if (kvp.Value.Rooms.TryGetValue(exit.DestinationRoomId, out var r))
                    {
                        destinationRoom = r;
                        break;
                    }
                }
            }

            // Final fallback: pick the closest area (by numeric id) that has at least one room,
            // prefer current area if it's available (handled above).
            if (destinationRoom == null)
            {
                Area? closestArea = null;
                long bestDistance = long.MaxValue;
                foreach (var kvp in GameState.Instance.Areas)
                {
                    if (kvp.Value.Rooms.Count == 0) continue;
                    long distance = System.Math.Abs(kvp.Key - exit.DestinationAreaId);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        closestArea = kvp.Value;
                    }
                }

                if (closestArea != null)
                {
                    destinationRoom = closestArea.Rooms.TryGetValue(0, out var r0) ? r0 : closestArea.Rooms.Values.FirstOrDefault();
                }
            }

            if (destinationRoom == null)
            {
                if (character is Player p)
                {
                    p.WriteLine("Could not locate a sensible destination. Movement aborted.");
                }
                return;
            }

            currentRoom.LeaveRoom(character, destinationRoom);
            destinationRoom.EnterRoom(character, currentRoom);

            character.AreaId = destinationRoom.AreaId;
            character.LocationId = destinationRoom.Id;
        }

        /// <summary>
        /// Perform the move without prompting (used by workflows that have already confirmed).
        /// Optionally remove the return exit so the player cannot go back.
        /// If the destination area/room cannot be located, choose sensible fallbacks:
        ///  - prefer player's current area (room 0 or first room),
        ///  - then any area that contains the requested room id,
        ///  - then the closest area by numeric id that has rooms.
        /// </summary>
        public static void PerformMoveDirect(Character character, Exit exit, bool removeReturnExit = false)
        {
            // Resolve current room safely
            Room? currentRoom = null;
            if (GameState.Instance.Areas.TryGetValue(character.AreaId, out var currentArea))
            {
                currentArea.Rooms.TryGetValue(character.LocationId, out currentRoom);

                if (currentRoom == null)
                {
                    currentRoom = currentArea.Rooms.TryGetValue(0, out var r0) ? r0 : currentArea.Rooms.Values.FirstOrDefault();
                    if (currentRoom != null)
                    {
                        character.LocationId = currentRoom.Id;
                    }
                }
            }

            if (currentRoom == null)
            {
                if (character is Player p)
                {
                    p.WriteLine("Your current location could not be resolved. Movement aborted.");
                }
                return;
            }

            // Attempt to resolve the target area and room as specified by the exit.
            GameState.Instance.Areas.TryGetValue(exit.DestinationAreaId, out Area? destArea);
            Room? destinationRoom = null;
            if (destArea != null)
            {
                destArea.Rooms.TryGetValue(exit.DestinationRoomId, out destinationRoom);
            }

            // Prefer player's current area as the first fallback (per request)
            if (destinationRoom == null && GameState.Instance.Areas.TryGetValue(character.AreaId, out var playerArea))
            {
                // If the requested room id exists in the player's area, use it
                if (playerArea.Rooms.TryGetValue(exit.DestinationRoomId, out var sameAreaRoom))
                {
                    destinationRoom = sameAreaRoom;
                    destArea = playerArea;
                }
                else
                {
                    // Prefer room 0 in player's area or first room
                    destinationRoom = playerArea.Rooms.TryGetValue(0, out var r0) ? r0 : playerArea.Rooms.Values.FirstOrDefault();
                    destArea = playerArea;
                }
            }

            // If still not found, attempt to find the requested room id in any area
            if (destinationRoom == null)
            {
                foreach (var kvp in GameState.Instance.Areas)
                {
                    if (kvp.Value.Rooms.TryGetValue(exit.DestinationRoomId, out var r))
                    {
                        destArea = kvp.Value;
                        destinationRoom = r;
                        break;
                    }
                }
            }

            // If still not found, pick the "closest" area by numeric id that has at least one room.
            if (destinationRoom == null)
            {
                Area? closestArea = null;
                long bestDistance = long.MaxValue;
                foreach (var kvp in GameState.Instance.Areas)
                {
                    if (kvp.Value.Rooms.Count == 0) continue;
                    long distance = System.Math.Abs(kvp.Key - exit.DestinationAreaId);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        closestArea = kvp.Value;
                    }
                }

                if (closestArea != null)
                {
                    destArea = closestArea;
                    destinationRoom = closestArea.Rooms.TryGetValue(0, out var r0) ? r0 : closestArea.Rooms.Values.FirstOrDefault();
                }
            }

            // Final fallback: if we still don't have a room, keep the player in place
            if (destinationRoom == null || destArea == null)
            {
                if (character is Player p)
                {
                    p.WriteLine("Could not locate a fallback destination. Staying in current location.");
                }
                return;
            }

            // Perform the movement into the chosen destinationRoom
            currentRoom.LeaveRoom(character, destinationRoom);
            destinationRoom.EnterRoom(character, currentRoom);

            character.AreaId = destinationRoom.AreaId;
            character.LocationId = destinationRoom.Id;

            if (removeReturnExit)
            {
                // Attempt to find and delete the return exit (if any)
                var returnExit = Room.FindReturnExit(exit.SourceRoomId, exit.SourceAreaId, exit.DestinationRoomId);
                if (returnExit != null)
                {
                    Exit.Delete(returnExit);
                }
            }
        }

        public static Direction GetOppositeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return Direction.South;
                case Direction.South:
                    return Direction.North;
                case Direction.East:
                    return Direction.West;
                case Direction.West:
                    return Direction.East;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                default:
                    return Direction.None;
            }
        }
    }
}
