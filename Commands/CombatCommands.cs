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
                            CombatWorkflow.Create(character, enemy);
                            return true;
                        }
                    }
                    return false;
                }
            }
        }
    }
}
