using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGFramework.Enums;
using RPGFramework.Geography;

namespace RPGFramework.Command
{
    public class NavigationCommand
    {
        /// <summary>
        /// We'll use Character instead of Player so that we can use the same commands for NPCs.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="command"></param>
        /// <returns>Returns true if a command was matched.</returns>
        public static bool ProcessCommand(Character character, List<string> parameters)
        {
            switch (parameters[0].ToLower())
            {
                case "n":
                case "north":
                    Navigation.Move(character, Direction.North);
                    return true;
                case "e":
                case "east":
                    Navigation.Move(character, Direction.East);
                    return true;
                case "s":
                case "south":
                    Navigation.Move(character, Direction.South);
                    return true;
                case "w":
                case "west":
                    Navigation.Move(character, Direction.West);
                    return true;
                case "u":
                case "up":
                    Navigation.Move(character, Direction.Up);
                    return true;
                case "d":
                case "down":
                    Navigation.Move(character, Direction.Down);
                    return true;
            }

            return false;
        }

    }
}
