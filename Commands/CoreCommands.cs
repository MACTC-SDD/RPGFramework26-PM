
using RPGFramework.Core;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

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
                new TimeCommand(),
                new HelpCommand(),
                new XPCommand(),
                new LevelCommand(),
                new TrainCommand(),
                // Add other core commands here as they are implemented
            };
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
                player.WriteLine($"{player.GetRoom().Description}");
                player.WriteLine("Exits:");
                foreach (var exit in player.GetRoom().GetExits())
                {
                    player.WriteLine($"{exit.Description} to the {exit.ExitDirection}");
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

    internal class HelpCommand : ICommand
    {
        public string Name => "help";
        public IEnumerable<string> Aliases => new List<string> { };
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                // if no help topic given
                if (parameters.Count < 2)
                {
                    foreach (HelpEntry he in GameState.Instance.HelpEntries)
                    {
                        player.WriteLine($"{he.Name}");
                    }
                }
                else
                {
                    foreach (HelpEntry he in GameState.Instance.HelpEntries)
                    {
                        if (he.Name.ToLower() == parameters[1].ToLower())
                        {
                            player.WriteLine($"{he.Name}");
                            player.WriteLine($"{he.Content}");

                        }
                    }
                }
                return true;
            }

            return false;
        }
    }
    internal class XPCommand : ICommand
    {
        public string Name => "xp";
        public IEnumerable<string> Aliases => new List<string> { };
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.WriteLine($"You have {player.XP} XP. You need  {player.Levels[player.Level].RequiredXp - player.XP} XP");
                return true;
            }
            return false;
        }

    }
    internal class LevelCommand : ICommand
    {
        public string Name => "level";
        public IEnumerable<string> Aliases => new List<string> { };
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.WriteLine($"You are level {player.Level} you will gain an additional {player.Levels[player.Level].Health} health and you will have {player.Levels[player.Level].StatPoints} points upon level up.");
                return true;
            }
            return false;
        }

    }
    internal class TrainCommand : ICommand
    {
        public string Name => "train";
        public IEnumerable<string> Aliases => new List<string> { };
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
            {
                return false;
            }
            if (parameters.Count < 2)
            {
                player.WriteLine($"to train you must type train and whatever you are trying to train for example, train strength.");
                return true;
            }
            if (player.StatPoints < 1)
            {
                player.WriteLine($"you dont have enough stat points or attribute doesnt exist");
                return true;
            }
           switch(parameters[1].ToLower())
            {
                case "Strength":
                    player.Strength++;
                    player.StatPoints--;
                    player.WriteLine($"added 1 point to strength");
                    break;
                case "dexterity":
                    player.Dexterity++;
                    player.StatPoints--;
                    player.WriteLine($"added 1 point to dexterity");
                break;
                case "constitution":
                    player.Constitution++;
                    player.StatPoints--;
                    player.WriteLine($"added 1 point to constitution");
                break;
                case "intelligence":
                    player.Intelligence++;
                    player.StatPoints--;
                    player.WriteLine($"added 1 point to intelligence");
                break;
                case "wisdom":
                    player.Wisdom++;
                    player.StatPoints--;
                    player.WriteLine($"added 1 point to wisdom");
                break;
                case "charisma":
                    player.Charisma++;
                    player.StatPoints--;
                    player.WriteLine($"added 1 point to charisma");
                break;
                default:
                    player.WriteLine("unkown attribute");
                break;
            }
            return false;
        }
    }
}
