using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Commands
{
    internal class ArmorCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return
            [
                new ArmorBuilderCommand(),
            ];
        }
    }

    internal class ArmorBuilderCommand : ICommand
    {
        public string Name => "/armor";

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

            Armor a = new()
            {
                Name = parameters[2],
                Description = parameters[3],
                Protection = Convert.ToInt32(parameters[4]),
                Durability = Convert.ToInt32(parameters[5]),
                Weight = Convert.ToInt32(parameters[6])
            };



            if (GameState.Instance.ItemCatalog.ContainsKey(a.Name))
            {
                player.WriteLine($"There is already an object named {a.Name}");
            }
            else
            {
                GameState.Instance.ItemCatalog.Add(a.Name, a);
            }
        }
    }
}
