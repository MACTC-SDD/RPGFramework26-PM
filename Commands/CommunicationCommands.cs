
using RPGFramework.Geography;

namespace RPGFramework.Commands
{
    internal class CommunicationCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return new List<ICommand>
            {
                // Add other communication commands here as they are implemented
                new EmoteCommand(),
            };
        }


    }

    internal class SocialCommand : ICommand
    {
        public string Name => "/soc";
        public IEnumerable<string> Aliases => new List<string> { };
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
    internal class EmoteCommand : ICommand
    {
        public string Name => "emote";
        public IEnumerable<string> Aliases => new List<string> {"smile","wave","laugh","nod", "shrug", "cheer", "cry", "dance", "bow", "yawn", "poke" };
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
            {
                return false;
            }
            Room room = player.GetRoom();
            Player? target = null;

            if (parameters.Count > 1)
            {
                GameState.Instance.Players.TryGetValue(parameters[1], out target);
            }
            
            switch (parameters[0].ToLower())
            {
                case "smile":
                    Comm.SendToRoomExcept(room, target == null ? $"{player.Name} smiles" : $"{player.Name} smiles at {target.Name}", player);
                    player.WriteLine($"you smile");
                    break;
                case "wave":
                    Comm.SendToRoomExcept(room, target == null ? $"{player.Name} waves" : $"{player.Name} waves at {target.Name}", player);
                    player.WriteLine($"you wave");
                    break;
                case "laugh":
                    Comm.SendToRoomExcept(room, target == null ? $"{player.Name} laughs" : $"{player.Name} laughs at {target.Name}", player);
                    player.WriteLine($"you laugh");
                    break;
                case "nod":
                    Comm.SendToRoomExcept(room, target == null ? $"{player.Name} nods" : $"{player.Name} nods at {target.Name}", player);
                    player.WriteLine($"you nod");
                    break;
                case "shrug":
                    Comm.SendToRoomExcept(room, target == null ? $"{player.Name} shrugs" : $"{player.Name} shrugs at {target.Name}", player);
                    player.WriteLine($"you shrug");
                    break;
                case "cheer":
                    Comm.SendToRoomExcept(room, target == null ? $"{player.Name} cheers" : $"{player.Name} cheers at {target.Name}", player);
                    player.WriteLine($"you cheer");
                    break;
                case "cry":
                    Comm.SendToRoomExcept(room, target == null ? $"{player.Name} cries" : $"{player.Name} whines to {target.Name}", player);
                    player.WriteLine($"you cry");
                    break;
                case "dance":
                    Comm.SendToRoomExcept(room, target == null ? $"{player.Name} dances" : $"{player.Name} dances towards {target.Name}", player);
                    player.WriteLine($"you dance");
                    break;
                case "bow":
                    Comm.SendToRoomExcept(room, target == null ? $"{player.Name} bows" : $"{player.Name} bows to {target.Name}", player);
                    player.WriteLine($"you bow");
                    break;
                case "yawn":
                    Comm.SendToRoomExcept(room, target == null ? $"{player.Name} yawns" : $"{player.Name} yawns at {target.Name}", player);
                    player.WriteLine($"you yawn");
                    break;
                case "poke":
                    Comm.SendToRoomExcept(room, target == null ? $"{player} pokes nothing" : $"{player.Name} pokes {target.Name}", player);
                    player.WriteLine($"you poke nothing");
                    break;
            }

            return true;
        }
    }
}
