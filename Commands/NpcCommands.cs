using System;
using RPGFramework;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Commands
{
    internal class NpcCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return
            [
                new MobBuilderCommand(),
                // Add other Npc commands here as they are implemented
            ];
        }
    }

    #region MobBuilderCommand Class
    internal class MobBuilderCommand : ICommand
    {
        public string Name => "/mob";
        public IEnumerable<string> Aliases => [];
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
            {
                return false;
            }

            if (parameters.Count < 2)
            {
                ShowHelp(player);
                return false;
            }

            switch (parameters[1].ToLower())
            {
                case "create":
                    return MobCreate(player, parameters);
                case "delete":
                    MobDelete(player, parameters);
                    break;
                case "kill":
                    MobKill(player, parameters);
                case "list":
                    //ShowCommand(player, parameters);
                    break;
                case "set":
                    return MobSet(player, parameters);
                default:
                    ShowHelp(player);
                    break;
            }
            return false;
        }

        #region MobCreate Method
        ////  what it will look like ---->  /Mob create kyler 'Long legged short hair'
        private static bool MobCreate(Player player, List<string> parameters)
        {
            if (parameters.Count < 4)
            {
                player.WriteLine("Provide at least a name and description.");
                return false;
            }

            if (GameState.Instance.MobCatalog.ContainsKey(parameters[2]))
            {
                player.WriteLine($"The mob {parameters[2]} already exists.");
                return false;
            }

            Mob m = new()
            {
                Name = parameters[2],
                Description = parameters[3]
            };

            GameState.Instance.MobCatalog.Add(m.Name, m);
            player.WriteLine($"{m.Name} added to the mob catalog.");
            return true;
        }
        #endregion

        #region MobDelete Method
        private static bool MobDelete(Player player, List<string> parameters)
        {
            if (parameters.Count < 4)
            {
                player.WriteLine("Provide at least a name and description.");
                return false;
            }

            if (!GameState.Instance.MobCatalog.ContainsKey(parameters[2]))
            {
                player.WriteLine($"The mob {parameters[2]} does not exist.");
                return false;
            }
            
            Mob m = GameState.Instance.MobCatalog[parameters[2]];
            GameState.Instance.MobCatalog.Remove(m.Name);
            player.WriteLine($"{m.Name} was removed the mob catalog.");
            return true;
        }
        #endregion

        #region MobSet Method
        private static bool MobSet(Player player, List<string> parameters)
        {
            if (parameters.Count < 5)
            {
                player.WriteLine("Usage: /mob set <mob name> <property name> <value>");
                return false;
            }

            if (!GameState.Instance.MobCatalog.TryGetValue(parameters[2], out Mob? m) || m == null)
            {
                player.WriteLine($"The mob {parameters[2]} does not exist.");
                return false;
            }

            string propName = parameters[3].ToLower();
            string propValue = parameters[4];

            switch (propName)
            {
                case "name":
                    if (GameState.Instance.MobCatalog.ContainsKey(propValue))
                    {
                        player.WriteLine($"A mob with the name '{propValue}' already exists.");
                        return false;
                    }
                    m.Name = propValue;
                    GameState.Instance.MobCatalog.Remove(parameters[2]);
                    GameState.Instance.MobCatalog.Add(m.Name, m);
                    break;
                case "description":
                case "desc":
                    m.Description = propValue;
                    return true;
                case "health":
                case "maxhealth":
                    if (!int.TryParse(propValue, out int health))
                    {
                        player.WriteLine("Health must be a valid integer.");
                        return false;
                    }

                    m.SetMaxHealth(health);
                    break;
                case "gold":
                    if (!int.TryParse(propValue, out int gold))
                    {
                        player.WriteLine("Gold must be a valid integer.");
                        return false;
                    }
                    m.Gold = gold;
                    break;
                default:
                    player.WriteLine($"Unknown property '{propName}'.");
                    return false;
            }

            // Set properties of the mob here based on additional parameters
            player.WriteLine($"{m.Name} {propName} was updated to {propValue} in the mob catalog.");
            return true;
        }
        #endregion
        private void MobKill(Player player, List<string> parameters)
        {
            if (parameters.Count < 4)
            {
                player.WriteLine("Provide at least a name and description.");
                return;
            }

            if (!GameState.Instance.MobCatalog.ContainsKey(parameters[2]))
            {
                player.WriteLine($"The mob {parameters[2]} is not alive or does not exist");
                return;
            }

            player.GetRoom

            Mob m = GameState.Instance.MobCatalog[parameters[2]];

            player.WriteLine($"{m.Name} was removed the mob catalog.");
        }
        // private  void Roomset(Player player, List<string> parameters)
        //{
        //mob.RoomID = player.GetRoom();
        //}
        private  void WriteUsage(Player player)
        {
            player.WriteLine("Usage: ");
            player.WriteLine("/mob description '<set room desc to this>'");
            player.WriteLine("/mob name '<set room name to this>'");
            player.WriteLine("/mob create '<name>' '<description>' <exit direction> '<exit description>'");
        }
        #endregion
    }
    #endregion

}
