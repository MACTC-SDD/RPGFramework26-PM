using RPGFramework.Enums;
using Spectre.Console;

namespace RPGFramework
{
    internal class Weapon : Item
    {
        public int Damage { get; set; } = 0;
        public WeaponType WeaponType { get; set; }
        public int Durability { get; set; } = 0;
        public int CurrentDurability { get; set; }
        public bool AmmoLeft { get; set; } = true;
        public int Range { get; set; } = 0;
        public double Speed { get; set; } = 0;
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
                    Range = 2;
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
                    Range = 10;
                    break;
                case WeaponType.Spear:
                    Range = 15;
                    break;
                case WeaponType.WarAxe:
                    Range = 5;
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
                    base.Weight = 0;
                    break;
                case WeaponType.Bow:
                    base.Weight = 5;
                    break;
                case WeaponType.Sword:
                    base.Weight = 5;
                    break;
                case WeaponType.Musket:
                    base.Weight = 25;
                    break;
                case WeaponType.Crossbow:
                    base.Weight = 15;
                    break;
                case WeaponType.Flail:
                    base.Weight = 8;
                    break;
                case WeaponType.Knife:
                    base.Weight = 2;
                    break;
                case WeaponType.LongSword:
                    base.Weight = 10;
                    break;
                case WeaponType.Mace:
                    base.Weight = 6;
                    break;
                case WeaponType.Rock:
                    base.Weight = 1;
                    break;
                case WeaponType.ShortSword:
                    base.Weight = 1;
                    break;
                case WeaponType.Spear:
                    base.Weight = 8;
                    break;
                case WeaponType.WarAxe:
                    base.Weight = 10;
                    break;
            }
        }
    }
}



