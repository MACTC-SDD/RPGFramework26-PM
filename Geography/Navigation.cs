using RPGFramework.Enums;
using RPGFramework.Workflows;
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
        /// This method will require confirmation when crossing area boundaries.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="direction"></param>
        public static void Move(Character character, Direction direction)
        {
            Room currentRoom = character.GetRoom();
            Exit? exit = currentRoom.GetExits().FirstOrDefault(e => e.ExitDirection == direction);

            // If invalid exit, send error message (if player)
            if (exit == null)
            {
                if (character is Player)
                {
                    Player p = (Player)character;
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

            // If this exit crosses areas, require confirmation
            if (exit.DestinationAreaId != exit.SourceAreaId && character is Player pChar)
            {
                // Set a workflow to confirm travel
                pChar.CurrentWorkflow = new WorkflowAreaTravelConfirm(exit.SourceAreaId, exit.Id);
                pChar.WriteLine("You are about to travel to another area. This action is irreversible and you will not be able to immediately return. Type YES to confirm.");
                return;
            }

            Room destinationRoom = GameState.Instance.Areas[character.AreaId].Rooms[exit.DestinationRoomId];

            currentRoom.LeaveRoom(character, destinationRoom);
            destinationRoom.EnterRoom(character, currentRoom);
            
            character.AreaId = destinationRoom.AreaId;
            character.LocationId = exit.DestinationRoomId;
        }

        /// <summary>
        /// Perform the move without prompting (used by workflows that have already confirmed).
        /// Optionally remove the return exit so the player cannot go back.
        /// </summary>
        public static void PerformMoveDirect(Character character, Exit exit, bool removeReturnExit = false)
        {
            Room currentRoom = GameState.Instance.Areas[character.AreaId].Rooms[character.LocationId];
            if (!GameState.Instance.Areas.TryGetValue(exit.DestinationAreaId, out Area? destArea)
                || !destArea.Rooms.TryGetValue(exit.DestinationRoomId, out Room? destinationRoom))
            {
                if (character is Player p)
                {
                    p.WriteLine("Destination room not found. Movement aborted.");
                }
                return;
            }

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
