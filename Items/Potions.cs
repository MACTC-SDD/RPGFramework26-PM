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

        public void PotionValue()
        {
            switch (PotionType)
            {
                case PotionType.HealingPotion:
                    Value = 65;
                    break;
                case PotionType.ManaPotion:
                    Value = 65;
                    break;
            }
        }

        public void Healing()
        {
            switch (PotionType)
            {
                case PotionType.HealingPotion:
                    HealAmount = 100;
                    break;
            }
        }

        public void Mana()
        {
            switch (PotionType)
            {
                case PotionType.ManaPotion:
                    ManaAmount = 100;
                    break;
            }
        }



        public void PotionStack()
        {
            switch (this.PotionType)
            {
                case PotionType.HealingPotion:
                    StackAmount = 10;
                    break;
                case PotionType.ManaPotion:
                    StackAmount = 10;
                    break;
            }
        }    
    }
}
