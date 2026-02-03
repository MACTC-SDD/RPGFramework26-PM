using RPGFramework.Enums;
using Spectre.Console;

namespace RPGFramework
{
    internal class Weapon : Item
    {
        public int Damage { get; set; } = 0;
        public WeaponType WeaponType { get; set; }
        public int Durability { get; set; } = 0;
        public bool ammmoleft { get; set; } = true;
        public int Range { get; set; } = 0;
        public double Speed { get; set; } = 0;
        public double weight { get; set; } = 0;
        // TODO
        // Add attack properties (damage, speed, etc.)
        // Implement attack methods
        // Maybe some kind of Weapon generator (random stats, etc.)


        public void WeaponValue()
        {
            switch(this.WeaponType)
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
                    Value = 5000;
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

        public void WeaponDamage()
        {
            switch (this.WeaponType)
            {
                case WeaponType.Bow:
                    Damage = 50;
                    break;
                case WeaponType.Flail:
                    Damage = 30;
                    break;
                case WeaponType.Hands:
                    Damage = 5;
                    break;
                case WeaponType.Crossbow:
                    Damage = 65;
                    break;
                case WeaponType.Knife:
                    Damage = 25;
                    break;
                case WeaponType.LongSword:
                    Damage = 50;
                    break;
                case WeaponType.Sword:
                    Damage = 30;
                    break;
                case WeaponType.Mace:
                    Damage = 35;
                    break;
                case WeaponType.Musket:
                    Damage = 250;
                    break;
                case WeaponType.Rock:
                    Damage = 10;
                    break;
                case WeaponType.ShortSword:
                    Damage = 30;
                    break;
                case WeaponType.Spear:
                    Damage = 40;
                    break;
                case WeaponType.WarAxe:
                    Damage = 50;
                    break;
                case WeaponType.GlassBottle:
                    Damage = 15;
                    break;
                case WeaponType.FireStaff:
                    Damage = 100;
                    break;
                case WeaponType.LightStaff:
                    Damage = 100;
                    break;
                case WeaponType.IceStaff:
                    Damage = 100;
                    break;
                case WeaponType.DarkStaff:
                    Damage = 100;
                    break;
                case WeaponType.AirStaff:
                    Damage = 100;
                    break;
                case WeaponType.EarthStaff:
                    Damage = 100;
                    break;
            }
        }

        public void WeaponDurability()
        {
            switch (this.WeaponType)
            {
                case WeaponType.Spear:
                    Durability = 50;
                    break;
                case WeaponType.Bow:
                    Durability = 30;
                    break;
                case WeaponType.Hands:
                    Durability = 100;
                    break;
                case WeaponType.Musket:
                    Durability = 10;
                    break;
                case WeaponType.Knife:
                    Durability = 25;
                    break;
                case WeaponType.LongSword:
                    Durability = 50;
                    break;
                case WeaponType.ShortSword:
                    Durability = 40;
                    break;
                case WeaponType.Mace:
                    Durability = 40;
                    break;
                case WeaponType.Sword:
                    Durability = 40;
                    break;
                case WeaponType.Rock:
                    Durability = 10;
                    break;
                case WeaponType.WarAxe:
                    Durability = 50;
                    break;
                case WeaponType.GlassBottle:
                    Durability = 5;
                    break;
                case WeaponType.FireStaff:
                    Durability = 15;
                    break;
                case WeaponType.LightStaff:
                    Durability = 15;
                    break;
                case WeaponType.IceStaff:
                    Durability = 15;
                    break;
                case WeaponType.DarkStaff:
                    Durability = 15;
                    break;
                case WeaponType.AirStaff:
                    Durability = 15;
                    break;
                case WeaponType.EarthStaff:
                    Durability = 15;
                    break;
            }
        }

        public void WeaponRange()
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
        }


        public void SetWeaponsSpeed()
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
        }


        public void WeaponWeight()
        {
            switch (this.WeaponType)
            {
                case WeaponType.Hands:
                    Weight = 0;
                    break;
                case WeaponType.Bow:
                    Weight = 5;
                    break;
                case WeaponType.Sword:
                    Weight = 5;
                    break;
                case WeaponType.Musket:
                    Weight = 25;
                    break;
                case WeaponType.Crossbow:
                    Weight = 15;
                    break;
                case WeaponType.Flail:
                    Weight = 8;
                    break;
                case WeaponType.Knife:
                    Weight = 2;
                    break;
                case WeaponType.LongSword:
                    Weight = 10;
                    break;
                case WeaponType.Mace:
                    Weight = 6;
                    break;
                case WeaponType.Rock:
                    Weight = 1;
                    break;
                case WeaponType.ShortSword:
                    Weight = 1;
                    break;
                case WeaponType.Spear:
                    Weight = 8;
                    break;
                case WeaponType.WarAxe:
                    Weight = 10;
                    break;
                case WeaponType.GlassBottle:
                    Weight = 1;
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
    }
}



