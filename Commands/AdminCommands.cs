
using RPGFramework.Core;
using RPGFramework.Display;

namespace RPGFramework.Commands
{
    internal class AdminCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return new List<ICommand>
            {
                new AnnounceCommand(),
                new ShutdownCommand(),
                new HelpEditCommand(),
                // Add more builder commands here as needed
            };
        }
    }

    internal class AnnounceCommand : ICommand
    {
        public string Name => "announce";
        public IEnumerable<string> Aliases => new List<string>() { "ann" };
        public bool Execute(Character character, List<string> parameters)
        {
            Comm.Broadcast($"{DisplaySettings.AnnouncementColor}[[Announcement]]: [/][white]" + 
                $"{string.Join(' ', parameters.Skip(1))}[/]");
            return true;
        }
    }

    internal class ShutdownCommand : ICommand
    {
        public string Name => "shutdown";
        public IEnumerable<string> Aliases => new List<string>() { };
        public bool Execute(Character character, List<string> parameters)
        {
            Comm.Broadcast($"{DisplaySettings.AnnouncementColor}[[WARNING]]: [/][white]" +
                $"Server is shutting down. All data will be saved.[/]");

            GameState.Instance.Stop();
            return true;
        }
    }
    internal class HelpEditCommand : ICommand
    {
        public string Name => "/help";
        public IEnumerable<string> Aliases => new List<string>() { };
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
            {
                return false;
            }

            if (parameters.Count > 1)
            {
                switch (parameters[1].ToLower())
                {
                    case "create":
                        CreateHelp(player, parameters);
                        break;
                    

                }
            }
            else
            {
                // Show usage message
            }

                // Do stuff
                return true;
        }

        public void CreateHelp(Player player, List<string> parameters)
        {
            HelpEntry h = new HelpEntry()
            {
                Name = parameters[2],
                Category = parameters[3],
                Content = parameters[4]
            };

            GameState.Instance.HelpEntries.Add(h);
        }
    }
}
