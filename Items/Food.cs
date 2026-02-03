using System;
using RPGFramework.Enums;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Items
{
    internal class Food : Consumable
    {
        public int HealAmount { get; set; } = 0;
        public int StackAmount { get; set; } = 1;
        public int StackMax { get; set; } = 10;

        public FoodType FoodType { get; set; } = 0;

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

                case FoodType.Chicken_Leg:
                    Value = 10;
                    break;

                case FoodType.Wolf_Meat:
                    Value = 15;
                    break;

                case FoodType.Boar_Meat:
                    Value = 15;
                    break;

                case FoodType.Pie:
                    Value = 30;
                    break;

                case FoodType.Berries:
                    Value = 2;
                    break;

                case FoodType.Water_Bottle:
                    Value = 3;
                    break;

                case FoodType.Milk_Bottle:
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
                    HealAmount = 45;
                    break;

                case FoodType.Stew:
                    HealAmount = 100;
                    break;

                case FoodType.Chicken_Leg:
                    HealAmount = 30;
                    break;

                case FoodType.Wolf_Meat:
                    HealAmount = 25;
                    break;

                case FoodType.Boar_Meat:
                    HealAmount = 15;
                    break;

                case FoodType.Pie:
                    HealAmount = 45;
                    break;

                case FoodType.Berries:
                    HealAmount = 5;
                    break;

                case FoodType.Water_Bottle:
                    HealAmount = 25;
                    break;

                case FoodType.Milk_Bottle:
                    HealAmount = 30;
                    break;

                case FoodType.Wine:
                    HealAmount = 1;
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

                case FoodType.Chicken_Leg:
                    StackAmount = 10;
                    break;

                case FoodType.Wolf_Meat:
                    StackAmount = 10;
                    break;

                case FoodType.Boar_Meat:
                    StackAmount = 10;
                    break;

                case FoodType.Pie:
                    StackAmount = 5;
                    break;

                case FoodType.Berries:
                    StackAmount = 100;
                    break;

                case FoodType.Water_Bottle:
                    StackAmount = 5;
                    break;

                case FoodType.Milk_Bottle:
                    StackAmount = 5;
                    break;

                case FoodType.Wine:
                    StackAmount = 10;
                    break;
            }
        }
    }
}
