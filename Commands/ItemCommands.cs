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

        private bool ItemCreate(Player player, List<string> parameters)
        {
            Item newItem;

            if (parameters.Count < 5)
            {
                WriteUsage(player);
                return false;
            }

            // Shorthand method for making item
            if (parameters.Count == 6)
            {
                newItem = new()
                {
                    Name = parameters[2],
                    Description = parameters[3],
                    Durability = int.Parse(parameters[5]),
                    Value = int.Parse(parameters[6]),
                };


            }
            else
            {
                // Every parse needs to be checked
                if (parameters.Count < 15)
                {
                    player.WriteLine("The full create requires 16 parameters");
                    return false;
                }

                newItem = new()
                {
                    // CODE REVIEW: Brayden, Tyler
                    // The boolean and double conversions here could throw exceptions if the input is invalid.
                    // I'm not sure what element 9 is supposed to be since Name is already at 2.
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
                    UseSpeed = Convert.ToDouble(parameters[14])
                };

                // These should all use TryParse
                //int = Convert.ToInt32(parameters[+]);
                //bool = Convert.ToBoolean(parameters[+]);
                //double =  Convert.ToDouble(parameters[+]);

                if (GameState.Instance.ItemCatalog.ContainsKey(newItem.Name))
                {
                    player.WriteLine($"There is already an object named {newItem.Name}");
                    return false;
                }

            }

            // If we get here we're good
            GameState.Instance.ItemCatalog.Add(newItem.Name, newItem);
            player.WriteLine($"Item ({newItem.Name} added successfully.");

            return true;

        }
    #endregion ---
    #region --- Weapon Code ---
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

            private bool WeaponCreate(Player player, List<string> parameters)
            {
                Weapon newWeapon;

                if (parameters.Count < 5)
                {
                    WriteUsage(player);
                    return false;
                }

                // Shorthand method for making item
                if (parameters.Count == 6)
                {
                    newWeapon = new()
                    {
                        Name = parameters[2],
                        Description = parameters[3],
                        Durability = int.Parse(parameters[5]),
                        Value = int.Parse(parameters[6]),
                    };


                }
                else
                {
                    // Every parse needs to be checked
                    if (parameters.Count < 15)
                    {
                        player.WriteLine("The full create requires 16 parameters");
                        return false;
                    }

                    newWeapon = new()
                    {
                        // CODE REVIEW: Brayden, Tyler
                        // The boolean and double conversions here could throw exceptions if the input is invalid.
                        // I'm not sure what element 9 is supposed to be since Name is already at 2.
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
                        UseSpeed = Convert.ToDouble(parameters[14])
                    };

                    // These should all use TryParse
                    //int = Convert.ToInt32(parameters[+]);
                    //bool = Convert.ToBoolean(parameters[+]);
                    //double =  Convert.ToDouble(parameters[+]);

                    if (GameState.Instance.WeaponCatalog.ContainsKey(newWeapon.Name))
                    {
                        player.WriteLine($"There is already an object named {newWeapon.Name}");
                        return false;
                    }

                }

                // If we get here we're good
                GameState.Instance.WeaponCatalog.Add(newWeapon.Name, newWeapon);
                player.WriteLine($"Item ({newWeapon.Name} added successfully.");

                return true;

            }
        }
        #endregion
        #region --- Food code ---
        /// <summary>
        /// /room command for building and editing rooms.
        /// </summary>
        internal class FoodBuilderCommand : ICommand
        {
            public string Name => "/food";

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

            private bool FoodCreate(Player player, List<string> parameters)
            {
                Food newFood;

                if (parameters.Count < 4)
                {
                    WriteUsage(player);
                    return false;
                }

                // Shorthand method for making item
                if (parameters.Count == 5)
                {
                    newFood = new()
                    {
                        Name = parameters[2],
                        Description = parameters[3],
                        Value = int.Parse(parameters[6]),
                    };


                }
                else
                {
                    // Every parse needs to be checked
                    if (parameters.Count < 15)
                    {
                        player.WriteLine("The full create requires 16 parameters");
                        return false;
                    }

                    newFood = new()
                    {
                        // CODE REVIEW: Brayden, Tyler
                        // The boolean and double conversions here could throw exceptions if the input is invalid.
                        // I'm not sure what element 9 is supposed to be since Name is already at 2.
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
                        UseSpeed = Convert.ToDouble(parameters[14])
                    };

                    // These should all use TryParse
                    //int = Convert.ToInt32(parameters[+]);
                    //bool = Convert.ToBoolean(parameters[+]);
                    //double =  Convert.ToDouble(parameters[+]);

                    if (GameState.Instance.FoodCatalog.ContainsKey(newFood.Name))
                    {
                        player.WriteLine($"There is already an object named {newFood.Name}");
                        return false;
                    }

                }

                // If we get here we're good
                GameState.Instance.FoodCatalog.Add(newFood.Name, newFood);
                player.WriteLine($"Item ({newFood.Name} added successfully.");

                return true;

            }
        }
        #endregion
    }

}
