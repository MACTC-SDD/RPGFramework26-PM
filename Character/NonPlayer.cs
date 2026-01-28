
using RPGFramework.Combat;
using RPGFramework.Enums;
using RPGFramework.Workflows;

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

        public bool IsMagic { get; set; } = false;
        public bool IsMelee { get; set; } = false;
        public bool IsRanged { get; set; } = false;
        public bool IsArmy { get; set; } = false;
        public bool IsUndead { get; set; } = false;
        public bool IsVillager { get; set; } = false;
        public bool IsHumanoid { get; set; } = false;
        public bool IsElf { get; set; } = false;
        public bool IsCreature { get; set; } = false;
        public bool IsHostile { get; set; } = false;


        // CODE REVIEW: Rylan (PR #16)
        // I'm adding HasElement and AttackPower properties so this will compile, but
        // I don't think they necessarily belong here. Please review and adjust as needed.
        public bool HasElement { get; set; } = false;
        public int AttackPower { get; set; } = 0;


        public static void TakeTurn(NonPlayer npc, CombatWorkflow combat)
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
                        int targetIndex = rand.Next(0, combat.Combatants.Count-1);
                        Character target = combat.Combatants[targetIndex];
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
