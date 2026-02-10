

namespace RPGFramework
{
    internal partial class Spell
    {
        public int Damage { get; set; } = 0;
        public string Name { get; set; } = "";
    }

    internal partial class Player
    {
        public List<Spell> Spellbook { get; set; } = [];


        public List<Consumable> GetConsumables()
        {
            return [.. BackPack.Items.OfType<Consumable>()];
        }
    }
}
