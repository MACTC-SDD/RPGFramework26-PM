
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
    }
}

