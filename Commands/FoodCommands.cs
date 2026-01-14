using RPGFramework.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Commands
{

    internal class FoodBuilderCommand : ICommand
    {
        public string Name => "/Food";

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
                    FoodCreate(player, parameters);
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

        private void FoodCreate(Player player, List<string> parameters)
        {
            // Make sure not < 4

            Food f = new Food();
            f.Name = parameters[2];
            f.Description = parameters[3];
            f.HealAmount = Convert.ToInt32(parameters[4]);
            f.StackAmount = Convert.ToInt32(parameters[5]);
            f.StackMax = Convert.ToInt32(parameters[6]);
            



            if (GameState.Instance.ItemsCatalog.ContainsKey(f.Name))
            {
                player.WriteLine($"There is already an object named {f.Name}");
            }
            else
            {
                GameState.Instance.ItemsCatalog.Add(f.Name, f);
            }
        }

    }
}
