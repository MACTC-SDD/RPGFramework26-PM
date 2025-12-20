using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGFramework.Enums;
using RPGFramework.Geography;

namespace RPGFramework.Commands
{
    public class NavigationCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return new List<ICommand>
            {
                new MoveCommand(),
                // Add more navigation commands here as needed
            };
        }
    }

    /// <summary>
    /// Move a character (player or NPC) in a direction if possible.
    /// </summary>
    public class MoveCommand : ICommand
    {
        public string Name => "move";
        public IEnumerable<string> Aliases => new List<string> 
        {
            "n", "north",
            "e", "east",
            "s", "south",
            "w", "west",
            "u", "up",
            "d", "down"
        };

        public bool Execute(Character character, List<string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                if (character is Player player)
                {
                    player.WriteLine("Usage: move <direction>");
                }
                return false;
            }

            string directionStr = parameters[1].ToLower();
            Direction? direction = directionStr switch
            {
                "n" or "north" => Direction.North,
                "e" or "east" => Direction.East,
                "s" or "south" => Direction.South,
                "w" or "west" => Direction.West,
                "u" or "up" => Direction.Up,
                "d" or "down" => Direction.Down,
                _ => null
            };
            if (direction.HasValue)
            {
                Navigation.Move(character, direction.Value);
                return true;
            }
            else if (character is Player player)
            {
                player.WriteLine("Invalid direction. Valid directions are: north, east, south, west, up, down.");
            }
            return false;
        }
    }
}
