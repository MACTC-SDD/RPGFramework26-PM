using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace RPGFramework.Combat
{
    internal class Equipment_Durability
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
            Random rand = new Random();

            int randomNumber = rand.Next(0, 20);
            if (randomNumber == 20)
            {
                CriticalHit = true;
            }
            return CriticalHit;
        }

    }
}



