using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using RPGFramework;
using Spectre.Console;



namespace RPGFramework.Commands
{
    internal class RoomMobCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return
            [
                new AddToList(),
                // Add other RoomNpc commands here as they are implemented
            ];
        }
        internal class AddToList : ICommand
        {
            public string Name => "/roommob";
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
                  //  ShowHelp(player);
                    return false;
                }
                switch (parameters[1].ToLower())
                {
                    case "list":
                        return MobSpawning(player, parameters);
                
                    default:
                        //ShowHelp(player);
                        break;
                }
                return false;
            }
            private static bool MobSpawning(Player player, List<string> parameters)
            {
                player.WriteLine("All the Mobs:\nName       Description"); //Put this into a table so it is organized for the player
                foreach (string mobName in player.GetRoom().MobSpawnList.Keys)
                {
                    if (!GameState.Instance.MobCatalog.TryGetValue(mobName, out Mob? mob) || null == mob)
                        continue;

                    player.WriteLine($"{mobName} - {mob.Description}");
                }
                return true;
            }

        }
    }
}
