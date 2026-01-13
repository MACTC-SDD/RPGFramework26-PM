using System;
using System.Collections.Generic;
using System.Text;
using RPGFramework.Combat;

namespace RPGFramework.Commands
{
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
                        CombatObject combat = new CombatObject();
                        GameState.Instance.Combats.Add(combat);
                        combat.CombatInitialization(character, enemy, combat);
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
