using RPGFramework.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Items
{
    internal class Potion : Consumable
    {
        public int StackAmount { get; set; } = 1;
        public int StackMax { get; set; } = 10;

        public PotionType PotionType { get; set; } = 0;

        //Need strength potion
        public void PotionHeal()
        {
            switch (PotionType)
            {
                case PotionType.Potion_Of_Healing:
                    HealAmount = 100;
                    break;
            }
        }

        public void PotionStack()
        {
            switch (this.PotionType)
            {
                case PotionType.Potion_Of_Healing:
                    StackAmount = 10;
                    break;
            }
        }    
    }
}
