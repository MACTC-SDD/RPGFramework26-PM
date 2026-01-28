using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework
{
    internal abstract partial class Character
    {
        public void DropItem(Character c, Item item)
        {
            if (c is Player p)
            {
                p.Inventory.Remove(item);
            }
            else if (item is Armor a)
                c.EquippedArmor.Remove(a);
            else if (item is Weapon w)
                c.PrimaryWeapon = null;

        }
    }
}
