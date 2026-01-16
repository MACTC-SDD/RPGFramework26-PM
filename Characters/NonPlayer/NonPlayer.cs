namespace RPGFramework
{
    internal abstract class NonPlayer : Character
    {
       
        Dictionary<NonPlayer, string> NpcList = new Dictionary<NonPlayer, string>();



        public bool IsMagic { get; set; } = false;
        public bool IsMelee { get; set; } = false;
        public bool IsRanged { get; set; } = false;
        public bool IsArmy { get; set; } = false;
        public bool IsUndead { get; set; } = false;
        public bool Isvillager { get; set; } = false;
        public bool IsHumanoid { get; set; } = false;
        public bool IsCreature { get; set; } = false;
        public bool IsHostile { get; set; } = false;

        public bool HasElement { get; set; } = false;
        public int AttackPower { get; set; } = 0;


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
                    int targetIndex = rand.Next(0, combat.combatants.Count - 1);
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
}