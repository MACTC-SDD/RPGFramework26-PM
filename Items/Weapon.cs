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
        public int range { get; set; } = 0;
        public double Speed { get; set; } = 0;
        public double weight { get; set; } = 0;
        // TODO
        // Add attack properties (damage, speed, etc.)
        // Implement attack methods
        // Maybe some kind of Weapon generator (random stats, etc.)

        //ADD THE STAFFS AND GLASS BOTTLES


        public void WeaponDamage()
        {
            switch (this.WeaponType)
            {
                case WeaponType.Bow:
                    Damage = 30;
                    break;
                case WeaponType.Flail:
                    Damage = 25;
                    break;
                case WeaponType.Hands:
                    Damage = 10;
                    break;
                case WeaponType.Crossbow:
                    Damage = 35;
                    break;
                case WeaponType.Knife:
                    Damage = 20;
                    break;
                case WeaponType.LongSword:
                    Damage = 35;
                    break;
                case WeaponType.Sword:
                    Damage = 30;
                    break;
                case WeaponType.Mace:
                    Damage = 20;
                    break;
                case WeaponType.Musket:
                    Damage = 50;
                    break;
                case WeaponType.Rock:
                    Damage = 15;
                    break;
                case WeaponType.ShortSword:
                    Damage = 20;
                    break;
                case WeaponType.Spear:
                    Damage = 25;
                    break;
                case WeaponType.WarAxe:
                    Damage = 35;
                    break;
            }
        }

        public void WeaponDurability()
        {
            switch (this.WeaponType)
            {
                case WeaponType.Spear:
                    Durability = 25;
                    break;
                case WeaponType.Bow:
                    Durability = 25;
                    break;
                case WeaponType.Hands:
                    Durability = 100;
                    break;
                case WeaponType.Musket:
                    Durability = 10;
                    break;
                case WeaponType.Knife:
                    Durability = 15;
                    break;
                case WeaponType.LongSword:
                    Durability = 25;
                    break;
                case WeaponType.ShortSword:
                    Durability = 25;
                    break;
                case WeaponType.Mace:
                    Durability = 25;
                    break;
                case WeaponType.Sword:
                    Durability = 25;
                    break;
                case WeaponType.Rock:
                    Durability = 35;
                    break;
                case WeaponType.WarAxe:
                    Durability = 25;
                    break;
            }
        }

        public void WeaponRange()
        {
            switch (this.WeaponType)
            {
                case WeaponType.Hands:
                    range = 2;
                    break;
                case WeaponType.Bow:
                    range = 15;
                    break;
                case WeaponType.Crossbow:
                    range = 20;
                    break;
                case WeaponType.Flail:
                    range = 5;
                    break;
                case WeaponType.Knife:
                    range = 2;
                    break;
                case WeaponType.LongSword:
                    range = 7;
                    break;
                case WeaponType.ShortSword:
                    range = 4;
                    break;
                case WeaponType.Sword:
                    range = 5;
                    break;
                case WeaponType.Mace:
                    range = 4;
                    break;
                case WeaponType.Musket:
                    range = 35;
                    break;
                case WeaponType.Rock:
                    range = 10;
                    break;
                case WeaponType.Spear:
                    range = 15;
                    break;
                case WeaponType.WarAxe:
                    range = 5;
                    break;
            }
        }


        public void SetWeaponsSpeed()
        {
            switch (this.WeaponType)
            {
                case WeaponType.Hands:
                    Speed = 1;
                    break;
                case WeaponType.Bow:
                    Speed = 1;
                    break;
                case WeaponType.Sword:
                    Speed = 1;
                    break;
                case WeaponType.Musket:
                    Speed = 1;
                    break;
                case WeaponType.Crossbow:
                    Speed = 1;
                    break;
                case WeaponType.Flail:
                    Speed = 1;
                    break;
                case WeaponType.Knife:
                    Speed = 1;
                    break;
                case WeaponType.LongSword:
                    Speed = 1;
                    break;
                case WeaponType.Mace:
                    Speed = 1;
                    break;
                case WeaponType.Rock:
                    Speed = 1;
                    break;
                case WeaponType.ShortSword:
                    Speed = 1;
                    break;
                case WeaponType.Spear:
                    Speed = 1;
                    break;
                case WeaponType.WarAxe:
                    Speed = 1;
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
            }
        }
    }
}



