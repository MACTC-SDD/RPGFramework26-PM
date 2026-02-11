
using System.Net.NetworkInformation;

namespace RPGFramework.Display
{
    internal static class DisplaySettings
    {
        public static string AnnouncementColor { get; set; } = "[yellow]";

        public static string ErrorColor { get; set; } = "[red]";

        public static string TellColor { get; set; } = "[darkorange]";

        public static string SystemMessageColor { get; set; } = "[deepskyblue1]";

        public static string CombatMessageColor { get; set; } = "[green3]";

        #region Map Settings
        public static string RoomMapIcon { get; set; } = "■";
        public static string RoomMapIconColor { get; set; } = "[green]";
        public static string YouAreHereMapIcon { get; set; } = "🙂";
        public static string YouAreHereMapIconColor { get; set; } = "[bold yellow]";
        #endregion
    }
}
