using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Commands
{
    internal class ArmorBuilderCommand : ICommand
    {
        public string Name => "/Armor";

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
                    ArmorCreate(player, parameters);
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

        private void ArmorCreate(Player player, List<string> parameters)
        {
            // Make sure not < 4

            Armor a = new Armor();
            a.Name = parameters[2];
            a.Description = parameters[3];
            a.protection = Convert.ToInt32(parameters[4]);
            a.Durability = Convert.ToInt32(parameters[5]);
            a.weight = Convert.ToInt32(parameters[6]);



            if (GameState.Instance.ItemsCatalog.ContainsKey(a.Name))
            {
                player.WriteLine($"There is already an object named {a.Name}");
            }
            else
            {
                GameState.Instance.ItemsCatalog.Add(a.Name, a);
            }
        }
    }
}
