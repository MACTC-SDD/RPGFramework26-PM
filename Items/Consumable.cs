using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Items
{
    internal class Consumable: Item
    {
        public int usesLeft { get; set; } = 1;
    }
}

//POTIONS here?