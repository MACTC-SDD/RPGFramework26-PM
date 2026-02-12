using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using RPGFramework;
using RPGFramework.Geography;
using Spectre.Console;

namespace RPGFramework.Commands
{
    internal class NpcCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return
            [
                new MobBuilderCommand(),
                new NpcBuilderCommand(),
                new ShopKeeperAdminCommand(),
                new ShopKeeperCommand()
                // Add other Npc commands here as they are implemented
            ];
        }
    }
    #region Mob Commands
    internal class MobBuilderCommand : ICommand
    {
        public string Name => "/mob";
        public IEnumerable<string> Aliases => [];
        public string Help => "[bold underline]Usage:[/]\n"
           + "/mob list \n"
           + "/mob create 'Name' 'MobClassifier' 'Description' \n"
           + "/mob delete 'Name' \n"
           + "/mob set 'Name' 'MobProporty' \n"
           + "/mob load 'Name' \n"
           + "/mob specify 'Name'";
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
                case "load":
                    MobLoad(player, parameters);
                    break;
                case "list":
                    MobList(player, parameters);
                    break;
                case "set":
                    return MobSet(player, parameters);
                case "specify":
                    return MobSpecify(player, parameters);
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
                player.WriteLine("Provide at least a name, The mob classifier and description.");
                return false;
            }

            if (GameState.Instance.MobCatalog.ContainsKey(parameters[2]))
            {
                player.WriteLine($"The mob {parameters[2]} already exists.");
                return false;
            }
            /*
                        Weapon? newWeapon;
                        if (
                            !GameState.Instance.WeaponCatalog.TryGetValue(parameters[2], out Weapon? w) ||
                            w == null ||
                            (newWeapon = Utility.Clone(w)) == null
                        ) {
                            player.WriteLine(
                                "No such weapon in weapon catalog.\n" +
                                "You may add it to the weapon catalog by typing:\n"
                            // "/item create 'Name' 'type' 'damage'"  will work on when they get it finished 
                            );
                            return false;
                        }
            */
            Mob m = new()
            {
                Name = parameters[2],
                Description = parameters[3],
            };


            GameState.Instance.MobCatalog.Add(m.Name, m);
            player.WriteLine($"{m.Name} added to the mob catalog.");
            return true;
        }
        #endregion
        #region MobDelete Method
        private static bool MobDelete(Player player, List<string> parameters)
        {
            if (parameters.Count < 3)
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
        #region MobLoad Method
        private static bool MobLoad(Player player, List<string> parameters)
        {
            if (parameters.Count < 3)

            {
                player.WriteLine("Usage: /mob load <name>");
                return false;
            }

            if (!GameState.Instance.MobCatalog.TryGetValue(parameters[02], out Mob? m))
            {
                player.WriteLine("The mob you are trying to summon is not avalible in the current mob catolog");
                return false;

            }
            Mob? clone = Utility.Clone<Mob>(m);
            if (clone == null)
            {
                return false;
            }
            player.GetRoom().Mobs.Add(clone);
            player.WriteLine($"mob {clone.Name} added to room");
            return true;
        }
        #endregion
        #region MobList Method
        private static bool MobList(Player player, List<string> parameters)
        {
            player.WriteLine("All the Mobs:");
            player.WriteLine("Name       Description"); //Put this into a table so it is organized for the player
            foreach (Mob mob in GameState.Instance.MobCatalog.Values.OrderBy(x => x.Name))
            {
                player.WriteLine($"{mob.Name} - {mob.Description}");
            }
            return true;
        }
        #endregion
        #region MobSpecify Method
        private static bool MobSpecify(Player player, List<string> parameters)
        {
            if (parameters.Count < 2)
            {
                player.WriteLine("Provide at least the name of the mob");
                return false;
            }

            if (!GameState.Instance.MobCatalog.ContainsKey(parameters[2]))
            {
                player.WriteLine($"The mob {parameters[2]} is not alive or does not exist within the current catolog");
                return false;
            }

            //player.GetRoom

            Mob m = GameState.Instance.MobCatalog[parameters[2]];

            player.WriteLine($"Max Health: {m.MaxHealth}");
            player.WriteLine($"Level: {m.Level}");
            player.WriteLine($"Class: {m.Class?.Name ?? "None"}");
            // player.WriteLine($"Element: {m.Element}");
            player.WriteLine($"XP: {m.XP}");
            player.WriteLine($"Primary Weapon: {m.PrimaryWeapon.Name}");
            return true;
        }
        #endregion
        #region ShowHelp Method
        private void ShowHelp(Player player)
        {
            var table = new Table();

            table.AddColumn(new TableColumn("[mediumpurple2]Mob Help[/]"));

            table.AddRow(Help);
            player.Write(table);
        }
        #endregion
    }

    #endregion

    #region npc Commands

    internal class NpcBuilderCommand : ICommand
    {
        public string Name => "/npc";
        public IEnumerable<string> Aliases => [];

        public string Help => "[bold underline]Usage: [/]\n"
            + "/npc list \n"
            + "/npc create 'Name' 'NpcClassifier' 'Description' \n"
            + "/npc delete 'Name'";
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
                    return NpcCreate(player, parameters);
                case "list":
                    return NpcList(player, parameters);
                case "delete":
                    return NpcDelete(player, parameters);
                default:
                    ShowHelp(player);

                    break;
            }
            return false;
        }
        #region ShowHelp Method
        private void ShowHelp(Player player)
        {
            var table = new Table();

            table.AddColumn(new TableColumn("[mediumpurple2]NPC List[/]"));

            table.AddRow(Help);
            player.Write(table);
        }
        #endregion
        #region NpcCreate Method
        private static bool NpcCreate(Player player, List<string> parameters)
        {
            if (parameters.Count < 5)
            {
                player.WriteLine("Provide at least a name, The npc classifier and description.");
                player.WriteLine("Npc Clasifier Examples: Villager, Farm Animal, Shop Keeper, Knight, Wizard");
                return false;
            }

            if (GameState.Instance.NPCCatalog.ContainsKey(parameters[2]))
            {
                player.WriteLine($"The Npc {parameters[2]} already exists.");
                return false;
            }

            NonPlayer n = new()
            {
                Name = parameters[2],
                NpcClasification = parameters[3],
                Description = parameters[4]
            };

            GameState.Instance.NPCCatalog.Add(n.Name, n);
            player.WriteLine($"{n.Name} was added to the Npc catalog.");
            return true;
        }
        #endregion
        #region NpcDelete Method
        private static bool NpcDelete(Player player, List<string> parameters)
        {
            if (parameters.Count < 5)
            {
                player.WriteLine("Provide at least a name");
                return false;
            }

            if (GameState.Instance.NPCCatalog.ContainsKey(parameters[2]))
            {
                player.WriteLine($"The Npc {parameters[2]} already exists.");
                return false;
            }

            NonPlayer n = new()
            {
                Name = parameters[2],
                NpcClasification = parameters[3],
                Description = parameters[4]
            };

            GameState.Instance.NPCCatalog.Remove(n.Name);
            player.WriteLine($"{n.Name} was Removed to the Npc catalog.");
            return true;
        }
        #endregion
        #region NpcList Method
        private static bool NpcList(Player player, List<string> parameters)
        {
            player.WriteLine("All the Mobs:");
            player.WriteLine("Name       Classification       Description"); //Put this into a table so it is organized for the player
            foreach (NonPlayer npc in GameState.Instance.NPCCatalog.Values.OrderBy(x => x.Name))
            {
                player.WriteLine($"{npc.Name} - {npc.NpcClasification} - {npc.Description}");
            }
            return true;
        }
        #endregion
    }

    #endregion

    #region Shopkeeper Command
    internal class ShopKeeperCommand : ICommand
    {
        public string Name => "shopkeeper";
        public IEnumerable<string> Aliases => [];
        public string Help => "Interact with a shopkeeper\nUsage:shopkeeper interact\nshopkeeper buy";

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
                case "interact":
                    return ShopkeeperInteract(player, parameters);
                default:
                    return ShowHelp(player);
            }
        }

        public bool ShowHelp(Player player)
        {
            player.WriteLine(Help);
            return false;
        }
        private static bool ShopkeeperInteract(Player player, List<string> parameters)
        {
            if (player.GetRoom().ShopKeeper.Count > 0)
            {
                player.WriteLine("There is no shopkeeper in this room.");
                return false;
            }

            ShopKeeper s = player.GetRoom().ShopKeeper[0];

            player.WriteLine(s.Greeting);

            // List items they have to sell
            player.WriteLine("Items For Sale:");
            foreach (Item i in s.BackPack.Items)
            {
                player.WriteLine($"{i.Name} : {i.Level} : {(int)(i.Value * s.UpCharge)}  ");
            }

            return true;
        }

    }
    #endregion

    #region ShopkeeperAdmin Command
    internal class ShopKeeperAdminCommand : ICommand
    {
        public string Name => "/shopkeeper";
        public IEnumerable<string> Aliases => [ "/sk" ];
        public string Help => "Manage a shopkeeper\nUsage:/shopkeeper load\n/shopkeeper create";

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
                case "load":
                    return ShopKeeperLoad(player, parameters);
                case "create":
                    return ShopKeeperCreate(player, parameters);
                default:
                    return ShowHelp(player);
            }
        }

        public bool ShowHelp(Player player)
        {
            player.WriteLine(Help);
            return false;
        }

        private bool ShopKeeperCreate(Player player, List<string> parameters)
        {
            if (parameters.Count < 4)
            {
                return ShowHelp(player);
            }

            if (GameState.Instance.ShopKeeperCatalog.TryGetValue(parameters[2], out _))
            {
                player.WriteLine("There is already a shopkeeper with this name.");
                return false;
            }

            ShopKeeper s = new()
            {
                Name = parameters[2],
                Description = parameters[3],
            };

            GameState.Instance.ShopKeeperCatalog.Add(s.Name, s);
            player.WriteLine($"Shopkeeper {s.Name} added to the catalog.");
            return true;
        }
        
        public bool ShopKeeperDelete(Player player, List<string> parameters)
        {
            if (parameters.Count < 3)
                return ShowHelp(player);

            string name = parameters[2];
            if (!GameState.Instance.ShopKeeperCatalog.TryGetValue(name, out ShopKeeper? s) || s == null)
            {
                player.WriteLine("I couldn't find that shopkeeper.");
            }

            GameState.Instance.ShopKeeperCatalog.Remove(name);

            foreach (Area a in GameState.Instance.Areas.Values)
            {
                foreach (Room r in  a.Rooms.Values)
                {
                    r.ShopKeeper.Remove(s!);
                }
            }

            player.WriteLine("Shopkeeper deleted from catalog.");
            return true;
        }

        private static bool ShopKeeperLoad(Player player, List<string> parameters)
        {
            if (parameters.Count < 3)
            {
                player.WriteLine("You must specify a name to load.");
                return false;
            }
            string name = parameters[2];

            if (!GameState.Instance.ShopKeeperCatalog.TryGetValue(name, out ShopKeeper? s) || s == null)
            {
                player.WriteLine("I couldn't find a shopkeeper called that.");
                return false;
            }

            ShopKeeper clone = Utility.Clone(s);

            player.GetRoom().ShopKeeper.Add(clone!);
            clone.LocationId = player.LocationId;
            clone.AreaId = player.AreaId;
            player.WriteLine($"Shopkeeper added to room.");
            return true;
        }
    }
    #endregion
}
