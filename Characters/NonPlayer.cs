
using RPGFramework.Combat;

namespace RPGFramework
{
    /// <summary>
    /// Represents a character in the game that is not controlled by a player.
    /// </summary>
    /// <remarks>Non-player characters (NPCs) may serve various roles such as quest givers, merchants, or
    /// enemies.</remarks>
    internal class NonPlayer : Character
    {
        // npc team need to fill this method out with logic for npc actions
        // like spells/element attacks, item usage, fleeing, basic attack options, etc.
        // contact combat team for help if needed, we know the combat system structure
        // DON'T RELY ON US TO DO IT FOR YOU, WE ARE NOT DESIGNING NPC BEHAVIOR (Logan)

        public static void TakeTurn(NonPlayer npc, CombatObject combat)
        {
            // NPC turn logic to be implemented
            int? action = null;
            if (npc.HasElement == true)
            {
                Random rand = new Random();
                action = rand.Next(0, 3);
            }
            else
            {
                Random rand = new Random();
                action = rand.Next(0, 2);
            }
                switch (action)
                {
                case 0:
                        // Attack
                        Random rand = new Random();
                        int targetIndex = rand.Next(0, combat.combatants.Count-1);
                        Character target = combat.combatants[targetIndex];
                        if (target != null)
                        {
                            target.TakeDamage(npc.AttackPower);
                        }
                        break;
                case 1:
                    //elemental attack(s)
                    //choose random from available elements
                    //copy target selection from above
                    //npc team fill this out with abilities(different element attacks, different basic attacks, other combat options)
                    break;
                default:
                    break;
            }
        }
    }
}
