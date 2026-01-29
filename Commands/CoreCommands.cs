
using RPGFramework.Core;
using RPGFramework.Display;
using RPGFramework.Geography;
using Spectre.Console;
using RPGFramework.Enums;
using System.Collections.Immutable;
using RPGFramework.Items;
using System.ComponentModel;
namespace RPGFramework.Commands
{
    /// <summary>
    /// Provides access to the set of built-in core command implementations.
    /// </summary>
    /// <remarks>The <c>CoreCommands</c> class exposes static methods for retrieving all available core
    /// commands. These commands represent fundamental operations supported by the system </remarks>
    internal class CoreCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return new List<ICommand>
            {
                new AFKCommand(),
                new IpCommand(),
                new LookCommand(),
                new QuitCommand(),
                new SayCommand(),
                new TellCommand(),
                new TimeCommand(),
                new StatusCommand(),
                new HelpCommand(),
                new EquipmentCommand(),
                new UseCommand(),
                // Add other core commands here as they are implemented
            };
        }


    }

    internal class EquipmentCommand : ICommand
    {
        public string Name => "equip";
        public IEnumerable<string> Aliases => new List<string> { };
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            List<Armor> armorItems = new List<Armor>();
            foreach (Item i in player.BackPack.Items)
            {
                if (i is Armor a)
                { armorItems.Add(a); }
            }
            


            return false;
        }
    }
    internal class UseCommand : ICommand
    {
        public string Name => "use";
        public IEnumerable<string> Aliases => new List<string> { };
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            // check if 2 params

            // find obj in inv that matches p[1]

            // is it consum

            Item? i = player.BackPack.GetItemByName(parameters[1]);
            Consumable? c = null;

            if (i == null || i is not Consumable)
            {
                // not coukgt find
            }
            else
            {
                c = (Consumable)i;
                if (c.UsesLeft > 0)
                {
                    c.UsesLeft--;
                    //c.Use();

                }
            }

            return true;



        }
    }


    

    internal class AFKCommand : ICommand
    {
        public string Name => "afk";
        public IEnumerable<string> Aliases => new List<string> { };
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.IsAFK = !player.IsAFK;
                player.WriteLine($"You are now {(player.IsAFK ? "AFK" : "no longer AFK")}.");
                return true;
            }
            return false;
        }
    }


    internal class IpCommand : ICommand
    {
        public string Name => "ip";
        public IEnumerable<string> Aliases => new List<string> { };
        public bool Execute(Character character, List<string> parameters)
        {           
            if (character is Player player)
            {
                player.WriteLine($"Your IP address is {player.GetIPAddress()}");
                return true;
            }
            return false;
        }
    }

    internal class LookCommand : ICommand
    {
        public string Name => "look";
        public IEnumerable<string> Aliases => new List<string> { "l" };
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                // For now, we'll ignore the command and just show the room description
                string content = $"{player.GetRoom().Description}\n";
                content += "[red]Exits:[/]\n";
                foreach (var exit in player.GetRoom().GetExits())
                {
                    content += $"{exit.Description} to the {exit.ExitDirection}\n";
                }
                content += "[Green]Players Here:[/]\n";
                content += $"{player.DisplayName()}";
                Panel panel = RPGPanel.GetPanel(content, player.GetRoom().Name);
                player.Write(panel);

                var table = new Table();
                table.AddColumn("[deepskyblue1]Items Here:[/]").Centered();

                foreach (Item item in player.GetRoom().Items)
                {
                    table.AddRow($"{item.DisplayText}");
                }
                return true;
            }
            return false;
        }
    }

    internal class QuitCommand : ICommand
    {
        public string Name => "quit";
        public IEnumerable<string> Aliases => new List<string> { "exit" };

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.Logout();
                return true;
            }
            return false;
        }
    }

    internal class SayCommand : ICommand
    {
        public string Name => "say";
        public IEnumerable<string> Aliases => new List<string> { "\"".Normalize(), "'".Normalize() };
        public bool Execute(Character character, List<string> parameters)
        {
            // If no message and it's a player, tell them to say something
            if (parameters.Count < 2 && character is Player player)
            {
                player.WriteLine("Say what?");
                return true;
            }
            Comm.RoomSay(character.GetRoom(), parameters[1], character);
            return true;
        }
    }

    internal class TellCommand : ICommand
    {
        public string Name => "tell";
        public IEnumerable<string> Aliases => new List<string> { "msg", "whisper" };
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            if (parameters.Count < 3)
            {
                player.WriteLine("Tell whom what?");
                return false;
            }
            string targetName = parameters[1];
            string message = string.Join(' ', parameters.Skip(2));
            Player? targetPlayer = GameState.Instance.GetPlayerByName(targetName);
            
            if (targetPlayer == null)
            {
                Comm.SendToIfPlayer(character, $"Player '{targetName}' not found.");

                return false;
            }

            // Probably should check if target is online

            targetPlayer.WriteLine($"{Messaging.CreateTellMessage(player.DisplayName(),message)}");
            player.WriteLine($"You tell {targetPlayer.DisplayName()}: {message}");
            return true;
        }
    }

    internal class TimeCommand : ICommand
    {
        public string Name => "time";
        public IEnumerable<string> Aliases => new List<string> { };
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.WriteLine($"The time is {GameState.Instance.GameDate.ToShortTimeString()}");
                return true;
            }
            return false;
        }
    }

    internal class StatusCommand : ICommand
    {
        public string Name => "status";
        public IEnumerable<string> Aliases => [];
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                int onlineCount = 0;
                long memory = GC.GetTotalMemory(true);
                player.WriteLine($"The Server started at {GameState.Instance.ServerStartTime}.");
                player.WriteLine($"There are currently {System.Diagnostics.Process.GetCurrentProcess().Threads} running.");
                player.WriteLine($"The system is currently using {memory}.");
                foreach (Player playerc in GameState.Instance.GetPlayersOnline())
                {
                    string name = playerc.DisplayName();
                    onlineCount++;
                    return true;
                }
                player.WriteLine($"There is currently {onlineCount} players online.");
                player.WriteLine($"It is currently {GameState.Instance.GameDate}.");

                return true;
            }

            return false;
        }
    }

    internal class HelpCommand : ICommand
    {
        public string Name => "help";
        public IEnumerable<string> Aliases => [];
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            // if no help topic given
            if (parameters.Count < 2)
            {
                foreach (HelpEntry he in GameState.Instance.HelpCatalog.Values)
                {
                    player.WriteLine($"{he.Name}");
                }
            }
            else
            {
                foreach (HelpEntry he in GameState.Instance.HelpCatalog.Values)
                {
                    if (he.Name.Equals(parameters[1], StringComparison.CurrentCultureIgnoreCase))
                    {
                        player.WriteLine($"{he.Name}");
                        player.WriteLine($"{he.Content}");

                    }
                }
            }
            return true;
        }

    }

    internal class CheckWeatherCommand : ICommand
    {
        public string Name => "weather";
        public IEnumerable<string> Aliases => new List<string> { };
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                // make a weather property for areas later
                Area area = player.GetArea();
                player.WriteLine($"The current weather is {area.Weather}");
                return true;
            }
            return false;

        }
    }

    internal class WeatherSetCommand : ICommand
    {
        public string Name => "setweather";
        public IEnumerable<string> Aliases => [];
        public bool Execute(Character character, List<string> parameters)
        {

            if (character is not Player player)
                return false;

            if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
            {
                player.WriteLine("You do not have permission to use this command.");
                return false;
            }

            if (parameters.Count < 2)
            {
                player.WriteLine("Set weather to what?");
                return true;
            }
            else
            {
                Area area = player.GetArea();
                area.Weather = parameters[1];
                player.WriteLine($"You set the weather to {area.Weather}");
                return true;
            }


        }
    }
}

