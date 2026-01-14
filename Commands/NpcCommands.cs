using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Commands
{
    internal class NpcCommands
    {
        public static List<ICommand> GetNpcCommands()
        {
            return new List<ICommand>
            {
                new NPCSpawnCommand()
                // Add other Npc commands here as they are implemented
            };
        }


    }

    internal class MobBuilderCommand : ICommand
    {
        public string Name => "/mob";
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
            switch (parameters[1].ToLower())
            {
                case "create":
                    MobCreate(player, parameters);
                    break;
                case "set":
                    // We'll move setting name and description into this
                    RoomSet(player, parameters);
                    break;
                case "delete":
                    ShowCommand(player, parameters);
                    break;
                case "List":
                    ShowCommand(player, parameters);
                    break;
                default:
                    WriteUsage(player);
                    break;
            }
            ////  what it will look like ---->  /Mob create kyler 'Long legged short hair'
            return false;
        }
        private static void WriteUsage(Player player)
        {
            player.WriteLine("Usage: ");
            player.WriteLine("/mob description '<set room desc to this>'");
            player.WriteLine("/mob name '<set room name to this>'");
            player.WriteLine("/mob create '<name>' '<description>' <exit direction> '<exit description>'");
        }
    }

}
