using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Commands
{
    internal class ItemCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return
            [
                new ItemBuilderCommand(),
                new WeaponBuilderCommand(),
            
                // Add more builder commands here as needed
            ];
        }
    }

    #region --- Item Code ---
    /// <summary>
    /// /room command for building and editing rooms.
    /// </summary>
    internal class ItemBuilderCommand : ICommand
    {
        public string Name => "/item";

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
                    ItemCreate(player, parameters);
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

        private void ItemCreate(Player player, List<string> parameters)
        {
            // Make sure not < 4

            // CODE REVIEW: Brayden, Tyler
            // The boolean and double conversions here could throw exceptions if the input is invalid.
            // I'm not sure what element 9 is supposed to be since Name is already at 2.
            Item i = new()
            {
                Name = parameters[2],
                Id = Convert.ToInt32(parameters[3]),
                Description = parameters[4],
                DisplayText = parameters[5],
                IsDroppable = Convert.ToBoolean(parameters[6]),
                IsGettable = Convert.ToBoolean(parameters[7]),
                Level = Convert.ToInt32(parameters[8]),
                Tags = [.. parameters[10].Split(",")],
                Value = Convert.ToDouble(parameters[12]),
                Weight = Convert.ToDouble(parameters[13]),
                SpawnChance = Convert.ToDouble(parameters[14]),
                UseSpeed = Convert.ToDouble(parameters[15])
            };



            //int = Convert.ToInt32(parameters[+]);
            //bool = Convert.ToBoolean(parameters[+]);
            //double =  Convert.ToDouble(parameters[+]);


            if (GameState.Instance.ItemCatalog.ContainsKey(i.Name))
            {
                player.WriteLine($"There is already an object named {i.Name}");
            }
            else
            {
                GameState.Instance.ItemCatalog.Add(i.Name, i);
            }
        }
        #endregion ---

    }

}
