using RPGFramework.Enums;
using Spectre.Console;

namespace RPGFramework
{
    using RPGFramework.Enums;
    using Spectre.Console;
    using static System.Net.Mime.MediaTypeNames;
    internal class Weapon : Item
    {
        public int MaxDamage { get; set; } = 0;
        public int MaxDice { get; set; } = 0;
        public WeaponType WeaponType { get; set; }
        public int Durability { get; set; } = 0;
        public int CurrentDurability { get; set; }
        public bool ammmoleft { get; set; } = true;
        public bool range { get; set; } = false;
        public double Speed { get; set; } = 0;
        public double Weight { get; set; } = 0;
        public double Value { get; set; } = 0;
        // TODO
        // Add attack properties (damage, speed, etc.)
        // COMBAT AND CORE TEAM: speed is not needed or important in any way
        // Implement attack methods
        // Maybe some kind of Weapon generator (random stats, etc.)


        public void WeaponDamage()
        {
            switch (this.WeaponType)
            {
                case WeaponType.Bow:
                    MaxDamage = 12;
                    MaxDice = 2;
                    range = true;
                    break;
                case WeaponType.Flail:
                    MaxDamage = 8;
                    MaxDice = 2;
                    break;
                case WeaponType.Hands:
                    MaxDamage = 1;
                    MaxDice = 2;
                    break;
                case WeaponType.Crossbow:
                    MaxDamage = 6;
                    MaxDice = 2;
                    range = true;
                    break;
                case WeaponType.Knife:
                    MaxDamage = 8;
                    MaxDice = 1;
                    break;
                case WeaponType.LongSword:
                    MaxDamage = 12;
                    MaxDice = 2;
                    break;
                case WeaponType.Sword:
                    MaxDamage = 8;
                    MaxDice = 2;
                    break;
                case WeaponType.Mace:
                    MaxDamage = 8;
                    MaxDice = 2;
                    break;
                case WeaponType.Musket:
                    MaxDamage = 20;
                    MaxDice = 2;
                    range = true;
                    break;
                case WeaponType.Rock:
                    MaxDamage = 6;
                    MaxDice = 1;
                    break;
                case WeaponType.ShortSword:
                    MaxDamage = 8;
                    MaxDice = 2;
                    break;
                case WeaponType.Spear:
                    MaxDamage = 8;
                    MaxDice = 2;
                    break;
                case WeaponType.WarAxe:
                    MaxDamage = 12;
                    MaxDice = 3;
                    break;
                case WeaponType.GlassBottle:
                    MaxDamage = 6;
                    MaxDice = 1;
                    break;
                case WeaponType.FireStaff:
                    MaxDamage = 8;
                    MaxDice = 3;
                    break;
                case WeaponType.LightStaff:
                    MaxDamage = 8;
                    MaxDice = 3;
                    break;
                case WeaponType.IceStaff:
                    MaxDamage = 8;
                    MaxDice = 3;
                    break;
                case WeaponType.DarkStaff:
                    MaxDamage = 8;
                    MaxDice = 3;
                    break;
                case WeaponType.AirStaff:
                    MaxDamage = 8;
                    MaxDice = 3;
                    break;
                case WeaponType.EarthStaff:
                    MaxDamage = 8;
                    MaxDice = 3;
                    break;
            }
        }


        // COMBAT AND CORE TEAM: weight is once again changed to pounds for consistancy
        // COMBAT AND CORE TEAM: and to make it make sense to both the players and anyone coding
        public void WeaponWeight()
        {
            switch (this.WeaponType)
            {
                case WeaponType.Hands:
                    base.Weight = 0;
                    break;
                case WeaponType.Bow:
                    Weight = 2;
                    break;
                case WeaponType.Sword:
                    Weight = 6;
                    break;
                case WeaponType.Musket:
                    Weight = 10;
                    break;
                case WeaponType.Crossbow:
                    Weight = 5;
                    break;
                case WeaponType.Flail:
                    Weight = 2;
                    break;
                case WeaponType.Knife:
                    Weight = 1;
                    break;
                case WeaponType.LongSword:
                    Weight = 3;
                    break;
                case WeaponType.Mace:
                    Weight = 4;
                    break;
                case WeaponType.Rock:
                    Weight = 10;
                    break;
                case WeaponType.ShortSword:
                    Weight = 2;
                    break;
                case WeaponType.Spear:
                    Weight = 3;
                    break;
                case WeaponType.WarAxe:
                    Weight = 7;
                    break;
                case WeaponType.GlassBottle:
                    Weight = 2;
                    break;
                case WeaponType.FireStaff:
                    Weight = 25;
                    break;
                case WeaponType.LightStaff:
                    Weight = 25;
                    break;
                case WeaponType.IceStaff:
                    Weight = 25;
                    break;
                case WeaponType.DarkStaff:
                    Weight = 25;
                    break;
                case WeaponType.AirStaff:
                    Weight = 25;
                    break;
                case WeaponType.EarthStaff:
                    Weight = 25;
                    break;
            }
        }
        public int RollDamage()
        {
            int totalDamage = 0;

            for (int i = 0; i < this.MaxDice; i++)
            {
                Random rand = new Random();
                int roll = rand.Next(1, this.MaxDamage);
                totalDamage += roll;
            }
            return totalDamage;
        }







        // TODO
        // Add attack properties (damage, speed, etc.)
        // Implement attack methods
        // Maybe some kind of Weapon generator (random stats, etc.)


        public void WeaponValue()
        {
            switch (this.WeaponType)
            {
                case WeaponType.Bow:
                    Value = 30;
                    break;
                case WeaponType.Flail:
                    Value = 30;
                    break;
                case WeaponType.Crossbow:
                    Value = 200;
                    break;
                case WeaponType.Knife:
                    Value = 10;
                    break;
                case WeaponType.LongSword:
                    Value = 90;
                    break;
                case WeaponType.Sword:
                    Value = 45;
                    break;
                case WeaponType.Mace:
                    Value = 65;
                    break;
                case WeaponType.Musket:
                    Value = 500;
                    break;
                case WeaponType.Rock:
                    Value = 1;
                    break;
                case WeaponType.ShortSword:
                    Value = 25;
                    break;
                case WeaponType.Spear:
                    Value = 75;
                    break;
                case WeaponType.WarAxe:
                    Value = 300;
                    break;
                case WeaponType.GlassBottle:
                    Value = 5;
                    break;
                case WeaponType.FireStaff:
                    Value = 500;
                    break;
                case WeaponType.LightStaff:
                    Value = 500;
                    break;
                case WeaponType.IceStaff:
                    Value = 500;
                    break;
                case WeaponType.DarkStaff:
                    Value = 500;
                    break;
                case WeaponType.AirStaff:
                    Value = 500;
                    break;
                case WeaponType.EarthStaff:
                    Value = 500;
                    break;


            }
        }


        public void WeaponDurability()
        {
            switch (this.WeaponType)
            {
                case WeaponType.Spear:
                    Durability = 50;
                    CurrentDurability = Durability;
                    break;
                case WeaponType.Bow:
                    Durability = 30;
                    CurrentDurability = Durability;
                    break;
                case WeaponType.Hands:
                    Durability = 100;
                    CurrentDurability = Durability;
                    break;
                case WeaponType.Musket:
                    Durability = 10;
                    CurrentDurability = Durability;
                    break;
                case WeaponType.Knife:
                    Durability = 25;
                    CurrentDurability = Durability;
                    break;
                case WeaponType.LongSword:
                    Durability = 50;
                    CurrentDurability = Durability;
                    break;
                case WeaponType.ShortSword:
                    Durability = 40;
                    CurrentDurability = Durability;
                    break;
                case WeaponType.Mace:
                    Durability = 40;
                    CurrentDurability = Durability;
                    break;
                case WeaponType.Sword:
                    Durability = 40;
                    CurrentDurability = Durability;
                    break;
                case WeaponType.Rock:
                    Durability = 10;
                    CurrentDurability = Durability;
                    break;
                case WeaponType.WarAxe:
                    Durability = 50;
                    CurrentDurability = Durability;
                    break;
                case WeaponType.GlassBottle:
                    Durability = 5;
                    CurrentDurability = Durability;
                    break;
                case WeaponType.FireStaff:
                    Durability = 15;
                    CurrentDurability = Durability;
                    break;
                case WeaponType.LightStaff:
                    Durability = 15;
                    CurrentDurability = Durability;
                    break;
                case WeaponType.IceStaff:
                    Durability = 15;
                    CurrentDurability = Durability;
                    break;
                case WeaponType.DarkStaff:
                    Durability = 15;
                    CurrentDurability = Durability;
                    break;
                case WeaponType.AirStaff:
                    Durability = 15;
                    CurrentDurability = Durability;
                    break;
                case WeaponType.EarthStaff:
                    Durability = 15;
                    CurrentDurability = Durability;
                    break;
            }
        }

        /*public void WeaponRange()
        {
            switch (this.WeaponType)
            {
                case WeaponType.Hands:
                    Range = 1;
                    break;
                case WeaponType.Bow:
                    Range = 15;
                    break;
                case WeaponType.Crossbow:
                    Range = 20;
                    break;
                case WeaponType.Flail:
                    Range = 5;
                    break;
                case WeaponType.Knife:
                    Range = 2;
                    break;
                case WeaponType.LongSword:
                    Range = 7;
                    break;
                case WeaponType.ShortSword:
                    Range = 4;
                    break;
                case WeaponType.Sword:
                    Range = 5;
                    break;
                case WeaponType.Mace:
                    Range = 4;
                    break;
                case WeaponType.Musket:
                    Range = 35;
                    break;
                case WeaponType.Rock:
                    Range = 2;
                    break;
                case WeaponType.Spear:
                    Range = 10;
                    break;
                case WeaponType.WarAxe:
                    Range = 5;
                    break;
                case WeaponType.GlassBottle:
                    Range = 2;
                    break;
                case WeaponType.FireStaff:
                    Range = 20;
                    break;
                case WeaponType.LightStaff:
                    Range = 20;
                    break;
                case WeaponType.IceStaff:
                    Range = 20;
                    break;
                case WeaponType.DarkStaff:
                    Range = 20;
                    break;
                case WeaponType.AirStaff:
                    Range = 20;
                    break;
                case WeaponType.EarthStaff:
                    Range = 20;
                    break;
            }
        } */
        //Range does not affect combat in the slightest, instead range weapons reacived a damage bonus


        /* public void SetWeaponsSpeed()
         {
             switch (this.WeaponType)
             {
                 case WeaponType.Hands:
                     Speed = 3;
                     break;
                 case WeaponType.Bow:
                     Speed = 30;
                     break;
                 case WeaponType.Sword:
                     Speed = 2;
                     break;
                 case WeaponType.Musket:
                     Speed = 100;
                     break;
                 case WeaponType.Crossbow:
                     Speed = 50;
                     break;
                 case WeaponType.Flail:
                     Speed = 3;
                     break;
                 case WeaponType.Knife:
                     Speed = 5;
                     break;
                 case WeaponType.LongSword:
                     Speed = 1;
                     break;
                 case WeaponType.Mace:
                     Speed = 2;
                     break;
                 case WeaponType.Rock:
                     Speed = 2;
                     break;
                 case WeaponType.ShortSword:
                     Speed = 3;
                     break;
                 case WeaponType.Spear:
                     Speed = 1;
                     break;
                 case WeaponType.WarAxe:
                     Speed = 1;
                     break;
                 case WeaponType.GlassBottle:
                     Speed = 3;
                     break;
                 case WeaponType.FireStaff:
                     Speed = 2;
                     break;
                 case WeaponType.LightStaff:
                     Speed = 2;
                     break;
                 case WeaponType.IceStaff:
                     Speed = 2;
                     break;
                 case WeaponType.DarkStaff:
                     Speed = 2;
                     break;
                 case WeaponType.AirStaff:
                     Speed = 2;
                     break;
                 case WeaponType.EarthStaff:
                     Speed = 2;
                     break;
             }
         } */
        // Speed has no effect on combat and instead it is assumed that through the dice system



    }
}



