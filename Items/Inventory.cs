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
                // It doesn't seem like inventory should have any null items
                // but let's check anyway
                if (item == null) continue;
                if (item.Name.ToLower() == name)
                    return item;
            }
            return null;
        }
        public bool CheckCarryWeight(Player player, Item NewItem)
        {
            double CurrentInventory = 0;
            foreach (Item item in Items)
            {
                CurrentInventory += item.Weight;
            }
            if ((NewItem.Weight + CurrentInventory) >= player.MaxCarryWeight)
            {
                double diff = (NewItem.Weight + CurrentInventory) - player.MaxCarryWeight;
                player.WriteLine($"You can't pick up {NewItem.Name} because it exceeds your max carry capacity by {diff}");
                return false;
            } else
            {
                player.WriteLine($"You are currently holding {(CurrentInventory + NewItem.Weight)}.");
                return true;
            }

        }
    }
}
