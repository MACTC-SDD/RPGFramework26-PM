
namespace RPGFramework
{
    internal class Item
    {
        public int Id { get; set; } = 0;
        public string Description { get; set; } = ""; // What you see when you look at it
        public string DisplayText { get; set; } = ""; // How it appears when in a room
        public bool IsDroppable { get; set; } // Can the item be dropped

        public int Durability { get; set; } //how durable the weapon is 

        public bool IsDropped { get; set; } = false; // Not sure what this is for? (Mr. Brown)
        public bool IsGettable { get; set; } // Can the item be picked up
        public bool IsPerishable { get; set; } = false;
        public string WeaponType { get; set; } = "";

        public int Level { get; set; } = 0;
        public string Name { get; set; } = "";
        public double SpawnChance { get; set; } = 0;
        public List<string> Tags { get; set; } = [];
        public double UseSpeed { get; set; } = 1;
        public double Value { get; set; } = 0;
        public double Weight { get; set; } = 0;
      
    }
}
