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
    // HEAD
    
  
    /*internal class CombatObject {
        
        
=======
>>>>>>> 279bcfcafddd2254b6351e7038db17f6fb1f0520


    internal class CombatObject
    {
        private int roundCounter { get; set; } = 0;


        /* Commented out to compile since it looks like you've replaced this with the combat workflow
        public static async Task RunCombat(CombatObject combat)
        {
            //main combat loop
            while (combat.Combatants.Count >= 1)
            {
                combat.roundCounter++;
                if (combat.Combatants.Count <= 1)
                {
                    //combat ends
                    CombatObject.EndCombat(combat);
                    return;
                }
                foreach (Character c in combat.Combatants)
                {
                    //each character takes their turn here
                    if (c.Alive == false)
                    {
                        combat.Combatants.Remove(c);
                        continue;
                    }
                    if (c is Player player)
                    {
                        player.CurrentWorkflow = new CombatWorkflow();
                        //handle player turn
                        while (player.CurrentWorkflow != null)
                        {
                            await Task.Delay(100); 
                            // waits for player to finish their turn
                        }
                    }
                    else if (c is NonPlayer npc)
                    {
                        //handle npc turn
                        NonPlayer.TakeTurn(npc, combat);
                    }
                }
            }
            return;
        }

        // CODE REVIEW: Rylan (PR #16)
        // Added IsEngaged property to Character class to track combat status.
        // Added stub EngageCombat method to set IsEngaged to true for combatants.
        public static void EndCombat(CombatObject combat)
        {
            foreach (Character c in combat.Combatants)
            {
                c.EngageCombat(false);
                if (c is Player player)
                {
                    player.WriteLine("Combat has ended!");
                    player.CurrentWorkflow = null;
                }
            }
            GameState.Instance.Combats.Remove(combat);
        */
 


}
