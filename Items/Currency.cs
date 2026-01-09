using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Items
{
    internal class Currency : Item
    {
        public int StartAmount { get; set; } = 1;
        public int MaxAmount { get; set; } = 500;
        

    }
}
