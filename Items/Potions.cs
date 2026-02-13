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
        public void Use(Character user)
        {
            user.Heal(HealAmount);
        }

        public void PotionValue()
        {
            switch (PotionType)
            {
                case PotionType.HealingPotion:
                    Value = 650;
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

        public void PotionStack()
        {
            switch (this.PotionType)
            {
                case PotionType.HealingPotion:
                    StackAmount = 10;
                    break;
            }
        }    
    }
}
