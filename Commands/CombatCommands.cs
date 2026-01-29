using RPGFramework.Enums;
using RPGFramework.Geography;
using RPGFramework.Workflows;


namespace RPGFramework.Commands
{
    internal class CombatCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return
                [
                    new ConsiderCommand(),
                    new CombatStatusCommand(),
                    new StartCombatCommand(),
                    new CombatAdminControlsCommand()
                ];
        }
    }

    #region ConsiderCommand Class
    internal class ConsiderCommand : ICommand
    {
        public string Name { get; set; } = "consider";
        public IEnumerable<string> Aliases => ["/consider"];
        public string Help => "";

        public bool Execute(Character character, List<string> parameters)
        {
            // CODE REVIEW: Rylan - I moved character finding to Room.FindCharacterInRoom
            // since it will be useful in other places. 
            // I also moved the consider text generation to Character.Consider(Character)
            // You might give that a look to see another way to use a switch statement to replace
            // if/else chains.
            if (parameters.Count < 2)
            {
                Comm.SendToIfPlayer(character, "Consider whom?");
                return false;
            }
            Character? c = Room.FindCharacterInRoom(character.GetRoom(), parameters[1]);

            if (c == null)
            {
                Comm.SendToIfPlayer(character, "They are not here.");
                return false;
            }

            return Comm.SendToIfPlayer(character, character.Consider(c));
        }
    }
    #endregion

    #region CombatStatusCommand Class
    internal class CombatStatusCommand : ICommand
    {
        public string Name => "combatstatus";
        public IEnumerable<string> Aliases => [ "/combatstatus", "/cs" ];
        public string Help => "";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            if (Utility.CheckPermission(player, PlayerRole.Builder))
            {
                player.WriteLine("You do not have permission to use this command.");
                return false;
            }

            switch (parameters.Count)
            {
                case 1:
                    player.WriteLine("Must choose a character to see the combat status");
                    return false;

                case 2:

                    return ShowCombatStatus(player, parameters);

                default:

                    player.WriteLine("Must choose a character to see the combat status");
                    return false;
            }
        }


        private static bool ShowCombatStatus(Player p, List<string> parameters)
        {
            CombatWorkflow? currentCombat = null;
            foreach (CombatWorkflow combat in GameState.Instance.Combats)
            {
                if (combat.Combatants.Contains(p))
                {
                    currentCombat = combat;
                    break;
                }
            }
            if (currentCombat != null)
            {
                p.WriteLine($"Current round count: {currentCombat.TurnTimer}");
                p.WriteLine("Combatants:");
                foreach (Character c in currentCombat.Combatants)
                {
                    p.WriteLine($"{c.Name} - HP: {c.Health}/{c.MaxHealth}");
                }
                return true;
            }
            else
            {
                return false;

            }
        }
    }

    internal class StartCombatCommand : ICommand
    {
        public string Name => "attack";
        public IEnumerable<string> Aliases => [ "/attack", "/a" ];
        public string Help => "";

        public bool Execute(Character character, List<string> parameters)
        {

            List<string> attackableNonPlayers = new List<string>();

            foreach (NonPlayer npc in character.GetRoom().NonPlayers)
            {
                attackableNonPlayers.Add(npc.Name);
            }

            List<string> attackablePlayers = new List<string>();

            foreach (Player p in Room.GetPlayersInRoom(character.GetRoom()))
            {
                attackablePlayers.Add(p.Name);
            }
            if (parameters.Count < 2)
            {
                if (character is Player player)
                {

                    player.WriteLine("Attackable NonPlayers");
                    foreach (string s in attackableNonPlayers)
                    {
                        player.WriteLine(s);
                    }
                    player.WriteLine("Attackable Players");
                    foreach (string s in attackablePlayers)
                    {
                        player.WriteLine(s);
                    }
                }
            }
            if (parameters.Count == 2)
            {
                if (attackablePlayers.Contains(parameters[1]) || attackableNonPlayers.Contains(parameters[1]))
                {
                    Character? enemy = Room.GetCharactersInRoom(character.GetRoom())
                        .Find(Character => Character.Name == parameters[1]);

                }
            }
            if (parameters.Count == 2)
            {
                if (attackablePlayers.Contains(parameters[1]) || attackableNonPlayers.Contains(parameters[1]))
                {
                    Character? enemy = Room.FindCharacterInRoom(character.GetRoom(), parameters[1]);
                    if (enemy == null)
                        return false;

                    CombatWorkflow.CreateCombat(character, enemy);
                    return true;
                }
            }
            return false;
        }
    }
    #endregion

    #region CombatAdminControlsCommand Class
    internal class CombatAdminControlsCommand : ICommand
    {
        public string Name => "/combat";
        public IEnumerable<string> Aliases => [];
        public string Help => "";

            // CODE REVIEW: Rylan - This method is quite long and complex.
            // This should be broken down into smaller methods for better
            // readability and maintainability.
            public bool Execute(Character character, List<string> parameters)
        {

            if (character is not Player player)
                    return false;
            
            if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
            {
                player.WriteLine("You do not have permission to use this command.");
                    return false;
            }

            switch (parameters.Count)
            {
                case 1:
                    player.WriteLine("The combat control command requires you to provide a subselect. (start, end, etc.)");
                    return false;
                case 2:
                    if (parameters[1].ToLower() == "start")
                    {
                        player.WriteLine($"You need to provide a target player");
                        return false;
                    }
                    else if (parameters[1].ToLower() == "end")
                    {
                        player.WriteLine("You need to provide a target player");
                        return false;
                    }
                    else
                    {
                        player.WriteLine("You provided a subselect that does not exist");
                        return false;
                    }
                case 3:
                    Player? target = null;
                    if (parameters[1].ToLower() == "start")
                    {

                        foreach (Player pl in GameState.Instance.Players.Values)
                        {
                            if (pl.Name == parameters[2])
                            {
                                target = pl;
                                break;
                            }
                        }
                        if (target == null)
                            return false;
                        // CODE REVIEW: Rylan - The method AdminStartCombatUntargeted is missing.
                        // AdminStartCombatUntargeted(target); 
                        return true;
                    }
                    else if (parameters[1].ToLower() == "end")
                    {
                        foreach (Player pl in GameState.Instance.Players.Values)
                        {
                            if (pl.Name == parameters[2])
                            {
                                target = pl;
                            }
                        }
                        if (target == null)
                            return false;
                        foreach (CombatWorkflow c in GameState.Instance.Combats)
                        {
                            if (c.Combatants.Contains(target))
                            {
                                c.EndCombat();
                                return true;
                            }
                        }
                        return false;
                    }
                    else
                    {
                        player.WriteLine("You provided a subselect that does not exist");
                        return false;
                    }
                case 4:
                    Player? target1 = null;
                    Character? target2 = null;
                    if (parameters[1].ToLower() == "start")
                    {

                        foreach (Player pl in GameState.Instance.Players.Values)
                        {
                            if (pl.Name == parameters[2])
                            {
                                target1 = pl;
                                break;
                            }
                        }

                        if (target1 == null) return false;

                        foreach (Character c in Room.GetCharactersInRoom(target1.GetRoom()))
                        {
                            if (c.Name == parameters[3])
                            {
                                target2 = c;
                                break;
                            }
                        }
                        if (target2 != null && target1 != null)
                        {
                            // CODE REVIEW: Rylan - The method AdminStartCombatTargeted is missing.
                            //AdminStartCombatTargeted(target1, target2);
                            return true;
                        }
                        return false;
                    }
                    else if (parameters[1].ToLower() == "end")
                    {
                        player.WriteLine("You provided too many arguments, /combat end takes one target (e.g. /combat end player)");
                        return false;
                    }
                    else
                    {
                        player.WriteLine("You provided a subselect that does not exist");
                        return false;
                    }
                default:
                    player.WriteLine("You provided inproper arguments, /combat only has two subselects, \n" +
                        "start and end, start takes 1 or 2 arguments, a primary target and an optional secondary target,\n" +
                        "end takes one argument, a target character");
                    return false;
            }
        }
    }
    #endregion
}












