using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using Spectre.Console;

namespace RPGFramework
{

    internal partial class Spell
    {
        public int Damage { get; set; } = 0;

        // these methods need to stay for spells to roll damage and check mana
        public bool CheckMana(int amount, Player p)
        {
            if (p.Mana >= amount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int RollDamageS(int dice, int damage)
        {
            int totalDamage = 0;

            for (int i = 0; i < dice; i++)
            {
                Random rand = new Random();
                int roll = rand.Next(1, damage);
                totalDamage += roll;
            }
            return totalDamage;
        }

        // spells are changing ignore this for now
        /*
        public void Fireball(Player Attacker, Player Target)
        {
            if (CheckMana(5, Attacker) == false)
            {
                Attacker.WriteLine("You do noy have enough mana to cast this spell");
                return;
            }
            else
            {
                Attacker.Mana -= 5;
                Attacker.WriteLine("You have decided to cast FIREBALL.");
                if (Character.DEXSavingThrow(11 + ((Attacker.Wisdom - 10) / 2), Target) == false)
                {
                    Target.TakeDamage(RollDamageS(8, 6));
                    Target.IsBurn = true;
                }
                else
                {
                    Target.TakeDamage(RollDamageS(8, 6) / 2);
                }
            }
        }
        public void Eldritch_Blast(Player Attacker, Player Target)
        {
            // cantrip does not use mana
            int i = 0;
            Attacker.WriteLine("You have decided to cast Eldritch Blast.");
            while (i < (Attacker.Level / 5))
            {
                if (Attacker.RollToHitS(Attacker, Target) == true)
                {
                    Target.TakeDamage(RollDamageS(1, 10));
                }
                else
                {
                    Attacker.WriteLine("You Missed your taget");
                }
                i++;
            }
        }
        public void FireBolt(Player Attacker, Player Target)
        {
            // cantrip does not use mana   
            Attacker.WriteLine("You have decided to cast Fire Bolt.");
            if (Attacker.RollToHitS(Attacker, Target) == true)
            {
                Target.TakeDamage(RollDamageS(2, 8));
                Target.IsBurn = true;
            }
            else
            {
                Attacker.WriteLine("You Missed your taget");
            }
        }
        public void PowerWordKill(Player Attacker, Player Target)
        {
            if (CheckMana(70, Attacker) == false)
            {
                Attacker.WriteLine("You do noy have enough mana to cast this spell");
                return;
            }
            else
            {
                Attacker.Mana -= 70;
                Attacker.WriteLine("You have decided to cast Power Word: Kill.");
                if (Target.Health <= 250)
                {
                    Target.DamageResistance = 1;
                    Target.TakeDamage(Target.MaxHealth);
                } else
                {
                    Attacker.WriteLine("The target has too much health for that");
                }
            }
        } */
    }
}
