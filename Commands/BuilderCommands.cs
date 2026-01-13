
using RPGFramework.Enums;
using RPGFramework.Geography;

namespace RPGFramework.Commands
{
    internal class BuilderCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return new List<ICommand>
            {
                new RoomBuilderCommand(),
                // Add more builder commands here as needed
            };
        }
    }

    /// <summary>
    /// /room command for building and editing rooms.
    /// </summary>
    internal class RoomBuilderCommand : ICommand
    {
        public string Name => "/room";

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
                    RoomCreate(player, parameters);
                    break;
                case "set":
                    // We'll move setting name and description into this
                    RoomSet(player, parameters);
                    break;
                case "show":
                    ShowCommand(player, parameters);
                    break;
                default:
                    WriteUsage(player);
                    break;
            }

            return true;
        }

        private static void RoomSet(Player player, List<string> parameters)
        {
            if (parameters.Count < 3)
            {
                WriteUsage(player);
                return;
            }
            switch (parameters[2].ToLower())
            {
                case "description":
                    RoomSetDescription(player, parameters);
                    break;
                case "name":
                    RoomSetName(player, parameters);
                    break;
                // As we add more settable properties, we can expand this switch
                default:
                    WriteUsage(player);
                    break;
            }
        }

        private static void WriteUsage(Player player)
        {
            player.WriteLine("Usage: ");
            player.WriteLine("/room description '<set room desc to this>'");
            player.WriteLine("/room name '<set room name to this>'");
            player.WriteLine("/room create '<name>' '<description>' <exit direction> '<exit description>'");
        }

        private static void RoomCreate(Player player, List<string> parameters)
        {
            if (!Utility.CheckPermission(player, PlayerRole.Player))
            {
                player.WriteLine("You do not have permission to do that.");
                player.WriteLine("Your Role is: " + player.PlayerRole.ToString());
                return;
            }

            // 0: /room
            // 1: create
            // 2: name
            // 3: description
            // 4: exit direction
            // 5: exit description
            if (parameters.Count < 6)
            {
                player.WriteLine("Usage: /room create '<name>' '<description>' <exit direction> '<exit description>'");
                return;
            }

            if (!Enum.TryParse(parameters[4], true, out Direction exitDirection))
            {
                player.WriteLine("Invalid exit direction.");
                return;
            }

            try
            {
                Room room = Room.CreateRoom(player.AreaId, parameters[2], parameters[3]);

                player.GetRoom().AddExits(player, exitDirection, parameters[5], room);
                player.WriteLine("Room created.");
            }
            catch (Exception ex)
            {
                player.WriteLine($"Error creating room: {ex.Message}");
                player.WriteLine(ex.StackTrace);
            }
        }

        private static void RoomSetDescription(Player player, List<string> parameters)
        {
            if (!Utility.CheckPermission(player, PlayerRole.Admin))
            {
                player.WriteLine("You do not have permission to do that.");
                return;
            }

            if (parameters.Count < 4)
            {
                player.WriteLine(player.GetRoom().Description);
            }
            else
            {
                player.GetRoom().Description = parameters[3];
                player.WriteLine("Room description set.");
            }
        }

        private static void RoomSetName(Player player, List<string> parameters)
        {
            if (parameters.Count < 4)
            {
                player.WriteLine(player.GetRoom().Name);
            }
            else
            {
                player.GetRoom().Name = parameters[3];
                player.WriteLine("Room name set.");
            }
        }
             private static void ShowCommand(Player player, List<string> parameters)
        {
            
            
                Room r = player.GetRoom(); 
                player.WriteLine($"Name: {r.Name}");
                player.WriteLine($"Id: {r.Id.ToString()}");
                player.WriteLine($"Area Id: {r.AreaId.ToString()}");
                player.WriteLine($"Description: {r.Description}"); 
                

            
        }
    }
}
