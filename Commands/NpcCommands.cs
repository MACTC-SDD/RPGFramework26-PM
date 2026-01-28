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
            return new List<ICommand>
            {
                new MobBuilderCommand(),
                // Add other Npc commands here as they are implemented
            };
        }
        /*
        public void NPCSpawnCommand()
        {
            NonPlayer n = new NonPlayer();
            NPCList.Add(n);
        }
        */

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
                    //RoomSet(player, parameters);
                    break;
                case "delete":
                    MobDelete(player, parameters);
                    break;
                case "kill":
                    MobKill(player, parameters);
                case "list":
                    //ShowCommand(player, parameters);
                    break;
                default:
                    WriteUsage(player);
                    break;
            }
            ////  what it will look like ---->  /Mob create kyler 'Long legged short hair'
            return false;
        }

        private void MobCreate(Player player, List<string> parameters)
        {
            if (parameters.Count < 4)
            {
                player.WriteLine("Provide at least a name and description.");
                return;
            }

            if (GameState.Instance.MobCatalog.ContainsKey(parameters[2]))
            {
                player.WriteLine($"The mob {parameters[2]} already exists.");
                return;
            }

            Mob m = new Mob()
            {
                Name = parameters[2],
                Description = parameters[3]
            };

            GameState.Instance.MobCatalog.Add(m.Name, m);
            player.WriteLine($"{m.Name} added to the mob catalog.");
        }
        private void MobDelete(Player player, List<string> parameters)
        {
            if (parameters.Count < 4)
            {
                player.WriteLine("Provide at least a name and description.");
                return;
            }

            if (!GameState.Instance.MobCatalog.ContainsKey(parameters[2]))
            {
                player.WriteLine($"The mob {parameters[2]} does not exist.");
                return;
            }
            
            Mob m = GameState.Instance.MobCatalog[parameters[2]];
            GameState.Instance.MobCatalog.Remove(m.Name);
            player.WriteLine($"{m.Name} was removed the mob catalog.");
        }
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
        // NpcList.Add(NonPlayer, Nonplayer.Name)


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
    }

}
