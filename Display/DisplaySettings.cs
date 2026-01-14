
namespace RPGFramework.Display
{
    internal static class DisplaySettings
    {
        public static string AnnouncementColor { get; set; } = "[yellow]";

        public static string HeaderColor { get; set; } = "[deepskyblue1]";

        public static string WarningColor { get; set; } = "[mediumpurple1]";

        public static string ErrorColor { get; set; } = "[red]";

        public static string SuccessColor { get; set; } = "[green3]";

        #region Map Settings
        public static string RoomMapIcon { get; set; } = "■";
        public static string RoomMapIconColor { get; set; } = "[green]";
        public static string YouAreHereMapIcon { get; set; } = "🙂";
        public static string YouAreHereMapIconColor { get; set; } = "[bold black]";
        #endregion
    }
}
