using System;
﻿using RPGFramework.Workflows;


namespace RPGFramework
{
    internal abstract partial class Character
    {
        public void DropItem(Character c, Item item)
        {
            c.BackPack.Items.Remove(item);
            c.GetRoom().Items.Add(item);
            item.IsDropped = true;
        }

        public static bool FleeCombat(Character character, CombatWorkflow combat)
        {
            Random rand = new Random();
            int fleeRoll = rand.Next(1, 100);
            if (fleeRoll >= 80)
                if (true) // Rylan - fix, not sure what the linebelow means.
                {
                    //{e(character);
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
            return false;
        }

        public static void RollToHitS(Character attacker, Spell weapon, Character target)
        {
            Random rand = new Random();
            int attackRoll = rand.Next(1, 20);
            int attackModifier = (attacker.Intelligence - 10) / 2;
            int totalAttack = attackRoll + attackModifier;
            int targetAC = 10 + ((target.Dexterity - 10) / 2); //simplified AC calculation
            int damageModifier = (attacker.Intelligence - 10) / 2;
            int totalDamage = weapon.Damage + damageModifier;
            if (attackRoll == 20)
            {
                target.TakeDamage(totalDamage * 2);
            }
            else if (attackRoll == 1)
            {
                if (attacker is Player player)
                    player.WriteLine($"You missed {target.Name}!");
                totalAttack = 0;
                attacker.TakeDamage(1);
            }
            else if (totalAttack >= targetAC)
            {
                target.TakeDamage(totalDamage);
                if (attacker is Player player)
                    player.WriteLine($"You hit {target.Name} for {totalDamage} damage!");
                if (target is Player targetPlayer)
                {
                    targetPlayer.WriteLine($"{attacker.Name} hit you with {weapon.Name} for {totalDamage} damage!");
                }
            }
            else
            {
                //miss
                if (attacker is Player player)
                    player.WriteLine($"You missed {target.Name}!");
            }
        }


        public static void RollToHitW(Character attacker, Weapon weapon, Character target)
        {
            Random rand = new Random();
            int attackRoll = rand.Next(1, 20);
            int attackModifier = (attacker.Strength - 10) / 2;
            int totalAttack = attackRoll + attackModifier;
            int targetAC = 10 + ((target.Dexterity - 10) / 2); //simplified AC calculation
            int damageModifier = (attacker.Strength - 10) / 2;
            int totalDamage = weapon.Damage + damageModifier;
            if (attackRoll == 20)
            {
                target.TakeDamage(totalDamage * 2);
            }
            else if (attackRoll == 1)
            {
                if (attacker is Player player)
                    player.WriteLine($"You missed {target.Name} and hit yourself in the face!");
                totalAttack = 0;
                attacker.TakeDamage(1);
            }
            else if (totalAttack >= targetAC)
            {
                target.TakeDamage(totalDamage);
                if (attacker is Player player)
                    player.WriteLine($"You hit {target.Name} for {totalDamage} damage!");
                if (target is Player targetPlayer)
                {
                    targetPlayer.WriteLine($"{attacker.Name} hit you with {weapon.Name} for {totalDamage} damage!");
                }
            }
            else
            {
                //miss
                if (attacker is Player player)
                    player.WriteLine($"You missed {target.Name}!");
            }
        }
    }
}
