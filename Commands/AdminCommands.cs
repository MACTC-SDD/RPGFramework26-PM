
using RPGFramework;
using RPGFramework.Core;
using RPGFramework.Display;
using RPGFramework.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace RPGFramework.Commands
{
    internal class AdminCommands
    {
        /* internal enum PlayerRole
         {
             Player,
             Builder,
             Admin,
             God
         }*/
        public static bool CheckPermission(PlayerRole role)
        {
            return PlayerRole.Player >= role;
        }
        public static List<ICommand> GetAllCommands()
        {
            return new List<ICommand>()
            {
                new AnnounceCommand(),
                new ShutdownCommand(),
                new WhereCommand(),
                new WhoCommand(),
                new GoToCommand(),
                new SaveAll(),
                new SummonCommand(),
                new KickCommand(),
                new RoleCommand(),
                new renameCommand(),
                new HelpEditCommand(),
                // Add more builder commands here as needed
            };
        }
    }

    internal class AnnounceCommand : ICommand
    {
        public static bool CheckPermission(PlayerRole role)
        {
            return PlayerRole.Player >= role;
        }
        public string Name => "announce";
        public IEnumerable<string> Aliases => new List<string>() { "ann" };
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                if (CheckPermission(PlayerRole.Admin) == true)
                {
                    Comm.Broadcast($"{DisplaySettings.AnnouncementColor}[[Announcement]]: [/][white]" +
                    $"{string.Join(' ', parameters.Skip(1))}[/]");
                    return true;
                }
                return false;
            }
            return false;
        }
    }
    internal class SaveAll : ICommand
    {
        public static bool CheckPermission(PlayerRole role)
        {
            return PlayerRole.Player >= role;
        }
        public string Name => "saveall";
        public IEnumerable<string> Aliases => new List<string>() { "ann" };
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                if (CheckPermission(PlayerRole.Admin) == true)
                {
                    GameState.Instance.SaveAllPlayers();
                }
                return false;
            }
            return false;
        }
    }

        internal class ShutdownCommand : ICommand
        {
            public static bool CheckPermission(PlayerRole role)
            {
                return PlayerRole.Player >= role;
            }

            public string Name => "shutdown";
            public IEnumerable<string> Aliases => new List<string>() { };
            public bool Execute(Character character, List<string> parameters)
            {
                if (character is Player player)
                {
                    if (CheckPermission(PlayerRole.Admin) == true)
                    {
                        Comm.Broadcast($"{DisplaySettings.AnnouncementColor}[[WARNING]]: [/][white]" +
                        $"Server is shutting down. All data will be saved.[/]");

                        GameState.Instance.Stop();
                        return true;
                    }
                    return false;
                }
                return false;
            }
        }

        internal class WhoCommand : ICommand
        {
            public string Name => "who";

            public IEnumerable<string> Aliases => new List<string>() { };

            public bool Execute(Character character, List<string> parameters)
            {
                if (character is Player player)
                {
                    //List<Player> = new List<Player>;
                    int onlineCount = 0;
                    foreach (Player playerc in GameState.Instance.GetPlayersOnline())
                    {
                        string name = playerc.DisplayName();
                        player.WriteLine(name + " is online.");
                        onlineCount++;
                        if (playerc.IsAFK == true)
                        {
                            player.WriteLine(name + " is AFK.");
                        }
                        player.WriteLine(playerc.GetRoom().ToString());



                    }
                    player.WriteLine($"Total online players: {onlineCount}.");
                    return true;
                }
            return false;
        }
        }
        internal class WhereCommand : ICommand
        {
            public string Name => "where";
            public static bool CheckPermission(PlayerRole role)
            {
                return PlayerRole.Player >= role;
            }

            public IEnumerable<string> Aliases => new List<string>() { };

            public bool Execute(Character character, List<string> parameters)
            {
                if (character is Player player)
                {
                    if (CheckPermission(PlayerRole.Admin) == true)
                    {
                        Player target = GameState.Instance.GetPlayerByDisplayName(parameters[1]);
                        player.WriteLine("That Player is in " + target.GetRoom());
                    }
                    return false;
                }
            return false;
        }
        }
        internal class GoToCommand : ICommand
        {
            public string Name => "goto";
            public bool CheckPermission(PlayerRole role)
            {
                return PlayerRole.Player >= role;
            }

            public IEnumerable<string> Aliases => new List<string>() { };

            public bool Execute(Character character, List<string> parameters)
            {
                if (character is Player player)
                {
                    if (CheckPermission(PlayerRole.Admin))
                    {
                        if (parameters[1] == null)
                        {
                            player.WriteLine("Player not found.");
                            return false;
                        }
                        // GameState.Instance.Players.Keys.Contains(parameters[1]);

                        Player target = GameState.Instance.GetPlayerByDisplayName(parameters[1]);
                        Player playerc = GameState.Instance.GetPlayerByDisplayName(parameters[2]);
                        playerc.AreaId = target.AreaId;

                        player.WriteLine($"You have been teleported to {target.DisplayName()}.");
                        return true;
                    }
                    return false;
                }
            return false;
        }
        }


        internal class SummonCommand : ICommand
        {
            public string Name => "summon";
            public bool CheckPermission(PlayerRole role)
            {
                return PlayerRole.Player >= role;
            }

            public IEnumerable<string> Aliases => new List<string>() { };

            public bool Execute(Character character, List<string> parameters)
            {
                if (character is Player player)
                {
                    if (CheckPermission(PlayerRole.Admin))
                    {
                        if (parameters[1] == null)
                        {
                            player.WriteLine("Player not found.");
                            return false;
                        }
                        // GameState.Instance.Players.Keys.Contains(parameters[1]);

                        Player target = GameState.Instance.GetPlayerByDisplayName(parameters[1]);
                        Player playerc = GameState.Instance.GetPlayerByDisplayName(parameters[2]);
                        target.AreaId = playerc.AreaId;

                        player.WriteLine($"You have teleported {target.DisplayName()} to you.");
                        return true;
                    }
                    return false;
                }
            return false;
        }
        }
        internal class KickCommand : ICommand
        {
            public string Name => "kick";
            public bool CheckPermission(PlayerRole role)
            {
                return PlayerRole.Player >= role;
            }

            public IEnumerable<string> Aliases => new List<string>() { };

            public bool Execute(Character character, List<string> parameters)
            {
                if (character is Player player)
                {
                    if (CheckPermission(PlayerRole.Admin))
                    {
                        if (parameters[1] == null)
                        {
                            player.WriteLine("Player not found.");
                            return false;
                        }
                        // GameState.Instance.Players.Keys.Contains(parameters[1]);

                        Player target = GameState.Instance.GetPlayerByDisplayName(parameters[1]);
                        if (target.IsOnline == true)
                            target.Logout();

                        player.WriteLine($"You have disconnected {target.DisplayName()}.");
                        return true;
                    }
                    return false;
                }
            return false;
        }
        }
        internal class RoleCommand : ICommand
        {
            public string Name => "role";
            public bool CheckPermission(PlayerRole role)
            {
                return PlayerRole.Player >= role;
            }

            public IEnumerable<string> Aliases => new List<string>() { };

            public bool Execute(Character character, List<string> parameters)
            {
                if (character is Player player)
                {
                    if (CheckPermission(PlayerRole.Admin))
                    {
                        if (parameters[1] == null)
                        {
                            player.WriteLine("Player not found.");
                            return false;
                        }
                        if (parameters[2] == null)
                        {
                            player.WriteLine("Role not found.");
                            return false;
                        }

                    // GameState.Instance.Players.Keys.Contains(parameters[1]);
                    if (!Enum.TryParse(parameters[2], true, out PlayerRole pr))
                    {
                        Player target = GameState.Instance.GetPlayerByDisplayName(parameters[1]);
                        target.PlayerRole = pr;
                        player.WriteLine($"You have changed {target.Name}'s role to {target.PlayerRole}.");
                        return true;
                    }

                    }
                    return false;
                }
                return false;
            }



        }
        internal class renameCommand : ICommand
        {
            public string Name => "rename";
            public bool CheckPermission(PlayerRole role)
            {
                return PlayerRole.Player >= role;
            }

            public IEnumerable<string> Aliases => new List<string>() { };

            public bool Execute(Character character, List<string> parameters)
            {
                if (character is Player player)
                {
                    if (CheckPermission(PlayerRole.Admin))
                    {
                        if (parameters[1] == null)
                        {
                            player.WriteLine("Player not found.");
                            return false;
                        }
                        // GameState.Instance.Players.Keys.Contains(parameters[1]);

                        Player target = GameState.Instance.GetPlayerByDisplayName(parameters[1]);
                        if (target.IsOnline == true)
                        {
                            target.Name = parameters[2];

                            player.WriteLine($"You have changed their name to {target.Name}");
                            return true;
                        }
                    }
                    return false;
                }
                return false;
            }
        }

    }
    internal class HelpEditCommand : ICommand
    {
        public string Name => "/help";
        public IEnumerable<string> Aliases => new List<string>() { };
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
            {
                return false;
            }

            if (parameters.Count > 1)
            {
                switch (parameters[1].ToLower())
                {
                    case "create":
                        CreateHelp(player, parameters);
                        break;
                    

                }
            }
            else
            {
                // Show usage message
            }

                // Do stuff
                return true;
        }

        public void CreateHelp(Player player, List<string> parameters)
        {
            HelpEntry h = new HelpEntry()
            {
                Name = parameters[2],
                Category = parameters[3],
                Content = parameters[4]
            };

            GameState.Instance.HelpEntries.Add(h);
        }
    }
}
