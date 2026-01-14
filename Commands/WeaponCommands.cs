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

        public IEnumerable<string> Aliases => Array.Empty<string>();

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

            Weapon w = new Weapon();
            w.Name = parameters[2];
            w.Description = parameters[3];
            w.Damage = Convert.ToDouble(parameters[4]);
            w.Durability = Convert.ToInt32(parameters[5]);
            //w.



            if (GameState.Instance.ItemsCatalog.ContainsKey(w.Name))
            {
                player.WriteLine($"There is already an object named {w.Name}");
            }
            else
            {
                GameState.Instance.ItemsCatalog.Add(w.Name, w);
            }
        }
        #endregion
    }
}
