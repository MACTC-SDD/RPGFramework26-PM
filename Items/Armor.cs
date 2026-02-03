using RPGFramework.Enums;
namespace RPGFramework
{
    internal class Armor : Item
    {
        public ArmorType ArmorType { get; set; }
        public int Durability { get; set; } = 20;
        public bool Equipped { get; set; } = false;
        public int Protection { get; set; } = 0;

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

        public void ArmorProtection()
        {
            switch (ArmorType)
            {
                case ArmorType.Rags:
                    Protection = 10;
                    break;
                case ArmorType.Hide:
                    Protection = 25;
                    break;
                case ArmorType.ChainMail:
                    Protection = 50;
                    break;
                case ArmorType.LightIron:
                    Protection = 75;
                    break;
                case ArmorType.MediumIron:
                    Protection = 100;
                    break;
                case ArmorType.HeavyIron:
                    Protection = 150;
                    break;
                case ArmorType.Gold:
                    Protection = 200;
                    break;
                case ArmorType.Fur:
                    Protection = 25;
                    break;
                case ArmorType.Scale:
                    Protection = 125;
                    break;
                case ArmorType.Shield:
                    Protection = 50;
                    break;
            }
        }

        public void ArmorWeight()
        {
            switch (ArmorType)
            {
                case ArmorType.Rags:
                    base.Weight = 2;
                    break;
                case ArmorType.Hide:
                    base.Weight = 5;
                    break;
                case ArmorType.ChainMail:
                    base.Weight = 10;
                    break;
                case ArmorType.LightIron:
                    base.Weight = 10;
                    break;
                case ArmorType.MediumIron:
                    base.Weight = 20;
                    break;
                case ArmorType.HeavyIron:
                    base.Weight = 35;
                    break;
                case ArmorType.Gold:
                    base.Weight = 40;
                    break;
                case ArmorType.Fur:
                    base.Weight = 8;
                    break;
                case ArmorType.Scale:
                    base.Weight = 25;
                    break;
                case ArmorType.Shield:
                    base.Weight = 45;
                    break;
            }
        }
    }
}
    
