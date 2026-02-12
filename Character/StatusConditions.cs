using RPGFramework.Workflows;
using System;
using System.Collections.Generic;
using System.Text;


namespace RPGFramework
{
    internal abstract partial class Character
    {
        //public int HitPenalty { get; set; } = 0;
        //public int HealPenalty { get; set; } = 0;
        public void Poisoned()
        {
            if (this.CountPoisoned == 0)
            {
                this.Disadvantage += 5;
            }
            if (this.CountPoisoned < 3)
            {
                if (CountPoisoned > 0 && !CONSavingThrow(15, this))
                {
                    int damage = (int)((double)MaxHealth / (10 * Math.Ceiling((double)Level / 2)));
                    if (damage < MaxHealth / 50)
                        damage = (int)((double)MaxHealth / 50);
                    this.TakeDamage(damage);
                    this.CountPoisoned++;
                }
                else
                {
                    this.IsPoisoned = false;
                    this.CountPoisoned = 0;
                    this.Disadvantage -= 5;
                }
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
                this.TakeDamage((int)Math.Ceiling((double)this.MaxHealth / (50 * Math.Ceiling((double)Level / 2))));
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
                    if (CountStun > 0 && !CONSavingThrow(16, this))
                    {
                        this.CountStun++;
                        c.EndTurn();
                    }
                    else
                    {
                        this.CountStun = 0;
                        this.IsStun = false;
                    }
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
                if (!WISSavingThrow(16, this))
                {
                    this.CountFreightened++;
                }
                else 
                {                     
                    this.HitPenalty -= 3;
                    this.CountFreightened = 0;
                    this.IsFreightened = false;
                }

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
            {
                this.Disadvantage += 5;
                this.DamageResistance *= 2;
            }
            if (this.CountPetrified < 3)
            {
                if (!CONSavingThrow(18, this))
                {
                this.IsIncapacitated = true;
                this.CountPetrified++;
                if (this.IsPoisoned == true)
                    this.IsPoisoned = false;
                }
                else
                {
                    this.DamageResistance /= 2;
                    this.Disadvantage -= 5;
                    this.CountPetrified = 0;
                    this.IsPetrified = false;
                }
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
                this.CountBleed = 0;
        }
                
            if (this.CountBurn < 3)
            {
                this.TakeDamage((int)Math.Ceiling(this.MaxHealth / (16 * Math.Ceiling((double)Level / 4))));
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
                if (!CONSavingThrow(16, this))
                    this.CountBlind++;
                else
                {
                    this.CountBlind = 0;
                    this.HitPenalty += 12;
                    this.IsBlind = false;
                }
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
                if (!CONSavingThrow(16, this))
                    this.CountDeafened++;
                else
                {
                    this.CountDeafened = 0;
                    this.Disadvantage -= 5;
                    this.IsDeafened = false;

                }
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
                if (!STRSavingThrow(18, this))
                {
                    this.CountGappled++;
                    this.IsIncapacitated = true;
                }
                else
                {
                    this.CountGappled = 0;
                    this.Disadvantage -= 5;
                    this.IsGappled = false;
                    this.IsIncapacitated = false;
                }
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
                this.DamageResistance *= 0.5;
            }
            if (this.CountParalyzed < 3)
            {
                if (!CONSavingThrow(18, this))
                {
                    this.CountParalyzed++;
                    this.IsIncapacitated = true;
                }
                else
                {
                    this.CountParalyzed = 0;
                    this.Disadvantage -= 5;
                    this.IsParalyzed = false;
                    this.IsIncapacitated = false;
                    this.DamageResistance /= 0.5;
                }
            }
            else
            {
                this.Disadvantage -= 5;
                this.IsParalyzed = false;
                this.CountParalyzed = 0;
                this.DamageResistance /= 0.5;
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
                this.DamageResistance *= 0.5;
            }
            if (this.CountUnconcious < 3)
            {
                if (!CONSavingThrow(17, this))
                {
                    this.CountUnconcious++;
                    this.IsIncapacitated = true;
                }
                else
                {
                    this.CountUnconcious = 0;
                    this.Disadvantage -= 5;
                    this.IsUnconcious = false;
                    this.IsIncapacitated = false;
                    this.DamageResistance /= 0.5;
                }
            }
            else
            {
                this.Disadvantage -= 5;
                this.IsUnconcious = false;
                this.CountUnconcious = 0;
                this.DamageResistance /= 0.5;
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
