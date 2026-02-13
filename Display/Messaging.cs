
namespace RPGFramework.Display
{
    internal class Messaging
    {
        public static string CreateAnnouncementMessage(string message, Player? player=null)
        {
            string sender = player?.DisplayName() ?? "ANON";
            return $"\n{DisplaySettings.AnnouncementColor}[[Announcement({sender})]]: [/][white] {message}[/]";
        }

        public static string CreateTellMessage(string senderName, string message)
            {
            return $"\n{DisplaySettings.TellColor}[[Tell from [cornflowerblue]{senderName}[/]]]: [/][lightgoldenrod2_1] {message}[/]";
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

