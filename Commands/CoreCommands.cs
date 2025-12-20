using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RPGFramework.Commands
{
    // A class for processing commands
    // We'll start simple and expand on this later
    public class CoreCommands
    {

        // Process a command from a player or character (NPC)
        // Although the other methods could be called directly, it's better to have a single
        // entry point for all commands. It's probably a good idea for us to make those
        // methods private for clarity.


        //private static bool ProcessCommand(Character character, List<string> parameters)
        //{
        //    switch (parameters[0].ToLower())
        //    {
        //        case "exit" when character is Player:
        //        case "quit":
        //            ((Player)character).Logout();
        //            return true;
        //        case "ip" when character is Player:
        //            Ip((Player)character, parameters);
        //            return true;
        //        case "look":
        //        case "l" when character is Player:
        //            Look((Player)character, parameters);
        //            return true;

        //        case "say":
        //        case "\"":
        //            Say(character, parameters);
        //            return true;
        //        case "time" when character is Player:
        //            ((Player)character).WriteLine($"The time is {GameState.Instance.GameDate.ToShortTimeString()}");
        //            break;
        //    }

        //    return false;
        //}

        ///// <summary>
        ///// Look at something (or room).
        ///// Where is the most appropriate place to put this?
        ///// </summary>
        ///// <param name="player"></param>
        ///// <param name="command"></param>
        //private static void Look(Player player, List<string> parameters)
        //{
        //    // For now, we'll ignore the command and just show the room description
        //    player.WriteLine($"{player.GetRoom().Description}");
        //    player.WriteLine("Exits:");
        //    foreach (var exit in player.GetRoom().GetExits())
        //    {
        //        player.WriteLine($"{exit.Description} to the {exit.ExitDirection}");
        //    }
        //}

        //private static void Ip(Player player, List<string> parameters)
        //{
        //    player.WriteLine($"Your IP address is {player.GetIPAddress()}");
        //}

        // Send message from player to all players in room.
        // TODO: Make this smarter so the speaker either doesn't hear their message
        // or sees it in the format "You say 'message'"
        //private static void Say(Character character, List<string> parameters) 
        //{
        //    // If no message and it's a player, tell them to say something
        //    if (parameters.Count < 2 && character is Player)
        //    {
        //        ((Player)character).WriteLine("Say what?");
        //        return;
        //    }

        //    Comm.RoomSay(character.GetRoom(), parameters[1], character);
        //}


        public static List<ICommand> GetAllCommands()
        {
            return new List<ICommand>
            {
                new IpCommand(),
                new LookCommand(),
                new QuitCommand(),
                new SayCommand(),
                new TimeCommand(),
                // Add other core commands here as they are implemented
            };
        }


    }

    public class IpCommand : ICommand
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

    public class LookCommand : ICommand
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

    public class QuitCommand : ICommand
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

    public class SayCommand : ICommand
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

    public class TimeCommand : ICommand
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


}
