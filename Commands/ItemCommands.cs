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

        public IEnumerable<string> Aliases => Array.Empty<string>();
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

            Item i = new Item();
            i.Name = parameters[2];
            i.Id = Convert.ToInt32(parameters[3]);
            i.Description = parameters[4];
            i.DisplayText = parameters[5];
            i.IsDroppable = Convert.ToBoolean(parameters[6]);
            i.IsDroppable = Convert.ToBoolean(parameters[7]);
            i.Level = Convert.ToInt32(parameters[8]);
            i.Name = parameters[9];
            i.Tags = parameters[10].Split(",").ToList();
            i.UsesRemaining = Convert.ToInt32(parameters[11]);
            i.Value = Convert.ToDouble(parameters[12]);
            i.Weight = Convert.ToDouble(parameters[13]);
            i.SpawnChance = Convert.ToDouble(parameters[14]);
            i.UseSpeed = Convert.ToDouble(parameters[15]);


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
