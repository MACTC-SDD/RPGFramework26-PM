
namespace RPGFramework.Display
{
    internal class Messaging
    {
        public static string CreateAnnouncementMessage(string message)
        {
            return $"\n{DisplaySettings.AnnouncementColor}[[Announcement]]: [/][white] {message}[/]";
        }
        public static string CreateTellMessage(string message)
            {
            return $"\n{DisplaySettings.TellColor}[[Tell]]: [/][lightgoldenrod2_1] {message}[/]";
        }
        public static string CreateErrorMessage(string message)
        {
            return $"\n{DisplaySettings.ErrorColor}[[Error]]: [/][lightpink1] {message}[/]";
        }
        public static string CreateSystemMessage(string message)
        {
            return $"\n{DisplaySettings.SystemMessageColor}[[System]]: [/][lightcyan1] {message}[/]";
        }
        public static string CreateCombatMessage(string message)
        {
            return $"\n{DisplaySettings.CombatMessageColor}[[Combat]]: [/][lightgreen1] {message}[/]";
        }
    }
}

