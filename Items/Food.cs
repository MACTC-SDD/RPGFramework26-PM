using System;
using RPGFramework.Enums;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework
{
    internal class Food : Consumable
    {
        public int StackAmount { get; set; } = 1;
        public int StackMax { get; set; } = 10;

        public FoodType FoodType { get; set; } = 0;
        public void Use(Character user)
        {
            user.Heal(HealAmount);
        }

        public void FoodValue()
        {
            switch (FoodType)
            {
                case FoodType.Apple:
                    Value = 5;
                    break;

                case FoodType.Bread:
                    Value = 3;
                    break;

                case FoodType.Cheesewheel:
                    Value = 15;
                    break;

                case FoodType.Stew:
                    Value = 30;
                    break;

                case FoodType.ChickenLeg:
                    Value = 10;
                    break;

                case FoodType.WolfMeat:
                    Value = 15;
                    break;

                case FoodType.BoarMeat:
                    Value = 15;
                    break;

                case FoodType.Pie:
                    Value = 30;
                    break;

                case FoodType.Berries:
                    Value = 2;
                    break;

                case FoodType.WaterBottle:
                    Value = 3;
                    break;

                case FoodType.MilkBottle:
                    Value = 5;
                    break;

                case FoodType.Wine:
                    Value = 10;
                    break;
            }
        }


        public void FoodHeal()
        {
            switch (FoodType)
            {
                case FoodType.Apple:
                    HealAmount = 10;
                    break;

                case FoodType.Bread:
                    HealAmount = 20;
                    break;

                case FoodType.Cheesewheel:
                    HealAmount = 20;
                    break;

                case FoodType.Stew:
                    HealAmount = 50;
                    break;

                case FoodType.ChickenLeg:
                    HealAmount = 25;
                    break;

                case FoodType.WolfMeat:
                    HealAmount = 25;
                    break;

                case FoodType.BoarMeat:
                    HealAmount = 25;
                    break;

                case FoodType.Pie:
                    HealAmount = 30;
                    break;

                case FoodType.Berries:
                    HealAmount = 5;
                    break;

                case FoodType.WaterBottle:
                    HealAmount = 15;
                    break;

                case FoodType.MilkBottle:
                    HealAmount = 15;
                    break;

                case FoodType.Wine:
                    HealAmount = 10;
                    break;
            }
        }

        public void FoodStack()
        {
            switch (this.FoodType)
            {
                case FoodType.Apple:
                    StackAmount = 10;
                    break;

                case FoodType.Bread:
                    StackAmount = 5;
                    break;

                case FoodType.Cheesewheel:
                    StackAmount = 2;
                    break;

                case FoodType.Stew:
                    StackAmount = 1;
                    break;

                case FoodType.ChickenLeg:
                    StackAmount = 10;
                    break;

                case FoodType.WolfMeat:
                    StackAmount = 10;
                    break;

                case FoodType.BoarMeat:
                    StackAmount = 10;
                    break;

                case FoodType.Pie:
                    StackAmount = 5;
                    break;

                case FoodType.Berries:
                    StackAmount = 100;
                    break;

                case FoodType.WaterBottle:
                    StackAmount = 5;
                    break;

                case FoodType.MilkBottle:
                    StackAmount = 5;
                    break;

                case FoodType.Wine:
                    StackAmount = 10;
                    break;
            }
        }
    }
}
