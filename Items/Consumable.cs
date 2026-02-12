using RPGFramework.Enums;

namespace RPGFramework
{
    internal class Consumable : Item
    {
        public int UsesLeft { get; set; } = -1; // -1 means unlimited uses
        public int HealAmount { get; set; } = 0;
        public int DuraRestore { get; set; } = 5;
        public ConsumType ConsumType { get; set; }
        public int StrengthAmount { get; set; } = 0;
        public int ManaAmount { get; set; } = 0;
        public int HealthAmount { get; set; } = 0;
        public int GoldAmount { get; set; } = 0;
        public int Value { get; set; } = 0;


        public void ConsumableHooks()
        {
            switch (ConsumType)
            {
                case ConsumType.PrimitiveRepairTool:
                    DuraRestore = 2;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.RamshackleRepairTool:
                    DuraRestore = 5;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.ApprenticeRepairTool:
                    DuraRestore = 10;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.JourneymanRepairTool:
                    DuraRestore = 15;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.MastercraftRepairTool:
                    DuraRestore = 25;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.AscendantRepairTool:
                    DuraRestore = 40;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.CommonManaPotion:
                    ManaAmount = 10;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.UncommonManaPotion:
                    ManaAmount = 30;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.RareManaPotion:
                    ManaAmount = 40;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.EpicManaPotion:
                    ManaAmount = 50;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.LegendaryManaPotion:
                    ManaAmount = 60;
                        break;
            }
            switch (ConsumType)
            {
                case ConsumType.GodlyManaPotion:
                    ManaAmount = 335;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.CommonHealthPotion:
                    HealthAmount = 50;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.UncommonHealthPotion:
                    HealthAmount = 100;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.RareHealthPotion:
                    HealthAmount = 150;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.EpicHealthPotion:
                    HealthAmount = 200;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.LegendaryHealthPotion:
                    HealthAmount = 250;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.GodlyHealthPotion:
                    HealthAmount = 600;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.CommonCoinPouch:
                    GoldAmount = 15;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.UncommonCoinPouch:
                    GoldAmount = 30;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.RareCoinPouch:
                    GoldAmount = 40;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.EpicCoinPouch:
                    GoldAmount = 50;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.LegendaryCoinPouch:
                    GoldAmount = 75;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.GodlyCoinPouch:
                    GoldAmount = 100;
                    break;
            }
        }
        public void Durability()
        {
            switch (ConsumType)
            {
                case ConsumType.PrimitiveRepairTool:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.RamshackleRepairTool:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.ApprenticeRepairTool:
                    UsesLeft = 2;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.JourneymanRepairTool:
                    UsesLeft = 3;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.MastercraftRepairTool:
                    UsesLeft = 4;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.AscendantRepairTool:
                    UsesLeft = 5;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.CommonManaPotion:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.UncommonManaPotion:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.RareManaPotion:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.EpicManaPotion:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.LegendaryManaPotion:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.GodlyManaPotion:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.CommonHealthPotion:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.UncommonHealthPotion:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.RareHealthPotion:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.EpicHealthPotion:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.LegendaryHealthPotion:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.GodlyHealthPotion:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.CommonCoinPouch:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.UncommonCoinPouch:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.RareCoinPouch:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.EpicCoinPouch:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.LegendaryCoinPouch:
                    UsesLeft = 1;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.GodlyCoinPouch:
                    UsesLeft = 1;
                    break;
            }
        }

        public void ConsumValue()
        {
            switch (ConsumType)
            {
                case ConsumType.PrimitiveRepairTool:
                    Value = 5;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.RamshackleRepairTool:
                    Value = 15;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.ApprenticeRepairTool:
                    Value = 25;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.JourneymanRepairTool:
                    Value = 30;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.MastercraftRepairTool:
                    Value = 45;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.AscendantRepairTool:
                    Value = 100;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.CommonManaPotion:
                    Value = 10;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.UncommonManaPotion:
                    Value = 40;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.RareManaPotion:
                    Value = 70;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.EpicManaPotion:
                    Value = 100;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.LegendaryManaPotion:
                    Value = 130;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.GodlyManaPotion:
                    Value = 160;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.CommonHealthPotion:
                    Value = 10;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.UncommonHealthPotion:
                    Value = 40;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.RareHealthPotion:
                    Value = 70;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.EpicHealthPotion:
                    Value = 70;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.LegendaryHealthPotion:
                    Value = 100;
                    break;
            }
            switch (ConsumType)
            {
                case ConsumType.GodlyHealthPotion:
                    Value = 550;
                    break;
            }
        }
    }
}