
using RPGFramework.Core;
using RPGFramework.Display;
using RPGFramework.Geography;
using Spectre.Console;
using RPGFramework.Enums;
using System.Collections.Immutable;
using RPGFramework.Items;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace RPGFramework.Commands
{
    /// <summary>
    /// Provides access to the set of built-in core command implementations.
    /// </summary>
    /// <remarks>The <c>CoreCommands</c> class exposes static methods for retrieving all available core
    /// commands. These commands represent fundamental operations supported by the system </remarks>
    internal class CoreCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return
            [
                new AFKCommand(),
                new ExamineCommand(),
                new IpCommand(),
                new LookCommand(),
                new QuitCommand(),
                new SayCommand(),
                new TellCommand(),
                new TimeCommand(),
                new StatusCommand(),
                new HelpCommand(),
                new CheckWeatherCommand(),
                new WeatherSetCommand(),
                new GoldCommand(),
                new HealCommand(),
                new DamageCommand(),
                new PurgeRoomCommand(),
                new XPCommand(),
                new LevelCommand(),
                new TrainCommand(),                
                new EquipmentCommand(),
                new InvCommand(),
                new GetCommand(),
                new DropCommand(),
                new GiveCommand(),
                // Add other core commands here as they are implemented
            ];
        }


    }


    internal class GiveCommand : ICommand
    {
        public string Name => "give";
        public IEnumerable<string> Aliases => [];
        public string Help => "Give an item.\nUsage: give <itemName|itemId>";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;


            if (parameters.Count < 2)
            {
                player.WriteLine("Nothing to give");
                return false;
            }

            // find item
            Item? i = character.FindItem(parameters[1]);
            Player? p = GameState.Instance.GetPlayerByName(parameters[1]);

            if (i == null)
            {
                player.WriteLine("No item to give");
                return false;
            }
            if (p == null)
            {
                player.WriteLine("No player found");
                return false;
            }
            else
            {
                p.BackPack.Items.Add(i);
                player.BackPack.Items.Remove(i);
                player.WriteLine($"Gave {i} To {p} ");
            }
            return true;


        }
    }

    internal class DropCommand : ICommand
    {
        public string Name => "drop";
        public IEnumerable<string> Aliases => [];
        public string Help => "Drop an item.\nUsage: drop <itemName|itemId>";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            Room room = player.GetRoom();

            if (parameters.Count < 2)
            {
                player.WriteLine("Nothing to drop");
                return false;
            }

            // find item
            Item? i = character.FindItem(parameters[1]);

            if (i == null)
            {
                // couldnt find
                return false;
            }
            else
            {
                room.Items.Add(i);
                player.BackPack.Items.Remove(i);
                player.WriteLine($"Dropped {i}");
            }
            return true;


        }
    }

    internal class GetCommand : ICommand
    {
        public string Name => "get";
        public IEnumerable<string> Aliases => [];
        public string Help => "Get an item.\nUsage: get <itemName|itemId>";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            if (parameters.Count < 2)
            {
                player.WriteLine("Nothing to get");
                return false;
            }

            Room room = player.GetRoom();


            // find item
            Item? i = room.FindItem(parameters[1]);
            
            if (i == null)
            {
                // couldnt find
                return false;
            }
            else
            {
                room.Items.Remove(i);
                player.BackPack.Items.Add(i);
                player.WriteLine($"Picked up {i}");
            }
            return true;


        }
    }



    internal class InvCommand : ICommand
    {
        public string Name => "inv";
        public IEnumerable<string> Aliases => [];
        public string Help => "Show your inventory.\nUsage: inv";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            if (player.BackPack.Items.Count < 1)
            {
                player.WriteLine("No items in your BackPack");
                return false;
            }
            foreach (Item i in player.BackPack.Items)
                player.WriteLine(i.Name);
                return true;


        }
    }


   

    internal class EquipmentCommand : ICommand
    {
        public string Name => "equip";
        public IEnumerable<string> Aliases => [];
        public string Help => "Equip an item, weapon or armor.\nUsage: equip <name>";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            List<Armor> armorItems = [];
            foreach (Item i in player.BackPack.Items)
            {
                if (i is Armor a)
                { armorItems.Add(a); }
            }
            List<Weapon> weaponItems = new List<Weapon>();
            foreach (Item i in player.BackPack.Items)
            {
                if (i is Weapon a)
                { weaponItems.Add(a); }
            }
            List <Food> foodItems = new List<Food>();
            foreach (Item i in player.BackPack.Items)
            {
                if (i is Food a)
                { foodItems.Add(a); }
            }
            List<Potion> potionItems = new List<Potion>();
            foreach (Item i in player.BackPack.Items)
            {
                if (i is Potion a)
                { potionItems.Add(a); }
            }



            return false;
        }
    }
    internal class UseCommand : ICommand
    {
        public string Name => "use";
        public IEnumerable<string> Aliases => new List<string> { };
        public string Help => "";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            // check if 2 params

            // find obj in inv that matches p[1]

            // is it consum

            Item? i = player.BackPack.GetItemByName(parameters[1]);
            Consumable? c = null;

            if (i == null || i is not Consumable)
            {
                // not coukgt find
            }
            else
            {
                c = (Consumable)i;
                if (c.UsesLeft > 0)
                {
                    c.UsesLeft--;
                    //c.Use();

                }
            }

            return true;



        }
    }


    

    internal class AFKCommand : ICommand
    {
        public string Name => "afk";
        public IEnumerable<string> Aliases => [];
        public string Help => "Toggles your AFK (Away From Keyboard) status. This just changes your display name.";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.IsAFK = !player.IsAFK;
                player.WriteLine($"You are now {(player.IsAFK ? "AFK" : "no longer AFK")}.");
                return true;
            }
            return false;
        }
    }


    internal class IpCommand : ICommand
    {
        public string Name => "ip";
        public IEnumerable<string> Aliases => [];
        public string Help => "Show the IP address you are connecting to the server from.";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.WriteLine($"Your IP address is {player.GetIPAddress()}");
                return true;
            }
            return false;
        }
    }

    internal class LookCommand : ICommand
    {
        public string Name => "look";
        public IEnumerable<string> Aliases => [ "l" ];
        public string Help => "";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                // For now, we'll ignore the command and just show the room description
                string content = $"{player.GetRoom().Description}\n";
                content += "[red]Exits:[/]\n";
                foreach (var exit in player.GetRoom().GetExits())
                {
                    content += $"{exit.Description} to the {exit.ExitDirection}\n";
                }
                content += "[Green]Players Here:[/]\n";
                content += $"{player.DisplayName()}";
                Panel panel = RPGPanel.GetPanel(content, player.GetRoom().Name);
                player.Write(panel);

                var table = new Table();
                table.AddColumn("[deepskyblue1]Items Here:[/]").Centered();

                foreach (Item item in player.GetRoom().Items)
                {
                    table.AddRow($"{item.DisplayText}");
                }
                return true;
            }
            return false;
        }
    }

    internal class QuitCommand : ICommand
    {
        public string Name => "quit";
        public IEnumerable<string> Aliases => [ "exit" ];
        public string Help => "";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.Logout();
                return true;
            }
            return false;
        }
    }

    internal class SayCommand : ICommand
    {
        public string Name => "say";
        public IEnumerable<string> Aliases => [ "\"", "'" ];
        public string Help => "";

        public bool Execute(Character character, List<string> parameters)
        {
            // If no message and it's a player, tell them to say something
            if (parameters.Count < 2 && character is Player player)
            {
                player.WriteLine("Say what?");
                return true;
            }
            Comm.RoomSay(character.GetRoom(), parameters[1], character);
            return true;
        }
    }

    internal class TellCommand : ICommand
    {
        public string Name => "tell";
        public IEnumerable<string> Aliases => [ "msg", "whisper" ];
        public string Help => "";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            if (parameters.Count < 3)
            {
                player.WriteLine("Tell whom what?");
                return false;
            }
            string targetName = parameters[1];
            string message = string.Join(' ', parameters.Skip(2));
            Player? targetPlayer = GameState.Instance.GetPlayerByName(targetName);

            if (targetPlayer == null)
            {
                Comm.SendToIfPlayer(character, $"Player '{targetName}' not found.");

                return false;
            }

            // Probably should check if target is online

            targetPlayer.WriteLine($"{Messaging.CreateTellMessage(player.DisplayName(), message)}");
            player.WriteLine($"You tell {targetPlayer.DisplayName()}: {message}");
            return true;
        }
    }

    internal class TimeCommand : ICommand
    {
        public string Name => "time";
        public IEnumerable<string> Aliases => new List<string> { };
        public string Help => "";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.WriteLine($"The time is {GameState.Instance.GameDate.ToShortTimeString()}");
                return true;
            }
            return false;
        }
    }

    internal class StatusCommand : ICommand
    {
        public string Name => "status";
        public IEnumerable<string> Aliases => [];
        public string Help => "";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                int onlineCount = 0;
                long memory = GC.GetTotalMemory(true);
                player.WriteLine($"The Server started at {GameState.Instance.ServerStartTime}.");
                player.WriteLine($"There are currently {System.Diagnostics.Process.GetCurrentProcess().Threads} running.");
                player.WriteLine($"The system is currently using {memory}.");
                foreach (Player playerc in GameState.Instance.GetPlayersOnline())
                {
                    string name = playerc.DisplayName();
                    onlineCount++;
                    return true;
                }
                player.WriteLine($"There is currently {onlineCount} players online.");
                player.WriteLine($"It is currently {GameState.Instance.GameDate}.");

                return true;
            }

            return false;
        }
    }

    internal class HelpCommand : ICommand
    {
        public string Name => "help";
        public IEnumerable<string> Aliases => [];
        public string Help => "";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            // if no help topic given
            if (parameters.Count < 2)
            {var table = new Table();
                table.AddColumn("═════ ⋆★⋆ ═════");
                table.AddColumn("════ ⋆★⋆ ════");
                table.AddColumn("════ ⋆★⋆ ════");
                table.AddColumn("════ ⋆★⋆ ════");
                //table.Title = new TableTitle("[mediumpurple2]Help Topics[/]");


                List<string> helpTopics = [];
                //foreach (HelpEntry he in GameState.Instance.HelpCatalog.Values)
                List<string> helpKeys = GameState.Instance.HelpCatalog.Keys.ToList();
                helpKeys.Sort();
                foreach (string key in helpKeys)
                {
                    HelpEntry he = GameState.Instance.HelpCatalog[key];
                    //player.WriteLine($"{he.Name}");
                    helpTopics.Add(he.Name);
                    if (helpTopics.Count == 4)
                    {
                        table.AddRow(helpTopics[0], helpTopics[1], helpTopics[2], helpTopics[3]);
                        helpTopics.Clear();
                    }

                }

                if (helpTopics.Count > 0)
                {
                    table.AddRow(
                    helpTopics.ElementAtOrDefault(0) ?? "",
                    helpTopics.ElementAtOrDefault(1) ?? "",
                    helpTopics.ElementAtOrDefault(2) ?? "",
                    helpTopics.ElementAtOrDefault(3) ?? "");
                    Panel panel = RPGPanel.GetPanel(table, "[mediumpurple2] Help Topics[/]");
                    player.Write(panel);
                }
            }
            else
            {
                foreach (HelpEntry he in GameState.Instance.HelpCatalog.Values)
                {
                    if (he.Name.Equals(parameters[1], StringComparison.CurrentCultureIgnoreCase))
                    {
                        player.WriteLine($"{he.Name}");
                        player.WriteLine($"{he.Content}");

                    }
                }
            }
            return true;
        }

    }

    internal class CheckWeatherCommand : ICommand
    {
        public string Name => "weather";
        public IEnumerable<string> Aliases => [];
        public string Help => "";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                // make a weather property for areas later
                Area area = player.GetArea();
                player.WriteLine($"The current weather is {area.Weather}");
                return true;
            }
            return false;

        }
    }
    internal class XPCommand : ICommand
    {
        public string Name => "xp";
        public IEnumerable<string> Aliases => [];
        public string Help => "";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.WriteLine($"You have {player.XP} XP. You need  {player.Levels[player.Level].RequiredXp - player.XP} XP");
                return true;
            }
            return false;
        }
    }

    internal class WeatherSetCommand : ICommand
    {
        public string Name => "setweather";
        public IEnumerable<string> Aliases => [];
        public string Help => "";
        public bool Execute(Character character, List<string> parameters)
        {

            if (character is not Player player)

                return false;

            if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
            {
                player.WriteLine("You do not have permission to use this command.");
                return false;
            }

            if (parameters.Count < 2)
            {
                player.WriteLine("Set weather to what?");
                return true;
            }
            else
            {
                Area area = player.GetArea();
                area.Weather = parameters[1];
                player.WriteLine($"You set the weather to {area.Weather}");
                return true;
            }


        }
    }
    internal class GoldCommand : ICommand
    {
        public string Name => "gold";
        public IEnumerable<string> Aliases => [];
        public string Help => "";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
                {
                    player.WriteLine("You do not have permission to run this command");
                    return false;
                }
                player.Gold += int.Parse(parameters[2]);
                player.WriteLine($"you have added {parameters[2]} to {parameters[1]}");
                return true;
            }
            return false;

        }
    }
    internal class HealCommand : ICommand
    {
        public string Name => "heal";
        public IEnumerable<string> Aliases => [];
        public string Help => "";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
                {
                    player.WriteLine("You do not have permission to run this command");
                    return false;
                }
                if (parameters[1] == null)
                {
                    player.WriteLine("Player not found.");
                    return false;
                }
                if (parameters[2] == null)
                {
                    player.WriteLine("No health amount stated.");
                    return false;
                }
                Player? target = GameState.Instance.GetPlayerByName(parameters[1]);
                target.Health += int.Parse(parameters[2]);
                player.WriteLine($"you have healed {target} by {parameters[1]}");
                return true;
            }
            return false;

        }
    }
    internal class DamageCommand : ICommand
    {
        public string Name => "damage";
        public IEnumerable<string> Aliases => [];
        public string Help => "";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
                {
                    player.WriteLine("You do not have permission to run this command");
                    return false;
                }
                if (parameters[1] == null)
                {
                    player.WriteLine("Player not found.");
                    return false;
                }
                if (parameters[2] == null)
                {
                    player.WriteLine("No Damage amount stated.");
                    return false;
                }
                Player Target = GameState.Instance.GetPlayerByName(parameters[1]);
                Target.Health -= int.Parse(parameters[2]);
                player.WriteLine($"you have damaged {Target} by {parameters[1]}");
                return true;
            }
            return false;

        }
    }
    internal class PurgeRoomCommand : ICommand
    {
        public string Name => "purge room";
        public IEnumerable<string> Aliases => [];
        public string Help => "";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

              if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
              {
                  player.WriteLine("You do not have permission to run this command");
                  return false;
              }

              foreach (Area a in GameState.Instance.Areas.Values)
              {
                  foreach (Room r in a.Rooms.Values)
                  {
                      foreach (Item i in r.Items)
                      {
                          if (i.IsDropped) // this was added through the weather finalization, once it is pulled it will work, probably
                              r.Items.Remove(i);
                      }
                  }
              }
            return true;
            }
        
        }
                
    internal class TimeRateCommand : ICommand
    {
        public string Name => "/timerate";
        public IEnumerable<string> Aliases => ["/tr"];
        public string Help => "";
        public bool Execute(Character character, List<string> parameters)
        {

            if (character is not Player player)
                return false;

            if (!Utility.CheckPermission (player, PlayerRole.Admin))
            {
                player.WriteLine("You do not have permission to use this command");
                return false;
            }
            if (parameters.Count < 2)
            {
                player.WriteLine("You did not provide a new rate for time passage");
                return false;
            }
            
            if (parameters.Count == 2) 
            {
                GameState.Instance.TimeRate = int.Parse(parameters[1]);
                player.WriteLine($"Time rate set to {GameState.Instance.TimeRate}");
                return true;
            }
            else
            {
                player.WriteLine("Improper use of timerate command");
                return false;
            }
        }
    }

    internal class ChangeTimeCommand : ICommand
    {
        public string Name => "/changetime";
        public IEnumerable<string> Aliases => [];
        public string Help => "";

        public bool Execute( Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            if (!Utility.CheckPermission (player, PlayerRole.Admin))
            {
                player.WriteLine("You do not have permission to use this command");
                return false;
            }

            if (parameters.Count < 2)
            {
                player.WriteLine("You need to provide an amount of time to change by");
                return false;
            }

            if (parameters.Count == 2)
            {
                double timeToAdd = int.Parse(parameters[1]);
                GameState.Instance.GameDate += TimeSpan.FromHours(timeToAdd);
                return true;
            }
            else
            {
                player.WriteLine("Inocrrect usage of changetime command");
                return false;
            }
        }
    }

    
    internal class LevelCommand : ICommand
    {
        public string Name => "level";
        public IEnumerable<string> Aliases => [];
        public string Help => "";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.WriteLine($"You are level {player.Level} you will gain an additional {player.Levels[player.Level].Health} health and you will have {player.Levels[player.Level].StatPoints} points upon level up.");
                return true;
            }
            return false;
        }
    }
    internal class SpawnCommand : ICommand
    {
        public string Name => "spawn";
        public IEnumerable<string> Aliases => [];
        public string Help => "";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;


            if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
            {
                player.WriteLine("You do not have permission to run this command");
                return false;
            }

            Item item = new()
            {
                Id = int.Parse(parameters[1])
            };
            player.GetRoom().Items.Add(item); // once item preconstruction exists come back to this
            return true;
        }
    }

    internal class TrainCommand : ICommand
    {
        public string Name => "train";
        public IEnumerable<string> Aliases => [];
        public string Help => "Train your attributes using stat points you have earned from leveling up.\nUsage: train <attribute>";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
            {
                return false;
            }
            if (parameters.Count < 2)
            {
                player.WriteLine($"to train you must type train and whatever you are trying to train for example, train strength.");
                return true;
            }
            if (player.StatPoints < 1)
            {
                player.WriteLine($"you dont have enough stat points or attribute doesnt exist");
                return true;
            }
           switch(parameters[1].ToLower())
            {
                case "Strength":
                    player.Strength++;
                    player.StatPoints--;
                    player.WriteLine($"added 1 point to strength");
                    break;
                case "dexterity":
                    player.Dexterity++;
                    player.StatPoints--;
                    player.WriteLine($"added 1 point to dexterity");
                break;
                case "constitution":
                    player.Constitution++;
                    player.StatPoints--;
                    player.WriteLine($"added 1 point to constitution");
                break;
                case "intelligence":
                    player.Intelligence++;
                    player.StatPoints--;
                    player.WriteLine($"added 1 point to intelligence");
                break;
                case "wisdom":
                    player.Wisdom++;
                    player.StatPoints--;
                    player.WriteLine($"added 1 point to wisdom");
                break;
                case "charisma":
                    player.Charisma++;
                    player.StatPoints--;
                    player.WriteLine($"added 1 point to charisma");
                break;
                default:
                    player.WriteLine("unkown attribute");
                break;
            }
            return false;
        }
    }
    internal class ExamineCommand : ICommand
    {         public string Name => "examine";
        public IEnumerable<string> Aliases => [ "ex", "exa" ];
        public string Help => "Examine an item in detail.\nUsage: examine <item name>";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
            {
                return false;
            }
            if (parameters.Count < 2)
            {
                player.WriteLine("Examine what?");
                return false;
            }
            string itemName = parameters[1];
            //if (player.GetRoom().Find TODO: implement FindItem method
            Item? item = player.GetRoom().Items
                .FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.CurrentCultureIgnoreCase));
            if (item == null)
            {
                player.WriteLine($"There is no '{itemName}' here to examine.");
                return false;
            }
            Panel panel = RPGPanel.GetPanel(item.Description, item.Name);
            player.Write(panel);
            return true;
        }
    }
}
