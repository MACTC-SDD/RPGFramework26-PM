using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Items
{
    internal class Food : Consumable
    {
        public int healAmount { get; set; } = 0;
        public int StackAmount { get; set; } = 1;
        public int StackMax { get; set; } = 10;
    }
}
