using RPGFramework.Workflows;
using RPGFramework.Combat;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework
{
    internal abstract partial class Character
    {
        public static bool FleeCombat(Character character, CombatWorkflow combat)
        {
            Random rand = new Random();
            int fleeRoll = rand.Next(1, 100);
            if (fleeRoll >= 80)
            {
                combat.Combatants.Remove(character);
                if (character is Player player)
                    player.WriteLine("You successfully fled the combat!");
                return true;
            }
            else
            {
                if (character is Player player)
                    player.WriteLine("You failed to flee the combat!");
                return false;
            }
        }

        public static void RollToHitS(Character a, Spell weapon, Character t)
        {
            Random rand = new Random();
            int attackRoll = rand.Next(1, 20);
            int attackModifier = (a.Intelligence - 10) / 2;
            int totalAttack = attackRoll + attackModifier + a.Advantage - a.HitPenalty - a.Disadvantage;
            int tAC = 10 + ((t.Dexterity - 10) / 2) + t.Advantage - t.Disadvantage; //simplified AC calculation
            int damageModifier = (a.Intelligence - 10) / 2;
            int totalDamage = weapon.Damage + damageModifier;
            if (attackRoll == 20)
            {
                t.TakeDamage(totalDamage * 2);
            }
            else if (attackRoll == 1)
            {
                if (a is Player player)
                    player.WriteLine($"You missed {t.Name}!");
                totalAttack = 0;
                a.TakeDamage(1);
            }
            else if (totalAttack >= tAC)
            {
                t.TakeDamage(totalDamage);
                if (a is Player player)
                    player.WriteLine($"You hit {t.Name} for {totalDamage} damage!");
                if (t is Player tPlayer)
                {
                    tPlayer.WriteLine($"{a.Name} hit you with {weapon.Name} for {totalDamage} damage!");
                }
            }
            else
            {
                //miss
                if (a is Player player)
                    player.WriteLine($"You missed {t.Name}!");
            }
        }


        public static void RollToHitW(Character a, Weapon weapon, Character t)
        {
            Random rand = new Random();
            int attackRoll = rand.Next(1, 20);
            int attackModifier = (a.Strength - 10) / 2;
            int totalAttack = attackRoll + attackModifier + a.Advantage - a.HitPenalty - a.Disadvantage;
            int tAC = 10 + ((t.Dexterity - 10) / 2) + t.Advantage - t.Disadvantage; //simplified AC calculation
            int damageModifier = (a.Strength - 10) / 2;
            int totalDamage = weapon.Damage + damageModifier;
            if (attackRoll == 20)
            {
                t.TakeDamage(totalDamage * 2);
            }
            else if (attackRoll == 1)
            {
                if (a is Player player)
                    player.WriteLine($"You missed {t.Name} and hit yourself in the face!");
                totalAttack = 0;
                a.TakeDamage(1);
            }
            else if (totalAttack >= tAC)
            {
                t.TakeDamage(totalDamage);
                if (a is Player player)
                    player.WriteLine($"You hit {t.Name} for {totalDamage} damage!");
                if (t is Player tPlayer)
                {
                    tPlayer.WriteLine($"{a.Name} hit you with {weapon.Name} for {totalDamage} damage!");
                }
            }
            else
            {
                //miss
                if (a is Player player)
                    player.WriteLine($"You missed {t.Name}!");
            }
        }
    }
}
