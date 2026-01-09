using RPGFramework.Enums;
namespace RPGFramework
{
    internal class Armor : Item
    {
        public int protection { get; set; } = 0;
        public int Durability { get; set; } = 20;
        public ArmorType ArmorType { get; set; }

        public void ArmorDurablility()
        {
            switch (ArmorType)
            {
                case ArmorType.ChainMail:
                    Durability = 20;
                    break;
            }
        }
    }
}
