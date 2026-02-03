using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Commands
{

    #region --- Weapon Code
    /// <summary>
    /// /room command for building and editing rooms.
    /// </summary>
    internal class WeaponBuilderCommand : ICommand
    {
        public string Name => "/weapon";

        public IEnumerable<string> Aliases => [];
        public string Help => "";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
            {
                return false;
            }

            if (parameters.Count < 2)
            {
                WriteUsage(player);
                return false;
            }

            // Decide what to do based on the second parameter
            switch (parameters[1].ToLower())
            {
                case "create":
                    WeaponCreate(player, parameters);
                    break;
                case "set":
                    // We'll move setting name and description into this
                    //RoomSet(player, parameters);
                    break;
                default:
                    WriteUsage(player);
                    break;
            }

            return true;
        }
        private void WriteUsage(Player player)
        {
            player.WriteLine("help message");
        }

        private void WeaponCreate(Player player, List<string> parameters)
        {
            // Make sure not < 4
            // CODE REVIEW: Brayden, Tyler
            // Make sure that the int and double conversions here could throw exceptions if the input is invalid.
            Weapon w = new()
            {
                Name = parameters[2],
                Description = parameters[3],
                Damage = Convert.ToInt32(parameters[4]),
                Durability = Convert.ToInt32(parameters[5]),
                Range = Convert.ToInt32(parameters[6]),
                Speed = Convert.ToDouble(parameters[7]),
                Weight = Convert.ToDouble(parameters[8])
            };

            
            if (GameState.Instance.ItemCatalog.TryGetValue(w.Name, out _))
            {
                player.WriteLine($"There is already an object named {w.Name}");
            }
            else
            {
                GameState.Instance.ItemCatalog.Add(w.Name, w);
            }
        }
        #endregion
    }
}
