using RPGFramework.Core;
using RPGFramework.Display;
using RPGFramework.Enums;
using RPGFramework.Persistence;
using RPGFramework.Workflows;
using RPGFramework.Geography;
using System.IO.Compression;
using System.Numerics;


namespace RPGFramework.Commands
{
    internal class AdminCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return
            [
                new AdminHealCommand(),
                new AdminHelpCommand(),
                new AnnounceCommand(),
                new BackupCommand(),
                new GoToCommand(),
                new HelpEditCommand(),
                new KickCommand(),
                new LevelUpCommand(),
                new MotdCommand(),
                new ReloadSeedDataCommand(),
                new RenameCommand(),
                new RoleCommand(),
                new SummonCommand(),
                new ShutdownCommand(),
                new SaveAll(),
                new WhereCommand(),
                new WhoCommand(),
                new TeleportRoomCommand(), // added teleport by room id
                new RestoreCommand(),
            ];
        }
    }

    #region Admin Help Command
    internal class AdminHelpCommand : ICommand
    {
        public string Name => "/admin";
        public IEnumerable<string> Aliases => [];
        public string Help => "Displays a list of admin commands.";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
            {
                player.WriteLine("You do not have permission to do that.");
                return false;
            }

            // If caller simply typed "/admin" show the list of admin commands and usage
            player.WriteLine("Admin Commands:");
            player.WriteLine("/announce <message>                        - Broadcast an announcement to all players");
            player.WriteLine("goto <targetPlayer> <playerToMove?>       - Move a playerToMove to targetPlayer's area (use carefully)");
            player.WriteLine("/help create <name> <category> <content>  - Create a help entry");
            player.WriteLine("kick <playerName>                         - Disconnect a player");
            player.WriteLine("/reloadseeddata                           - Reload seed data (requires confirmation: type YES!)");
            player.WriteLine("rename <targetPlayerName> <newName>       - Change a player's name");
            player.WriteLine("role <playerName> <role>                  - Set a player's role (Admin/Builder/Player etc)");
            player.WriteLine("summon <targetPlayerName> [[destPlayer]]    - Teleport target player to you or destPlayer if provided");
            player.WriteLine("/shutdown                                 - Shutdown the server");
            player.WriteLine("/saveall                                   - Save all players");
            player.WriteLine("where <playerName>                        - Show location for a player");
            player.WriteLine("who                                       - List online players");
            player.WriteLine("teleportroom <areaId>:<roomId> OR <roomId> - Teleport to a specific room (area optional)");
            return true;
        }
    }
    #endregion

    #region AnnounceCommand Class
    internal class AnnounceCommand : ICommand
    {
        public string Name => "/announce";
        public IEnumerable<string> Aliases => ["/ann"];
        public string Help => "Make an annoucement to all connected players.";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
            {
                player.WriteLine("You don't have permission to use this command.");
                return false;
            }

            Comm.Broadcast(Messaging.CreateAnnouncementMessage(string.Join(' ', parameters.Skip(1))));

            return true;
        }
    }
    #endregion

    // CODE REVIEW: Aidan
    // This method was looking for the player at parameter 0 so always failing
    // It also only set the area, not the location.
    // I have fixed these issues and you can delete once you've read this.
    #region GoToCommand Class
    internal class GoToCommand : ICommand
    {
        public string Name => "/goto";

        public IEnumerable<string> Aliases => [];
        public string Help => "Jump to a specific player:\n/goto <player name>";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player) return false;

            if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
            {
                player.WriteLine("You do not have permission to use this command.");
                return false;
            }


            if (parameters.Count < 1)
            {
                player.WriteLine("Usage: /goto <player name>");
                return false;
            }

            string targetName = parameters[1];

            if (!Player.TryFindPlayer(targetName, GameState.Instance.Players, out Player? target) || target == null)        
            {
                player.WriteLine("Player not found.");
                return false;
            }


            player.AreaId = target.AreaId;
            player.LocationId = target.LocationId;
            player.WriteLine($"You have been teleported to {target.Name}.");
            return true;
        }

        public bool ShowHelp(Player player)
        {
            player.WriteLine(Help);
            return false;
        }
    }
    #endregion

    #region HelpEditCommand Class
    internal class HelpEditCommand : ICommand
    {
        public string Name => "/help";
        public IEnumerable<string> Aliases => [];
        public string Help => "Usage: /help create <name> <category> <content>";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
            {
                return false;
            }

            if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
            {
                player.WriteLine("You do not have permission to use this command.");
                return false;
            }

            if (parameters.Count < 2)
            {
                ShowHelp(player);
                return true;
            }

            switch (parameters[1].ToLower())
            {
                case "create":
                    CreateHelp(player, parameters);
                    break;
                default:
                    ShowHelp(player);
                    break;
            }

            return true;
        }



        public bool CreateHelp(Player player, List<string> parameters)
        {
            if (parameters.Count < 5)
            {
                ShowHelp(player);
                return false;
            }

            if (GameState.Instance.HelpCatalog.ContainsKey(parameters[2]))
            {
                player.WriteLine("A help entry with that name already exists.");
                return false;
            }

            HelpEntry h = new()
            {
                Topic = parameters[2],
                Category = parameters[3],
                Content = parameters[4]
            };

            GameState.Instance.HelpCatalog.Add(h.Topic, h);
            return true;
        }

        public bool ShowHelp(Player player)
        {
            player.WriteLine(Help);
            return false;
        }
    }

    #endregion

    #region KickCommand Class
    // CODE REVIEW: Aidan - The KickCommand had several issues similar to those I addressed in SummonCommand.
    internal class KickCommand : ICommand
    {
        public string Name => "/kick";

        public IEnumerable<string> Aliases => [];
        public string Help => "Disconnect a player from the server. This does NOT ban them.";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player) return false;

            if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
            {
                player.WriteLine("You do not have permission to use this command.");
                return false;
            }

            if (parameters.Count < 1 || string.IsNullOrWhiteSpace(parameters[0]))
            {
                player.WriteLine("Usage: /kick <playername>");
                return false;
            }

            string targetName = parameters[0];
            Player? target = GameState.Instance.GetPlayerByName(targetName);

            if (target == null)
            {
                player.WriteLine($"Player ({targetName}) not found.");
                return false;
            }

            if (target.IsOnline)
            {
                target.Logout();
                player.WriteLine($"You have disconnected {target.DisplayName()}.");
                return true;
            }
            else
            {
                player.WriteLine($"{target.DisplayName()} is not currently online.");
                return false;
            }
        }
    }
    #endregion

    #region ReloadSeedDataCommand Class
    internal class ReloadSeedDataCommand : ICommand
    {
        public string Name => "/reloadseeddata";
        public IEnumerable<string> Aliases => [];
        public string Help => "Reload all seed data files. This won't delete existing files, but it will OVERWRITE them if they exist in seed_data. Use this with caution!";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
            {
                player.WriteLine("You do not have permission to use this command.");
                return false;
            }

            character.CurrentWorkflow = new WorkflowReloadSeedData();
            player.WriteLine("Watch out, you're about to overwrite your data with the default seed files. If that's what you want, type YES!");
            return true;
        }
    }
    #endregion

    #region RenameCommand Class
    // CODE REVIEW: Aidan - The renameCommand had several issues similar to those I addressed in SummonCommand.
    // Also, class names should be PascalCase, so I've renamed it to RenameCommand.
    internal class RenameCommand : ICommand
    {
        public string Name => "/rename";

        public IEnumerable<string> Aliases => [];
        public string Help => "Change a player's username.";

        // CODE REVIEW: Aidan - I un-nested this by moving character and permission checks to the 
        // beginning and exiting if they failed. This makes the code a lot more readable because we don't
        // have so many nested blocks.
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
                return false;

            // CODE REVIEW: Aidan - We should check if parameters has enough elements to avoid index out of range exceptions.
            if (parameters.Count < 3)
            {
                player.WriteLine("Usage: rename <targetPlayerName> <newName>");
                return false;
            }


            Character target = GameState.Instance.GetPlayerByName(parameters[1]);
            if (target == null)
            {
                player.WriteLine("Player not found.");
                return false;
            }

            if (Player.Exists(parameters[1], GameState.Instance.Players))
            {
                player.WriteLine("That name is already taken.");
                return false;
            }
            if (target is Player p)
            {
                GameState.Instance.Players.Remove(p.Name);
                target.Name = parameters[2];
                GameState.Instance.Players.Add(target.Name, p);

                player.WriteLine($"You have changed their name to {p.Name}");
                return true;
            }
            else
            {
                player.WriteLine("Your target was not a player");
                return false;
            }
        }
    }
    #endregion

    #region RoleCommand Class
    // CODE REVIEW: Aidan - The RoleCommand had several issues similar to those I addressed in SummonCommand.
    internal class RoleCommand : ICommand
    {
        public string Name => "/role";

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

            // CODE REVIEW: Aidan - We should check if parameters has enough elements to avoid index out of range exceptions.
            // We don't need null checking here
            if (parameters.Count < 3)
            {
                player.WriteLine("Usage: role <playerName> <role>");
                return false;
            }

            /*if (parameters[1] == null)
            {
                player.WriteLine("Player not found.");
                return false;
            }
            if (parameters[2] == null)
            {
                player.WriteLine("Role not found.");
                return false;
            }*/

            Character target = GameState.Instance.GetPlayerByName(parameters[1]);
            // CODE REVIEW: Aidan - Added null check for target to avoid potential null reference exception.
            if (target == null)
            {
                player.WriteLine($"Player ({parameters[1]}) not found.");
                return false;
            }

            // GameState.Instance.Players.Keys.Contains(parameters[1]);
            if (!Enum.TryParse(parameters[2], true, out PlayerRole pr))
            {
                if (target is Player p)
                {
                    p.PlayerRole = pr;
                    player.WriteLine($"You have changed {target.Name}'s role to {p.PlayerRole}.");
                    return true;
                }
                else
                {
                    player.WriteLine("Your target is not a player");
                    return false;
                }
            }
            else
            {
                player.WriteLine("Role not found.");
                return false;
            }
        }
    }
    #endregion

    #region SaveAll Class
    internal class SaveAll : ICommand
    {
        public string Name => "/saveall";
        public IEnumerable<string> Aliases => [];
        public string Help => "";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
            {
                player.WriteLine("You don't have permission to use this command.");
            }


            GameState.Instance.SaveAllPlayers();
            return true;
        }
    }
    #endregion

    #region ShutdownCommand Class
    internal class ShutdownCommand : ICommand
    {
        public string Name => "/shutdown";
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
            Comm.Broadcast($"{DisplaySettings.AnnouncementColor}[[WARNING]]: [/][white]" +
            $"Server is shutting down. All data will be saved.[/]");

            GameState.Instance.Stop();
            return true;
        }
    }
    #endregion

    #region SummonCommand Class
    internal class SummonCommand : ICommand
    {
        public string Name => "/summon";
        public IEnumerable<string> Aliases => [];
        public string Help => "";

        // CODE REVIEW: Aidan - Revised to use Utility.CheckPermission for consistency.
        // Also un-nested the code for better readability by moving checks to the start and exiting early.
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
            {
                player.WriteLine("You do not have permission to use this command.");
                return false;
            }

            // CODE REVIEW: Aidan - We don't need to check parameters[1] for null directly,
            // but we should check if parameters has enough elements to avoid index out of range exceptions.
            // This is doesn't mean the player exists, just that the command was used correctly.
            /*if (parameters[1] == null)
            {
                player.WriteLine("Player not found.");
                return false;
            }
            */

            if (parameters.Count < 2)
            {
                player.WriteLine("Usage: summon <targetPlayerName>");
                return false;
            }

            // CODE REVIEW: Aidan - It looks like your intent was not necessarily to just summon
            // a target player to the person running this code, but perhaps to summon them to 
            // another player. I think "summon" with a single argument should move the targe to
            // the player issuing the command. If a second argument is provided, it should move the target
            // to that player instead. I've adjusted the code accordingly.

            Character? target = GameState.Instance.GetPlayerByName(parameters[1]);
            if (target == null)
            {
                player.WriteLine("Target player not found.");
                return false;
            }

            // If we have a second parameter, try to get that player as the destination.
            Player? playerc = parameters.Count > 2 ? GameState.Instance.GetPlayerByName(parameters[2]) : null;
            Player destPlayer = playerc ?? player; // null coalescing operator to choose destination player

            target.AreaId = destPlayer.AreaId;
            target.LocationId = destPlayer.LocationId;

            if (target is Player p)
                p.WriteLine($"You have been summoned to {destPlayer.DisplayName()}.");
            player.WriteLine($"You have teleported {target.Name} to you.");
            return true;
        }
    }
    #endregion

    #region TeleportRoomCommand
    internal class TeleportRoomCommand : ICommand
    {
        public string Name => "/teleportroom";
        public IEnumerable<string> Aliases => ["/tpr"];
        public string Help => "Teleport to a specific room by ID. Usage: teleportroom <areaId>:<roomId> or teleportroom <roomId> (uses your current area)";
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
                player.WriteLine("Usage: teleportroom <areaId>:<roomId>   or teleportroom <roomId> (uses your current area)");
                return false;
            }

            if (!Room.TryParseId(parameters[1], character.AreaId, out int roomId, out int areaId))
            {
                player.WriteLine("Invalid destination format. Use <roomId> or <areaId>:<roomId>.");
                return false;
            }

            if (!GameState.Instance.Areas.TryGetValue(areaId, out Area? area) || !area.Rooms.TryGetValue(roomId, out Room? destRoom))
            {
                player.WriteLine($"Destination room not found (Area: {areaId}, Room: {roomId}).");
                return false;
            }

            // move the player correctly (call Leave/Enter so room notifications work)
            Room current = character.GetRoom();
            current.LeaveRoom(character, destRoom);
            destRoom.EnterRoom(character, current);

            character.AreaId = areaId;
            character.LocationId = roomId;

            player.WriteLine($"Teleported to Area {areaId} Room {roomId}.");
            return true;
        }
    }
    #endregion

    #region WhereCommand Class
    internal class WhereCommand : ICommand
    {
        public string Name => "/where";

        public IEnumerable<string> Aliases => [];
        public string Help => "Show where a specific player is.\nUsage: where <target player>";

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
                return ShowHelp(player);

            string targetName = parameters[1];

            if (!Player.TryFindPlayer(targetName, GameState.Instance.Players, out Player? target) || target == null)
            { 
                player.WriteLine("Player not found.");
                return false;
            }

            player.WriteLine($"That Player is in {target.GetRoom().Name} ({target.GetArea().Name})");
            return true;
        }

        public bool ShowHelp(Player player)
        {
            player.WriteLine(Help);
            return false;
        }
    }
    #endregion

    #region WhoCommand Class
    internal class WhoCommand : ICommand
    {
        public string Name => "who";

        public IEnumerable<string> Aliases => [];
        public string Help => "";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            int onlineCount = 0;
            foreach (Player playerc in GameState.Instance.GetPlayersOnline())
            {
                string name = playerc.Name;
                player.WriteLine(name + " is online.");
                onlineCount++;
                if (playerc.IsAFK == true)
                {
                    player.WriteLine(name + " is AFK.");
                }
                player.WriteLine(playerc.GetRoom().Name);
            }
            player.WriteLine($"Total online players: {onlineCount}.");
            return true;
        }
    }
    #endregion

    #region BackupCommand Class
    internal class BackupCommand : ICommand
    {
        public string Name => "/backup";

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


            try
            {
                GameState.CreateBackup();
                player.WriteLine("Backup created successfully.");
                return true;
            }
            catch (Exception ex)
            {
                player.WriteLine($"Backup failed: {ex.Message}");
                return false;
            }
        }


    }
    #endregion
    
    #region RestoreCommand Class
    internal class RestoreCommand : ICommand
    {
        public string Name => "restore";

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

            if (parameters.Count == 0)
            {
                player.WriteLine("Usage: /restore <backupName | latest>");
                return false;
            }

            try
            {
                GameState.RestoreBackup(parameters[0]);
                player.WriteLine("Restore completed successfully. Server restart recommended.");
                return true;
            }
            catch (Exception ex)
            {
                player.WriteLine($"Restore failed: {ex.Message}");
                return false;
            }
        }

    }
    #endregion

    #region MotdCommand Class
    internal class MotdCommand : ICommand
    {
        public string Name => "/motd";

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
                player.WriteLine(GameState.Instance.MessageCatalog.ContainsKey("motd")
                    ? GameState.Instance.MessageCatalog["motd"]
                    : "nothing");
                return false;
            }
            player.WriteLine($"successfully set motd");
            GameState.Instance.MessageCatalog["motd"] = parameters[1];
            return true;
        }
    }
    #endregion

    #region LevelUpCommand Class
    internal class LevelUpCommand : ICommand
    {
        public string Name => "/levelup";

        public IEnumerable<string> Aliases => [];
        public string Help => "";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
            {
                return false;
            }
            if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
            {
                player.WriteLine("You do not have permission to use this command.");
                return false;
            }
            player.LevelUp(1);
            player.WriteLine("you have given yourself one level");
            return true;
        }
    }
    #endregion

    #region AdminHealCommand Class
    // CODE REVIEW: Aidan/Rylan
    // This didn't really find a target or check to see if a number was passed
    // it just checked for null parameters, which actually would have crashed :)
    // It is fixed now, so this is just for reference. You can delete this comment once you review.
    // It also tried to set Health directly rather than using the Heal command.
    internal class AdminHealCommand : ICommand
    {
        public string Name => "/heal";
        public IEnumerable<string> Aliases => [];
        public string Help => "/heal <target player> <heal amount>";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;


            if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
            {
                player.WriteLine("You do not have permission to run this command");
                return false;
            }

            if (parameters.Count < 3)
                return ShowHelp(player);

            string targetName = parameters[1];

            Player? target = Player.FindPlayer(targetName, GameState.Instance.Players);

            if (target == null)
            {
                player.WriteLine("Player not found.");
                return false;
            }

            if (!int.TryParse(parameters[2], out int healAmount))
            {
                player.WriteLine("Heal amount needs to be a number!");
                return false;
            }

            target.Heal(healAmount);

            player.WriteLine($"You have [green]healed[/] {target.Name} by {healAmount}!");
            return true;
        }

        public bool ShowHelp(Player player)
        {
            player.WriteLine(Help);
            return false;
        }
    }
    #endregion
}