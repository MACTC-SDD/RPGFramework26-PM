

namespace RPGFramework
{
    internal class Spell
    {
        public int Damage { get; set; } = 0;
        public string Name { get; set; } = "";
    }

    internal partial class Player
    {
        public List<Spell> Spellbook { get; set; } = [];

        // Isn't this a duplicate of Character.Backpack?
        public List<Item> Inventory { get; set; } = [];

        public List<Consumable> GetConsumables()
        {
            return [.. Inventory.OfType<Consumable>()];
        }
    }
}
