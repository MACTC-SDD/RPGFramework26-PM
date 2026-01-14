using RPGFramework.Characters;
using RPGFramework.Engine;
using RPGFramework.Workflows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace RPGFramework.Combat
{
    
    //rounds will be 6 seconds
    //actions + bonus action + reaction etc
    //initialization will be DND initiative(random 1-20 + dexterity modifier((dexterity score - 10) / 2)
    
  
    internal class CombatObject {
        
        public void InitiativeOrder(List<Character> combatants)
        {
            int n = combatants.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                { 
                    if (combatants[j].Initiative < combatants[j + 1].Initiative)
                    {
                        Character temp = combatants[j];
                        combatants[j] = combatants[j + 1];
                        combatants[j + 1] = temp;
                    }
                }
            }
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
            CombatObject.RunCombat(combat);
        }

        public async Task CombatRound()
        {
            roundCounter++;
            foreach (Character c in combatants)
            {
                //each character takes their turn here
                
            }

        }
        
        public static bool FleeCombat(Character character, CombatObject combat)
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

        public static void RunCombat(CombatObject combat)
        {
            //main combat loop
            while (true)
            {
                combat.roundCounter++;
                if (combat.combatants.Count <= 1)
                {
                    //combat ends
                    CombatObject.EndCombat(combat);
                    return;
                }
                foreach (Character c in combat.combatants)
                {
                    //each character takes their turn here
                    if (c.Alive == false)
                    {
                        combat.combatants.Remove(c);
                        continue;
                    }
                    if (c is Player player)
                    {
                        player.CurrentWorkflow = new CombatTurnWorkflow();
                        //handle player turn
                    }
                    else if (c is NonPlayer npc)
                    {
                        //handle npc turn
                        NonPlayer.TakeTurn(npc, combat);
                    }
                }
            }
        }

        public static void EndCombat(CombatObject combat)
        {
            foreach (Character c in combat.combatants)
            {
                c.IsEngaged = false;
                if (c is Player player)
                {
                    player.WriteLine("Combat has ended!");
                    player.CurrentWorkflow = null;
                }
            }
            GameState.Instance.Combats.Remove(combat);
        }
    }


}
