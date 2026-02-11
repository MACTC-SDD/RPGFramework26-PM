using RPGFramework.Geography;

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
               + "/roommob list \n"
               + "/roommob add 'Name' Spawn%\n"
               + "/roommob delete 'Name' \n";

            public bool Execute(Character character, List<string> parameters)
            {
                if (character is not Player player)
                    return false;

                if (parameters.Count < 2)
                    return ShowHelp(player);

                switch (parameters[1].ToLower())
                {
                    case "list":
                        return ListSpawnMobs(player, parameters);
                    case "add":
                        return AddSpawnMob(player, parameters);
                    case "delete":
                        return DeleteSpawnMob(player, parameters);
                    default:
                        return ShowHelp(player);
                }
            }

            public bool ShowHelp(Player player)
            {
                player.WriteLine(Help);
                return false;
            }

            #region ListSpawnMobs Method
            private static bool ListSpawnMobs(Player player, List<string> parameters)
            {
                Room r = player.GetRoom();

                player.WriteLine("All the Mobs:\nName (%)      Description"); //Put this into a table so it is organized for the player
                foreach (string mobName in r.MobSpawnList.Keys.OrderBy(o => o))
                {
                    if (!GameState.Instance.MobCatalog.TryGetValue(mobName, out Mob? mob) || null == mob)
                        continue;

                    player.WriteLine($"{mob.Name} ({r.MobSpawnList[mob.Name] * 100}%) - {mob.Description}");
                }
                return true;
            }
            #endregion

            #region AddSpawnMob Method 
            private static bool AddSpawnMob(Player player, List<string> parameters)
            {
                Room r = player.GetRoom();

                if (parameters.Count < 4)
                {
                    player.WriteLine("You have to specify a mob name and a spawn chance.");
                    return false;
                }
                string mobName = parameters[2].ToLower();

                if (!double.TryParse(parameters[3], out double spawnChance) || spawnChance < 0 || spawnChance > 1)
                {
                    player.WriteLine("Spawn chance needs to be a percentage (a number > 0 and < 1 (ie. .50)");
                    return false;
                }

                if (!GameState.Instance.MobCatalog.TryGetValue(mobName, out Mob? mob) || null == mob)
                {
                    player.WriteLine($"I couldn't find a mob named {mobName}.");
                    return false;
                }


                if (!r.MobSpawnList.TryAdd(mobName, spawnChance))
                {
                    player.WriteLine($"Changed mob spawn chance to {spawnChance}");
                    r.MobSpawnList[mobName] = spawnChance;
                }
                else
                {
                    player.WriteLine($"Added mob {mobName} with a spawn chance of {spawnChance}");
                }
                return true;
            }
            #endregion

            #region DeleteSpawnMob Method
            private static bool DeleteSpawnMob(Player player, List<string> parameters)
            {
                Room r = player.GetRoom();
                if (parameters.Count < 3)
                {
                    player.WriteLine("You have to specify a mob name to remove.");
                    return false;
                }

                string mobName = parameters[2].ToLower();

                if (r.MobSpawnList.Remove(mobName))
                    player.WriteLine($"Mob ({mobName}) remove from mob spawn list.");
                else
                    player.WriteLine("That mob wasn't spawning in this room.");

                return true;
            }
            #endregion
        }
    }
}
