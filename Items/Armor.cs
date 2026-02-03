using RPGFramework.Enums;
namespace RPGFramework
{
    internal class Armor : Item
    {
        public int AC { get; set; } = 0;
        public int Durability { get; set; } = 20;
        public bool Equipped { get; set; } = false;
        public ArmorType ArmorType { get; set; }
        public int ACBonus { get; set; } = 0;
        public bool HasDex { get; set; } = false;
        public int DexMax { get; set; } = 10;
        // COMBAT AND CORE TEAM: Armor Durability and Protection did not fit with how combat was run, and weight was unrealistic
        // COMBAT AND CORE TEAM: additinally there was some armor that when looking at the material properties did not make sense having the protection score they had as well as weight
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
                    AC = 10;
                    HasDex = true;
                    DexMax = 10;
                    break;
                case ArmorType.Hide:
                    AC = 12;
                    HasDex = true;
                    DexMax = 2;
                    break;
                case ArmorType.ChainMail:
                    AC = 16;
                    HasDex = false;
                    DexMax = 0;
                    break;
                case ArmorType.LightIron:
                    AC = 14;
                    HasDex = true;
                    DexMax = 2;
                    break;
                case ArmorType.MediumIron:
                    AC = 15;
                    HasDex = true;
                    DexMax = 2;
                    break;
                case ArmorType.HeavyIron:
                    AC = 18;
                    HasDex = false;
                    DexMax = 0;
                    break;
                case ArmorType.Gold:
                    AC = 8;
                    HasDex = false;
                    DexMax = 0;
                    // COMBAT AND CORE TEAM: gold can be bent with your hands, its not stopping a sword
                    break;
                case ArmorType.Fur:
                    AC = 11;
                    HasDex = true;
                    DexMax = 10;
                    break;
                case ArmorType.Scale:
                    AC = 14;
                    HasDex = true;
                    DexMax = 2;
                    break;
                case ArmorType.Shield:
                    ACBonus = 5;
                    break;
            }
        }

        public void ArmorWeight()
        {
            // COMBAT AND CORE TEAM: weight is now in pounds and based on either DnD or some other game
            switch (ArmorType)
            {
                case ArmorType.Rags:
                    base.Weight = 2;
                    break;
                case ArmorType.Hide:
                    Weight = 12;
                    break;
                case ArmorType.ChainMail:
                    Weight = 55;
                    break;
                case ArmorType.LightIron:
                    Weight = 20;
                    break;
                case ArmorType.MediumIron:
                    Weight = 40;
                    break;
                case ArmorType.HeavyIron:
                    Weight = 65;
                    break;
                case ArmorType.Gold:
                    Weight = 150;
                    break;
                case ArmorType.Fur:
                    Weight = 25;
                    break;
                case ArmorType.Scale:
                    Weight = 45;
                    break;
                case ArmorType.Shield:
                    Weight = 6;
                    break;
            }
        }
    }
}
    
