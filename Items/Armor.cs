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
                case ArmorType.Rags:
                    Durability = 25;
                    protection = 10;
                    break;
                case ArmorType.Hide:
                    Durability = 50;
                    protection = 25;
                    break;
                case ArmorType.ChainMail:
                    Durability = 15;
                    protection = 50;
                    break;
                case ArmorType.LightIron:
                    Durability = 20;
                    protection = 75;
                    break;
                case ArmorType.MediumIron:
                    Durability = 25;
                    protection = 100;
                    break;
                case ArmorType.HeavyIron:
                    Durability = 75;
                    protection = 150;
                    break;
                case ArmorType.Gold:
                    Durability = 50;
                    protection = 200;
                    break;
                case ArmorType.Fur:
                    Durability = 50;
                    protection = 25;
                    break;
                case ArmorType.Scale:
                    Durability = 65;
                    protection = 125;
                    break;
                case ArmorType.Shield:
                    Durability = 100;
                    protection = 50;
                    break;
            }
        }
    }
}
