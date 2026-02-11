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
    }


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
        }
}