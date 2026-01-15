using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework
{
    internal class Spell
    {
        public int Damage { get; set; } = 0;
        public string Name { get; set; } = "";
    }

    internal partial class Player
    {
        public List<Spell> Spellbook { get; set; } = new List<Spell>();
        public List<Item> Inventory { get; set; } = new List<Item>();

        public List<Consumable> GetConsumables()
                    {
            List<Consumable> consumables = new List<Consumable>();
            foreach (Item item in Inventory)
            {
                if (item is Consumable consumable)
                {
                    consumables.Add(consumable);
                }
            }
            return consumables;
        }
    }

    internal class Consumable : Item
    {
        public int HealAmount { get; set; } = 0;
    }
}
