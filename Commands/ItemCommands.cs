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

    #region ItemBuilderCommand Class
    /// <summary>
    /// /room command for building and editing rooms.
    /// </summary>
    internal class ItemBuilderCommand : ICommand
    {
        public string Name => "/item";

        public IEnumerable<string> Aliases => [];
        public string Help => "Create, modify and delete items\n"
            + "/item list - show all items\n"
            + "/item create <name> '<description>' <durability> <value>\n"
            + "/item create <name> '<description>' <durability> <value> all the rest (TODO)\n"
            + "/item set <name> <property> <value>\n";


        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            if (parameters.Count < 2)
                return ShowHelp(player);

            // Decide what to do based on the second parameter
            switch (parameters[1].ToLower())
            {
                case "create":
                    return ItemCreate(player, parameters);
                case "set":
                    return ItemSet(player, parameters);
                case "list":
                    return ListItems(player);
                default:
                    return ShowHelp(player);
            }
        }

        #region ItemCreate Method
        private bool ItemCreate(Player player, List<string> parameters)
        {
            Item newItem;

            if (parameters.Count < 6)
                return ShowHelp(player);


            if (!ParseInt(player, parameters[4], "Durability", out int durability))
                return false;

            if (!ParseInt(player, parameters[5], "Value", out int value))
                return false;

            // Shorthand method for making item
            if (parameters.Count == 6)
            {
                newItem = new()
                {
                    Name = parameters[2],
                    Description = parameters[3],
                    Durability = durability,
                    Value = value,
                };
            }
            else
            {
                // Every parse needs to be checked (see ParseInt example above).
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
                    Value = Convert.ToInt32(parameters[12]),
                    Weight = Convert.ToDouble(parameters[13]),
                    UseSpeed = Convert.ToDouble(parameters[14])
                };
            }

            if (GameState.Instance.ItemCatalog.ContainsKey(newItem.Name))
            {
                player.WriteLine($"There is already an object named {newItem.Name}");
                return false;
            }

            // If we get here we're good
            GameState.Instance.ItemCatalog.Add(newItem.Name, newItem);
            player.WriteLine($"Item ({newItem.Name}) added successfully.");

            return true;

        }
        #endregion ---

        #region ListItems Method
        private static bool ListItems(Player player)
        {
            player.WriteLine("All Items In Catalog:");
            foreach (Item i in GameState.Instance.ItemCatalog.Items.Values.OrderBy(o => o.Name))
            {
                // Could should more information and probably in a table, but this is a start.
                player.WriteLine($"{i.Name}: {i.Description}");
            }
            return true;
        }
        #endregion

        #region ItemSet Method
        private bool ItemSet(Player player, List<string> parameters)
        {
            if (parameters.Count < 5)
                return ShowHelp(player);

            string targetName = parameters[2];
            string property = parameters[3].ToLower();
            string valueString = parameters[4];

            // Find object
            if (!GameState.Instance.ItemCatalog.TryGetValue(targetName, out Item? target) || target == null)
            {
                player.WriteLine($"I couldn't find an item called {targetName}");
                return false;
            }
            
            switch (property)
            {
                case "description": target.Description = valueString; break;
                case "displaytext": target.DisplayText = valueString; break;
                case "durability":
                    if (!ParseInt(player, valueString, "Durability", out int durability))
                        return false;
                    target.Durability = durability; break;
                case "isdroppable":
                    if (!ParseBool(player, valueString, "IsDroppable", out bool isDroppable))
                        return false;
                    target.IsDroppable = isDroppable; break;
                case "isgettable":
                    if (!ParseBool(player, valueString, "IsGettable", out bool isGettable))
                        return false;
                    target.IsGettable = isGettable; break;
                case "isperishable":
                    if (!ParseBool(player, valueString, "IsPerishable", out bool isPerishable))
                        return false;
                    target.IsPerishable = isPerishable; break;
                case "level":
                    if (!ParseInt(player, valueString, "Level", out int level))
                        return false;
                    target.Level = level; break;
                // NOTE: This could break things that rely on the name, so don't do lightly.
                case "name":
                    GameState.Instance.ItemCatalog.Remove(valueString);
                    target.Name = valueString;
                    GameState.Instance.ItemCatalog[valueString] = target;
                    break;
                case "usespeed":
                    if (!ParseDouble(player, valueString, "UseSpeed", out double useSpeed))
                        return false;
                    target.UseSpeed = useSpeed; break;
                case "value":
                    if (!ParseInt(player, valueString, "Value", out int iValue))
                        return false;
                    target.Value = iValue; break;
                case "weight":
                    if (!ParseDouble(player, valueString, "Weight", out double weight))
                        return false;
                    target.Weight = weight; break;
                default:
                    player.WriteLine("I couldn't find that property!");
                    return false;
            }

            player.WriteLine($"{targetName}: {property} set to {valueString}");
            return true;
        }
        #endregion

        #region Parsing Helper Methods
        /// <summary>
        /// This is a little helper method so we don't have to have so much duplicated code.
        /// </summary>
        /// <returns>True/false if it was able to parse.</returns>
        private bool ParseInt(Player player, string toParse, string description, out int value)
        {
            if (!int.TryParse(toParse, out value))
            {
                player.WriteLine($"{description} has to be an integer!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// This is a little helper method so we don't have to have so much duplicated code.
        /// </summary>
        /// <returns>True/false if it was able to parse.</returns>
        private bool ParseDouble(Player player, string toParse, string description, out double value)
        {
            if (!double.TryParse(toParse, out value))
            {
                player.WriteLine($"{description} has to be a number!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// This is a little helper method so we don't have to have so much duplicated code.
        /// </summary>
        /// <returns>True/false if it was able to parse.</returns>
        private bool ParseBool(Player player, string toParse, string description, out bool value)
        {
            if (!bool.TryParse(toParse, out value))
            {
                player.WriteLine($"{description} has to be true/false!");
                return false;
            }
            return true;
        }
        #endregion

        private bool ShowHelp(Player player)
        {
            player.WriteLine(Help);
            return false;
        }
    }
    #endregion

}
