using RPGFramework.Workflows;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework
{
    // CODE REVIEW: Rylan - can this be removed?
    // CODE REVIEW: Mr. Brown - no this cannot be removed as it is a stepping stool for both members of the combat team to work on the same thing
    internal partial class Character
    {
        public void Poisoned()
        {

        }
        public void Bleed()
        {
            this.TakeDamage((int)Math.Ceiling((double)this.MaxHealth / 100));
            this.HealPenalty = (int)Math.Ceiling((double)this.MaxHealth / 20);
        }
        public void Stun()
        {
            CombatWorkflow c = this.FindCombat();
            if (c != null)
            {
                c.EndTurn();
            }
            // this.IsStunned = false
        }
        public void Freightened()
        {
            // this.HitPenalty -= 7;
        }
        public void Incapacitated()
        {
            CombatWorkflow c = this.FindCombat();
            if (c != null)
            {
                c.EndTurn();
            }
        }

        public CombatWorkflow FindCombat()
        {
            CombatWorkflow? currentCombat = null;
            foreach (CombatWorkflow c in GameState.Instance.Combats)
            {
                if (c.Combatants.Contains(this))
                    currentCombat = c;
            }
            return currentCombat;
        }

        public void Petrified()
        {
            this.IsIncapacitated = true;
            this.CountPetrified++;
            this.Disadvantage += 5;
            if (this.CountPetrified >= 3)
            {
                this.DamageResistance = 2;
                if (this.IsPoisoned == true)
                    this.IsPoisoned = false;
                
            }
            else
            {
                this.DamageResistance = 1;
                this.Disadvantage -= 5;
                this.CountPetrified = 0;
                this.IsPetrified = false;
                this.IsIncapacitated = false;
            }
            
        }

        /*Poisoned,
        Bleed,
        Burn,
        Stun,
        Blinded,
        Charmed,
        Deafend,
        Frieghtened,
        Grappled,
        Incapacitated,
        Invisibe,
        Paralyzed,
        Petrified,
        Prone,
        Restrained,
        Unconscious,*/
    }
}
