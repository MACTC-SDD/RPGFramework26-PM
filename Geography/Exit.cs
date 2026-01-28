using RPGFramework.Enums;

namespace RPGFramework.Geography
{
    internal class Exit
    {
        #region --- Properties ---
        public int Id { get; set; } = 0;
        public Direction ExitDirection { get; set; }
        public ExitType ExitType { get; set; } = ExitType.Open;
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int SourceAreaId { get; set; } = 0;
        public int SourceRoomId { get; set; }
        public int DestinationRoomId { get; set; }
        public int DestinationAreaId { get; set; } = 0;

        // whether the exit is currently open (can be traversed)
        public bool IsOpen { get; set; } = true;
        #endregion

        /// <summary>
        /// Applies sensible default open/close state based on ExitType.
        /// Open      -> IsOpen = true
        /// Door      -> IsOpen = true
        /// LockedDoor-> IsOpen = false
        /// Impassable-> IsOpen = false
        /// </summary>
        public void ApplyDefaultsForType()
        {
            switch (ExitType)
            {
                case ExitType.Open:
                    IsOpen = true;
                    break;
                case ExitType.Door:
                    IsOpen = true;
                    break;
                case ExitType.LockedDoor:
                    IsOpen = false;
                    break;
                case ExitType.Impassable:
                default:
                    IsOpen = false;
                    break;
            }
        }

        #region Delete Methods
        public static void Delete(int exitId, int sourceRoom, int areaId)
        {
            GameState.Instance.Areas[areaId].Exits.Remove(exitId);
            GameState.Instance.Areas[areaId].Rooms[sourceRoom].ExitIds.Remove(exitId);
        }

        public static void Delete(Exit exit)
        {
            Delete(exit.Id, exit.SourceRoomId, exit.SourceAreaId);
        }
        #endregion

        /// <summary>
        /// Finds the highest Exit ID for the current area in GameState and returns one higher
        /// This could lead to gaps in the ID sequence if we delete Exits, but that's ok for now.
        /// </summary>
        /// <returns></returns>
        public static int GetNextId(Area a)
        {
            return GetNextId(a.Id);         
        }

        public static int GetNextId(int areaId)
        {
            if (GameState.Instance.Areas[areaId].Exits.Count == 0)
            {
                return 0;
            }
            return GameState.Instance.Areas[areaId].Exits.Keys.Max() + 1;
            // Return one higher
        }


    }

}
