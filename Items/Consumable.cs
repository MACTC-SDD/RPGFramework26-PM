using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Items
{
    internal class Consumable: Item
    {
        public int UsesRemaining { get; set; } = -1; // -1 means unlimited uses
    }
}

//POTIONS here?