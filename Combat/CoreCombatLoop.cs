using RPGFramework.Engine;
using RPGFramework.Workflows;
using System;
using RPGFramework;

namespace RPGFramework.Combat
{

    //rounds will be 6 seconds
    //actions + bonus action + reaction etc
    //initialization will be DND initiative(random 1-20 + dexterity modifier((dexterity score - 10) / 2)

    //CODE Review: Rylan (PR #19) - Initial 223 lines
    // Because we are doing so much player checking I made a simple method
    // in Comm called SendToIfPlayer(Character c, string message)
    internal class CombatObject {

        // This should be a property not a public field (always avoid public fields)
        public List<Character> Combatants { get; set; } =  new List<Character>();

        // If this is private, no need for a property, just make it a field and start with underscore
        private int _roundCounter = 0;

        public void InitiativeOrder()
        {
            // CODE REVIEW: Rylan
            // This is a good implementation of a bubble sort but you might 
            // consider using LINQ and sort. It's more efficient and easier to read.
            // Also, 1 line versus 11. Since this is an instance method we don't need to pass combatants as a parameter.
            Combatants = Combatants.OrderByDescending(c => c.Initiative).ToList();
        }

        // CODE REVIEW: It appears that combat parameter is redundant since this is an instance method.
        public async Task CombatInitialization(Character attacker, Character enemy)
        {
            Combatants.Add(attacker);
            Combatants.Add(enemy);
            foreach (NonPlayer npc in attacker.GetRoom().GetNonPlayers())
            {
                //if (npc.Hostile == true || npc.Army == true)
                { 
                    Combatants.Add(npc);
                }
            }
            foreach (Character c in Combatants)
            {
                // No need to create a new Random instance each iteration, already one in GameState
                int initiativeRoll = GameState.Instance.Random.Next(1, 20);
                int dexterityModifier = (c.Dexterity - 10) / 2;
                c.Initiative = initiativeRoll + dexterityModifier;
            }
            InitiativeOrder();
            RunCombat(); 
        }

        // This makes more sense as an instance method rather than static
        public bool FleeCombat(Character character)
        {
            int fleeRoll = GameState.Instance.Random.Next(1, 100);
            if (fleeRoll >= 80)
            {
                Combatants.Remove(character);
                Comm.SendToIfPlayer(character, "You successfully fled the combat!");
                return true;
            }
            else
            {
                Comm.SendToIfPlayer(character, "You failed to flee the combat!");
            }
            return false;
        }

        public static void RollToHitS(Character attacker, Spell weapon, Character target)
        {
            int attackRoll = GameState.Instance.Random.Next(1, 20);
            int attackModifier = (attacker.Intelligence - 10) / 2;
            int totalAttack = attackRoll + attackModifier;
            int targetAC = 10 + ((target.Dexterity - 10) / 2); //simplified AC calculation
            int damageModifier = (attacker.Intelligence - 10) / 2;
            int totalDamage = weapon.Damage + damageModifier;
            if (attackRoll == 20)
            { 
                target.TakeDamage(totalDamage * 2);
            }
            else if (attackRoll == 1)
            {
                Comm.SendToIfPlayer(attacker, $"You missed {target.Name} and hit yourself in the face!");
                totalAttack = 0;
                attacker.TakeDamage(1);
            }
            else if (totalAttack >= targetAC)
            { 
                target.TakeDamage(totalDamage);
                Comm.SendToIfPlayer(attacker, $"You hit {target.Name} for {totalDamage} damage!");
                Comm.SendToIfPlayer(target, $"{attacker.Name} hit you with {weapon.Name} for {totalDamage} damage!");            
            }
            else
                Comm.SendToIfPlayer(attacker, $"You missed {target.Name}!"); //miss
        }

        
        public static void RollToHit(Character attacker, Weapon weapon, Character target)
        {
            int attackRoll = GameState.Instance.Random.Next(1, 20);
            int attackModifier = (attacker.Strength - 10) / 2;
            int totalAttack = attackRoll + attackModifier;
            int targetAC = 10 + ((target.Dexterity - 10) / 2); //simplified AC calculation
            int damageModifier = (attacker.Strength - 10) / 2;
            int totalDamage = weapon.Damage + damageModifier;
            if (attackRoll == 20)
            {
                target.TakeDamage(totalDamage * 2);
            }
            else if (attackRoll == 1)
            {
                Comm.SendToIfPlayer(attacker, $"You missed {target.Name} and hit yourself in the face!");
                totalAttack = 0;
                attacker.TakeDamage(1);
            }
            else if (totalAttack >= targetAC)
            {
                target.TakeDamage(totalDamage);
                Comm.SendToIfPlayer(attacker, $"You hit {target.Name} for {totalDamage} damage!");
                Comm.SendToIfPlayer(target, $"{attacker.Name} hit you with {weapon.Name} for {totalDamage} damage!");
            }
            else
                Comm.SendToIfPlayer(attacker, $"You missed {target.Name}!"); //miss            
        }

        // CODE REVIEW: Rylan (PR #19)
        // We almost certainly don't want this pausing the thread with Task.Delay.
        // The good thing is that if we don't use that mechanism we won't have to make this method async.
        // Also I think this method makes more sense as an instance method.
        // Most likely handling rounds is best done by an outside process
        // we can revisit this later.
        public async Task RunCombat()
        {
            //main combat loop
            // CODE REVIEW: This doesn't account for multiple enemies/allies, but we discussed 
            // some teams mechanism.
            while (Combatants.Count >= 1)
            {
                _roundCounter++;               
                if (Combatants.Count <= 1)
                {
                    //combat ends
                    EndCombat();
                    return;
                }
                foreach (Character c in Combatants)
                {
                    //each character takes their turn here
                    if (c.Alive == false)
                    {                        
                        Combatants.Remove(c);
                        continue;
                    }
                    if (c is Player player)
                    {
                        player.CurrentWorkflow = new CombatTurnWorkflow();
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
                        NonPlayer.TakeTurn(npc, this);
                    }
                }
            }
            return;
        }

        // CODE REVIEW: Rylan (PR #16)
        // Added IsEngaged property to Character class to track combat status.
        // Added stub EngageCombat method to set IsEngaged to true for combatants.
        // This makes more sense as an instance method.
        public void EndCombat()
        {
            foreach (Character c in Combatants)
            {
                c.EngageCombat(false);
                if (c is Player player)
                {
                    player.WriteLine("Combat has ended!");
                    player.CurrentWorkflow = null;
                }
            }
            GameState.Instance.Combats.Remove(this);
        }
    }


}
