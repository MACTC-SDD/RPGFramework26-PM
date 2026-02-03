using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace RPGFramework.Combat
{
    // CODE REVIEW: Rylan - I'm not sure if where this the right place for this
    // but it looks like maybe you were just parking it here to get some thoughts down?
    // Also, since we are starting to get a lot of new code in here it's a good idea to start adding
    // at least a small summary comment to each class and method so we know what they are intended to do.
    internal class EquipmentDurability
    {
        public bool IsBroken { get; set; } = false;
        public double MaxDurability { get; set; } = 100;
        public double CurrentDurability { get; set; }
        public bool CriticalHit { get; set; }
        // add more later based on Item Team

        public void ReduceDurability(double amountLost)
        {
            if (CurrentDurability! < 0)
            {
                CurrentDurability -= amountLost;
                if (CurrentDurability <= 0)
                {
                    IsBroken = true;
                }

            }
        }
        public void IfDurabilityLost(double DamageTaken, int Level) //Change level later on when player team does it
        {
            if (DamageTaken >= 5 * Level)
            {
                ReduceDurability(MaxDurability / 15);
            }
            if (CriticalHit == true)
            {
                ReduceDurability(MaxDurability / 20);
            }
        }

        public bool IsCrit()
        {
            Random rand = new();

            int randomNumber = rand.Next(0, 20);
            if (randomNumber == 20)
            {
                CriticalHit = true;
            }
            return CriticalHit;
        }

    }
}



