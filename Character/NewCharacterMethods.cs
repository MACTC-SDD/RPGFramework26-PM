using System;
﻿using RPGFramework.Workflows;
using RPGFramework.Combat;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework
{
    internal abstract partial class Character
    {
        public void DropItem(Character c, Item item)
        {
            c.BackPack.Remove(item);
            c.GetRoom().Items.Add(item);
            item.IsDropped = true;
        }
    }
}
