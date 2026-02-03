using RPGFramework.Workflows;
using System;
using System.Collections.Generic;
using System.Text;


namespace RPGFramework
{
    internal abstract partial class Character
    {
        public int HitPenalty { get; set; } = 0;
        public int HealPenalty { get; set; } = 0;
        public void Poisoned()
        {
            if (this.CountPoisoned == 0)
            {
                this.Disadvantage += 5;
            }
            if (this.CountPoisoned < 3)
            {
            this.TakeDamage(this.MaxHealth / 10);
                this.CountPoisoned++;
            }
            else
            {
                this.IsPoisoned = false;
                this.CountPoisoned = 0;
                this.Disadvantage -= 5;
            }
        }
        public void Bleed()
        {
            if (this.CountBleed == 0)
                this.HealPenalty += (int)Math.Ceiling((double)this.MaxHealth / 20);
            if (this.CountBleed < 3)
            {
                this.TakeDamage((int)Math.Ceiling((double)this.MaxHealth / 100));
                this.CountBleed++;
            }
            else
            {
                this.CountBleed = 0;
                this.IsBleed = false;
            }
        }
        public void Stun()
        {
            CombatWorkflow c = this.FindCombat();
            if (c != null)
            {
                if (this.CountStun < 3)
                {
                    this.CountStun++;
                    c.EndTurn();
                }
                else
                    this.IsStun = false;
            }
            
        }
        public void Freightened()
        {
            if (this.CountFreightened == 0)
            {
                this.HitPenalty += 3;
            }
            if (this.CountFreightened < 3)
            {
                this.CountFreightened++;
            }
            else
            {
                this.IsFreightened = false;
                this.CountFreightened = 0;
                this.HitPenalty -= 3;
            }
                
        }
        public void Incapacitated()
        {
            CombatWorkflow c = this.FindCombat();
            if (c != null)
            {
                c.EndTurn();
            }
            this.IsIncapacitated = false;
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
            if (this.CountPetrified == 0)
                this.Disadvantage += 5;
            if (this.CountPetrified < 3)
            {
                this.IsIncapacitated = true;
                this.CountPetrified++;
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
            }
            
        }
        
        public void Burn()
        {
            if (this.CountBurn == 0)
            {
                this.HealPenalty = (int)Math.Ceiling((double)this.MaxHealth / 20);
                this.IsBleed = false;
                this.CountBleed = 0;
        }
                
            if (this.CountBurn < 3)
            {
                this.TakeDamage(this.MaxHealth / 16);
                this.CountBurn++;
            }
            else
            {
                this.CountBurn = 0;
                this.HealPenalty -= (int)Math.Ceiling((double)this.MaxHealth / 20);
            }
            
        }
        public void Blinded()
        {
            if (this.CountBlind == 0)
            {
            HitPenalty -= 12;
            }
            if (this.CountBlind < 3)
            {
                this.CountBlind++;
            }
            else
            {
                this.CountBlind = 0;
                this.IsBlind = false;
                this.HitPenalty += 12;
            }
        }
        public void Deafened()
        {
            // once spells are more fleshed out add a way to grant disadvantage to saves
            // this is impractical as of now, ignore
            if (this.CountDeafened == 0)
            {
                this.Disadvantage += 5;
            }
            if (this.CountDeafened < 3)
            {
                this.CountDeafened++;
            }
            else
            {
                this.CountDeafened = 0;
                this.IsDeafened = false;
                this.Disadvantage -= 5;
            }

        }
        public void Grappled()
        {
            if (this.CountGappled == 0)
            {
                this.Disadvantage += 5;
            }
            if (this.CountGappled < 3)
            {
                this.CountGappled++; 
                this.IsIncapacitated = true;
            }
            else
            {
                this.Disadvantage -= 5; 
                this.IsIncapacitated = false;
                this.CountGappled = 0;
                this.IsGappled = false;
        }
        }
        public void Paralyzed()
        {
            // when paralyzed also incapacatated
            // attackers have advantage
            // fail strength and dex saves
            // all attacks that hit are a critical hit
            if (this.CountParalyzed == 0)
            {
                this.Disadvantage += 5;
                this.DamageResistance = 0.5;
        }
            if (this.CountParalyzed < 3)
            {
                this.CountParalyzed++;
                this.IsIncapacitated = true;
            }
            else
            {
                this.Disadvantage -= 5;
                this.IsParalyzed = false;
                this.CountParalyzed = 0;
                this.DamageResistance = 1;
            }
        }
        
        public void Unconscious()
        {
            // also incapacitated
            // drop what you are holding
            // attack rolls against you have advantage
            // any attack that hits is a critical hit
            if (this.CountUnconcious == 0)
            {
                // this.DropItem();
                this.Disadvantage += 5;
                this.DamageResistance = 0.5;
            }
            if (this.CountUnconcious < 3)
            {
                this.CountUnconcious++;
                this.IsIncapacitated = true;
        }
            else
            {
                this.Disadvantage -= 5;
                this.IsUnconcious = false;
                this.CountUnconcious = 0;
                this.DamageResistance = 1;
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
