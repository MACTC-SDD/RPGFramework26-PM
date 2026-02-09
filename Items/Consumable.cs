namespace RPGFramework
{
    internal class Consumable: Item
    {
        public int UsesLeft { get; set; } = -1; // -1 means unlimited uses
        public int HealAmount { get; set; } = 0;
        public int DuraRestore { get; set; } = 5;
    }
}

//POTIONS here?