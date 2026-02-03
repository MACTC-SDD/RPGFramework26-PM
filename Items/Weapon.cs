using RPGFramework.Enums;
using Spectre.Console;

namespace RPGFramework
{
    internal class Weapon : Item
    {
        public int MaxDamage { get; set; } = 0;
        public int MaxDice { get; set; } = 0;
        public WeaponType WeaponType { get; set; }
        public int Durability { get; set; } = 0;
        public bool ammmoleft { get; set; } = true;
        public bool range { get; set; } = false;
        public double Speed { get; set; } = 0;
        public double weight { get; set; } = 0;
        // TODO
        // Add attack properties (damage, speed, etc.)
        // COMBAT AND CORE TEAM: speed is not needed or important in any way
        // Implement attack methods
        // Maybe some kind of Weapon generator (random stats, etc.)
        // COMBAT AND CORE TEAM: already done

        //ADD THE STAFFS AND GLASS BOTTLES
        // COMBAT AND CORE TEAM: glass bottles should do a max of 4 with a max dice of 1
        // COMBAT AND CORE TEAM: staff should do a max of 6 with a max dice of 1


        public void WeaponDamage()
        {
            switch (this.WeaponType)
            {
                case WeaponType.Bow:
                    MaxDamage = 6;
                    MaxDice = 1;
                    range = true;
                    break;
                case WeaponType.Flail:
                    MaxDamage = 8;
                    MaxDice = 1;
                    break;
                case WeaponType.Hands:
                    MaxDamage = 1;
                    MaxDice = 1;
                    break;
                case WeaponType.Crossbow:
                    MaxDamage = 8;
                    MaxDice = 1;
                    range = true;
                    break;
                case WeaponType.Knife:
                    MaxDamage = 4;
                    MaxDice = 1;
                    break;
                case WeaponType.LongSword:
                    MaxDamage = 8;
                    MaxDice = 1;
                    break;
                case WeaponType.Sword:
                    MaxDamage = 8;
                    MaxDice = 2;
                    break;
                case WeaponType.Mace:
                    MaxDamage = 6;
                    MaxDice = 1;
                    break;
                case WeaponType.Musket:
                    MaxDamage = 12;
                    MaxDice = 1;
                    range = true;
                    break;
                case WeaponType.Rock:
                    MaxDamage = 4;
                    MaxDice = 1;
                    break;
                case WeaponType.ShortSword:
                    MaxDamage = 6;
                    MaxDice = 1;
                    break;
                case WeaponType.Spear:
                    MaxDamage = 6;
                    MaxDice = 1;
                    break;
                case WeaponType.WarAxe:
                    MaxDamage = 12;
                    MaxDice = 1;
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

        /*public void WeaponRange()
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
        }*/
        // COMBAT AND CORE TEAM: range should just be a bool to see if it can add strenght to damage


        /*public void SetWeaponsSpeed()
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
        } */

        // COMBAT AND CORE TEAM: weight is once again changed to pounds for consistancy
        // COMBAT AND CORE TEAM: and to make it make sense to both the players and anyone coding
        public void WeaponWeight()
        {
            switch (this.WeaponType)
            {
                case WeaponType.Hands:
                    Weight = 0;
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
            }
        }
        public int RollDamage()
        {
            int totalDamage = 0;
    
    for(int i = 0; i < this.MaxDice; i++)
	{
                Random rand = new Random();
                int roll = rand.Next(1, this.MaxDamage);
                totalDamage += roll;
            }
            return totalDamage;
        }

    }
}



