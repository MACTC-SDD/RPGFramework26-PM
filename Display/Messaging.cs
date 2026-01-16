
namespace RPGFramework.Display
{
    internal class Messaging
    {public static string CreateAnnouncementMessage(string message)
        {
            return $"\n{DisplaySettings.AnnouncementColor}[[Announcement]]: [/][white] {message}[/]";
        }
    }
}
