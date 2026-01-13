using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Commands
{
    internal class ItemCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return new List<ICommand>
            {
                new ItemBuilderCommand(),
                // Add more builder commands here as needed
            };
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
            i.Description = parameters[3];

            if (GameState.Instance.ItemsCatalog.ContainsKey(i.Name))
            {
                player.WriteLine($"There is already an object named {i.Name}");
            }
            else
            {
                GameState.Instance.ItemsCatalog.Add(i.Name, i);
            }
        }
        #endregion ---


        #region --- Item Code
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

                Item w = new Item();
                w.Name = parameters[2];
                w.Description = parameters[3];
                //w.Damage = parameters[4];

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
}
