using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace RPGFramework.Combat
{
    
    //rounds will be 6 seconds
    //actions + bonus action + reaction etc
    //initialization will be DND initiative(random 1-20 + dexterity modifier((dexterity score - 10) / 2)
    internal class CombatActionMethods
    {
        public bool FleeCombat(Character character, CombatObject combat)
        {
        Random rand = new Random();
        int fleeRoll = rand.Next(1, 100);
            if (fleeRoll >= 80)
            {
                combat.combatants.Remove(character);
                if (character is Player player)
                    player.WriteLine("You successfully fled the combat!");
                return true;
            }
            else
            {
                if (character is Player player)
                    player.WriteLine("You failed to flee the combat!");
                return false;
            }
        }
    }
  
    internal class CombatObject {
        
        public void InitiativeOrder(List<Character> combatants)
        {
            combatants.Sort((x, y) => y.Initiative.CompareTo(x.Initiative));
        }
        public List<Character> combatants = new List<Character>();

        private int roundCounter { get; set; } = 0;



        public void CombatInitialization(Character attacker, Character enemy, CombatObject combat)
        {
            combatants.Add(attacker);
            combatants.Add(enemy);
            foreach (NonPlayer npc in attacker.GetRoom().GetNonPlayers())
            {
                //if (npc.Hostile == true || npc.Army == true)
                { 
                    combatants.Add(npc);
                }
            }
            foreach (Character c in combatants)
            {
                Random rand = new Random();
                int initiativeRoll = rand.Next(1, 20);
                int dexterityModifier = (c.Dexterity - 10) / 2;
                c.Initiative = initiativeRoll + dexterityModifier;
            }
            combat.InitiativeOrder(combatants);
        }

        public async Task CombatRound()
        {
            roundCounter++;
            foreach (Character c in combatants)
            {
                //each character takes their turn here
                
            }

        }

        

    }


}
