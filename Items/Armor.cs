using RPGFramework.Enums;
namespace RPGFramework
{
    internal class Armor : Item
    {
        public int protection { get; set; } = 0;
        public int Durability { get; set; } = 20;
        public double weight { get; set; } = 0;
        public bool Equipped { get; set; } = false;
        public ArmorType ArmorType { get; set; }

        public void ArmorDurablility()
        {
            switch (ArmorType)
            {
                case ArmorType.Rags:
                    Durability = 25;
                    break;
                case ArmorType.Hide:
                    Durability = 50;
                    break;
                case ArmorType.ChainMail:
                    Durability = 15;
                    break;
                case ArmorType.LightIron:
                    Durability = 20;
                    break;
                case ArmorType.MediumIron:
                    Durability = 25;
                    break;
                case ArmorType.HeavyIron:
                    Durability = 75;
                    break;
                case ArmorType.Gold:
                    Durability = 50;
                    break;
                case ArmorType.Fur:
                    Durability = 50;
                    break;
                case ArmorType.Scale:
                    Durability = 65;
                    break;
                case ArmorType.Shield:
                    Durability = 100;
                    break;
            }
        }

        public void Armorprotection()
        {
            switch (ArmorType)
            {
                case ArmorType.Rags:
                    protection = 10;
                    break;
                case ArmorType.Hide:
                    protection = 25;
                    break;
                case ArmorType.ChainMail:
                    protection = 50;
                    break;
                case ArmorType.LightIron:
                    protection = 75;
                    break;
                case ArmorType.MediumIron:
                    protection = 100;
                    break;
                case ArmorType.HeavyIron:
                    protection = 150;
                    break;
                case ArmorType.Gold:
                    protection = 200;
                    break;
                case ArmorType.Fur:
                    protection = 25;
                    break;
                case ArmorType.Scale:
                    protection = 125;
                    break;
                case ArmorType.Shield:
                    protection = 50;
                    break;
            }
        }

        public void ArmorWeight()
        {
            switch (ArmorType)
            {
                case ArmorType.Rags:
                    Weight = 2;
                    break;
                case ArmorType.Hide:
                    Weight = 5;
                    break;
                case ArmorType.ChainMail:
                    Weight = 10;
                    break;
                case ArmorType.LightIron:
                    Weight = 10;
                    break;
                case ArmorType.MediumIron:
                    Weight = 20;
                    break;
                case ArmorType.HeavyIron:
                    Weight = 35;
                    break;
                case ArmorType.Gold:
                    Weight = 40;
                    break;
                case ArmorType.Fur:
                    Weight = 8;
                    break;
                case ArmorType.Scale:
                    Weight = 25;
                    break;
                case ArmorType.Shield:
                    Weight = 45;
                    break;
            }
        }
    }
}
    
