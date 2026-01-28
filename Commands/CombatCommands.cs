using RPGFramework.Combat;
using RPGFramework;
using RPGFramework.Enums;
using RPGFramework.Workflows;


namespace RPGFramework.Commands
{

    internal class ConsiderCommand : ICommand
    {
        public string Name { get; set; } = "consider";
        public IEnumerable<string> Aliases => new List<string> { "/consider" };
        public bool Execute(Character character, List<string> parameters)
        {
            List<Character> charactersInRoom = new List<Character>();
            foreach (Character c in character.GetRoom().GetCharacters())
            {
                charactersInRoom.Add(c);
            }
            switch (parameters.Count)
            {
                case 1:
                    if (character is Player player)
                    {
                        player.WriteLine("Consider whom?");
                    }
                    return false;
                case 2:
                    {
                        bool found = false;
                        foreach (Character c in charactersInRoom)
                        {
                            if (c.Name == parameters[1])
                            {
                                found = true;
                                int levelDifference = c.Level - character.Level;
                                if (levelDifference >= 5)
                                {
                                    if (character is Player pl)
                                    {
                                        pl.WriteLine($"{c.Name} looks like a formidable opponent.");
                                        return true;
                                    }
                                }
                                else if (levelDifference >= 2)
                                {
                                    if (character is Player pl)
                                    {
                                        pl.WriteLine($"{c.Name} seems slightly stronger than you.");
                                        return true;
                                    }
                                }
                                else if (levelDifference >= -1 && levelDifference <= 1)
                                {
                                    if (character is Player pl)
                                    {
                                        pl.WriteLine($"{c.Name} appears to be evenly matched with you.");
                                        return true;
                                    }
                                }
                                else if (levelDifference >= -4)
                                {
                                    if (character is Player pl)
                                    {
                                        pl.WriteLine($"{c.Name} seems a bit weaker than you.");
                                        return true;
                                    }
                                }
                                else
                                {
                                    if (character is Player pl)
                                    {
                                        pl.WriteLine($"{c.Name} looks like an easy target.");
                                        return true;
                                    }

                                }
                            }

                        }
                        return found;
                    }
                default:
                    if (character is Player p)
                    {
                        p.WriteLine("Consider whom?");

                    }
                    return false;

            }
        }

        internal class ComabtStatusCommand : ICommand
        {

            public string Name => "combatstatus";
            public IEnumerable<string> Aliases => new List<string> { "/combatstatus", "/cs" };
            public bool Execute(Character character, List<string> parameters)
            {
                if (character is not Player player)
                    return false;


                if (Utility.CheckPermission(player, PlayerRole.Admin))
                {
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
                else
                {
                    player.WriteLine("You do not have permission to use this command.");
                    return false;
                }


            }
            private bool ShowCombatStatus(Player p, List<string> parameters)
            {
                CombatWorkflow currentCombat = null;
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

        internal class CombatCommands
        {
            internal class StartCombatCommand : ICommand
            {
                public string Name => "attack";
                public IEnumerable<string> Aliases => new List<string> { "/attack", "/a" };
                public bool Execute(Character character, List<string> parameters)
                {

                    List<string> attackableNonPlayers = new List<string>();

                    foreach (NonPlayer npc in character.GetRoom().GetNonPlayers())
                    {
                        attackableNonPlayers.Add(npc.Name);
                    }

                    List<string> attackablePlayers = new List<string>();

                    foreach (Player p in character.GetRoom().GetPlayers())
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
                            Character enemy = character.GetRoom().GetCharacters().Find(Character => Character.Name == parameters[1]);
                            CombatWorkflow.CreateCombat(character, enemy);
                            return true;
                        }
                    }
                    return false;
                }
            }
        }
    }

    internal class CombatAdminControls : ICommand
    {
        public void AdminStartCombatUntargeted(Player player)
        {
            NonPlayer? target = null;
            List<NonPlayer> possibleTargets = new List<NonPlayer>();
            foreach (NonPlayer npc in player.GetRoom().GetNonPlayers())
            {
                if (npc.IsHostile)
                {
                    possibleTargets.Add(npc);
                }
            }
            if (!possibleTargets.Any())
            {
                return;
            }
            Random random = new Random();
            target = possibleTargets[random.Next(0, possibleTargets.Count - 1)];
            CombatWorkflow.CreateCombat(player, target);
            return;
        }
        public void AdminStartCombatTargeted(Character a, Character e)
        {
            CombatWorkflow.CreateCombat(a, e);
        }
        public string Name => "/combat";
        public IEnumerable<string> Aliases => new List<string> { };
        public bool Execute(Character character, List<string> parameters)
        {
            Player? p = null;
            if (character is Player player)
            {
                p = character as Player;
            }
            else
            {
                return false;
            }
            if (Utility.CheckPermission(character as Player, PlayerRole.Admin) == false)
            {
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
                        p.WriteLine($"You need to provide a target player");
                        return false;
                    }
                    else if (parameters[1].ToLower() == "end")
                    {
                        p.WriteLine("You need to provide a target player");
                        return false;
                    }
                    else
                    {
                        p.WriteLine("You provided a subselect that does not exist");
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
                        AdminStartCombatUntargeted(target);
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
                        p.WriteLine("You provided a subselect that does not exist");
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
                        foreach (Character c in target1.GetRoom().GetCharacters())
                        {
                            if (c.Name == parameters[3])
                            {
                                target2 = c;
                                break;
                            }
                        }
                        if (target2 != null && target1 != null)
                        {
                            AdminStartCombatTargeted(target1, target2);
                            return true;
                        }
                        return false;
                    }
                    else if (parameters[1].ToLower() == "end")
                    {
                        p.WriteLine("You provided too many arguments, /combat end takes one target (e.g. /combat end player)");
                        return false;
                    }
                    else
                    {
                        p.WriteLine("You provided a subselect that does not exist");
                        return false;
                    }
                default:
                    p.WriteLine("You provided inproper arguments, /combat only has two subselects, \n" +
                        "start and end, start takes 1 or 2 arguments, a primary target and an optional secondary target,\n" +
                        "end takes one argument, a target character");
                    return false;
            }
        }

    }
}


