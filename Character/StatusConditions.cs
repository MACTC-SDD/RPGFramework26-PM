using System;
using System.Collections.Generic;
using System.Text;


namespace RPGFramework
{
    // CODE REVIEW: Rylan - can this be removed?
    // CODE REVIEW: Mr. Brown - no this cannot be removed as it is a stepping stool for both members of the combat team to work on the same thing
    internal partial class Character
    {
        public int HitPenalty { get; set; } = 0;
        public int HealPenalty { get; set; } = 0;
        public void Poisoned()
        {
            // add a way to grant disadvantage or -7 to attack rolls
            this.TakeDamage(this.MaxHealth / 10);
        }
        public void Burn()
        {
            TakeDamage(this.MaxHealth / 16);
            HealPenalty -= MaxHealth / 20;
        }
        public void Blinded()
        {
            HitPenalty -= 12;
            // add a way to make it so you cant flee while blined
        }
        public void Deafened()
        {
            // once spells are more fleshed out add a way to grant disadvantage to saves
        }
        public void Grappled()
        {
            HitPenalty -= 5;
            //grant advantage to attackers
            //cant flee while grappled
        }
        public void Paralyzed()
        {
            // when paralyzed also incapacatated
            // attackers have advantage
            // fail strength and dex saves
            // all attacks that hit are a critical hit
        }
        
        public void Unconscious()
        {
            //also incapacitated
            // drop what you are holding
            // fail strength and dex saves
            //attack rolls against you have advantage
            // any attack that hits is a critical hit
        }
    }
}

// HitPenalty
// -=

/*        Poisoned,
        Bleed,
        Burn,
        Stun,
        Blinded,
        Charmed,
        Deafend,
        Frieghtened,
        Grappled,
        Incapacitated,
        Paralyzed,
        Petrified,
        Unconscious,*/