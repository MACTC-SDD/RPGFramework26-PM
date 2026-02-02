using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Items
{
    internal class Inventory
    {
        public List<Item> Items { get; set; } = [];

        public Item? GetItemByName(string name)
        {
            name = name.ToLower();
            foreach (Item item in Items)
            {
                if (item.Name.ToLower() == name)
                    return item;
            }
            return null;
        }
    }
}
