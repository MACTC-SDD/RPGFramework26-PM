using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;
using RPGFramework.Enums;

namespace RPGFramework
{
    internal class MonsterWeapons
    {
        public int MaxDamage { get; set; } = 0;
        public int MaxDice { get; set; } = 0;
        public MonsterWeapon MonsterWeapon { get; set; }
        public bool range { get; set; } = false;




        public void WeaponDamage()
        {
            switch (MonsterWeapon)
            {
                case MonsterWeapon.Fang:
                    MaxDamage = 8;
                    MaxDice = 1;
                    range = true;
                    break;
            }
            switch (MonsterWeapon)
            {
                case MonsterWeapon.Claw:
                    MaxDamage = 8;
                    MaxDice = 1;
                    break;
            }
            switch (MonsterWeapon)
            {
                case MonsterWeapon.Venom:
                    MaxDamage = 8;
                    MaxDice = 2;
                    range = true;
                    break;
            }
            switch (MonsterWeapon)
            {
                case MonsterWeapon.Sword:
                    MaxDamage = 6;
                    MaxDice = 2;
                    break;
            }
            switch (MonsterWeapon)
            {
                case MonsterWeapon.Dagger:
                    MaxDamage = 8;
                    MaxDice = 1;
                    range = true;
                    break;
            }
            switch (MonsterWeapon)
            {
                case MonsterWeapon.Bite:
                    MaxDamage = 6;
                    MaxDice = 2;
                    break;
            }
            switch (MonsterWeapon)
            {
                case MonsterWeapon.BetterFang:
                    MaxDamage = 12;
                    MaxDice = 3;
                    range = true;
                    break;
            }
            switch (MonsterWeapon)
            {
                case MonsterWeapon.BetterClaw:
                    MaxDamage = 12;
                    MaxDice = 3;
                    break;
            }
            switch (MonsterWeapon)
            {
                case MonsterWeapon.BetterVenom:
                    MaxDamage = 12;
                    MaxDice = 4;
                    range = true;
                    break;
            }
            switch (MonsterWeapon)
            {
                case MonsterWeapon.BetterSword:
                    MaxDamage = 8;
                    MaxDice = 4;
                    break;
            }
            switch (MonsterWeapon)
            {
                case MonsterWeapon.BetterDagger:
                    MaxDamage = 12;
                    MaxDice = 3;
                    range = true;
                    break;
            }
            switch (MonsterWeapon)
            {
                case MonsterWeapon.BetterBite:
                    MaxDamage = 8;
                    MaxDice = 4;
                    break;
            }
            switch (MonsterWeapon)
            {
                case MonsterWeapon.BossLongSword:
                    MaxDamage = 12;
                    MaxDice = 8;
                    break;
            }
            switch (MonsterWeapon)
            {
                case MonsterWeapon.BossFang:
                    MaxDamage = 8;
                    MaxDice = 6;
                    break;
            }
            switch (MonsterWeapon)
            {
                case MonsterWeapon.GClaw:
                    MaxDamage = 6;
                    MaxDice = 3;
                    break;
            }
        }
        public int RollDamageMW()
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
    }
    }
