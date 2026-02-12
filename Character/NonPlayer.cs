
using System.Runtime.CompilerServices;
using RPGFramework.Enums;
using RPGFramework.Geography;
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
        public List<string> Dialog = [];
        public bool IsAlive { get; set; } = false;
        public bool IsMagic { get; set; } = false;
        public bool IsMelee { get; set; } = false;
        public bool IsRanged { get; set; } = false;
        #region Clasification
        public bool IsArmy { get; set; } = false;
        public bool IsUndead { get; set; } = false;
        public bool IsVillager { get; set; } = false;
        public bool IsHumanoid { get; set; } = false;
        public bool IsElf { get; set; } = false;
        public bool IsCreature { get; set; } = false;
        #endregion
        public bool IsHostile { get; set; } = false;
        public string NpcClasification { get; set; } = "";
      

        // CODE REVIEW: Rylan (PR #16)
        // I'm adding HasElement and AttackPower properties so this will compile, but
        // I don't think they necessarily belong here. Please review and adjust as needed.
        public bool HasElement { get; set; } = false;
        public int AttackPower { get; set; } = 0;

        public void SaySomething()
        {
            if(Dialog.Count == 0)
            {  return; }

            // pick a dialog to display
            int n=GameState.Instance.Random.Next(Dialog.Count);
            Room r = GetRoom();
            Console.WriteLine($"room {r.Name}");
            Console.WriteLine($"d {Dialog[n]}");
            Comm.SendToRoomExcept(r,Dialog[n], this);
        }
        public static void TakeTurn(NonPlayer npc, CombatWorkflow combat)
        {
            GameState _instance = GameState.Instance;

            // NPC turn logic to be implemented
            //int action;

            //action = _instance.Random.Next(0, 2);

            // Attack
            Character? target = null;

            do
            {
                int targetIndex = _instance.Random.Next(0, combat.Combatants.Count - 1);
                if (combat.Combatants[targetIndex] == npc) target = null;
            } while (target == null);

            int damage = npc.CalculateDamage();
            target.TakeDamage(damage);

            Comm.SendToIfPlayer(target, $"{npc.Name} attacks you for {damage} damage!");               
        }

        public int CalculateDamage()
        {
            return PrimaryWeapon.RollDamage() + Strength;
        }
    }
}
