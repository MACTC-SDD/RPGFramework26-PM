using RPGFramework.Workflows;
using System;


namespace RPGFramework
{
    internal abstract partial class Character
    {
        public void DropItem(Item item)
        {
            // Remove the item from the character's backpack Items list
            BackPack.Items.Remove(item);
            GetRoom().Items.Add(item);
            item.IsDropped = true;
        }
        public static bool FleeCombat(Character character, CombatWorkflow combat)
        {
            Random rand = new Random();
            int fleeRoll = rand.Next(1, 100);
            if (fleeRoll >= 80)
            {
                combat.Combatants.Remove(character);
                character.CurrentWorkflow = null;
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

        public bool RollToHitS(Character attacker, Character target)
        {
            Random rand = new Random();
            int attackRoll = rand.Next(1, 20);
            int attackModifier = (attacker.Intelligence - 10) / 2;
            int totalAttack = attackRoll + attackModifier;
            int targetAC = target.EquippedArmor.AC + ((target.Dexterity - 10) / 2); //simplified AC calculation
            if (target.EquippedArmor.HasDex == true)
                {
                    if (target.EquippedArmor.DexMax > ((target.Dexterity - 10) / 2))
                    {
                        targetAC += target.EquippedArmor.DexMax;
                    }
                    else
                    {
                        targetAC += (target.Dexterity - 10) / 2;
                    }
                }  
            if (attackRoll == 1)
            {
                if (attacker is Player player)
                    player.WriteLine($"You missed {target.Name}!");
                totalAttack = 0;
                attacker.TakeDamage(1);
                return false;
            }
            else if (totalAttack >= targetAC)
            {
                return true;
            }
            else
            {
                //miss
                if (attacker is Player player)
                    player.WriteLine($"You missed {target.Name}!");
                return false;
            }
        }


        public static void RollToHitW(Character attacker, Weapon weapon, Character target)
        {
            Random rand = new Random();
            int attackRoll = rand.Next(1, 20);
            int attackModifier = (attacker.Strength - 10) / 2;
            int totalAttack = attackRoll + attackModifier;
            int targetAC = target.EquippedArmor.AC + ((target.Dexterity - 10) / 2); //simplified AC calculation
            if (target.EquippedArmor.HasDex == true)
            {
                if (target.EquippedArmor.DexMax > ((target.Dexterity - 10) / 2))
                {
                    targetAC += target.EquippedArmor.DexMax;
                }
                else
                {
                    targetAC += (target.Dexterity - 10) / 2;
                }
            }
            int damageModifier = (attacker.Strength - 10) / 2;
            int totalDamage = weapon.RollDamage();
            if (attackRoll == 20)
            {
                target.TakeDamage(totalDamage * 2);
                target.ReduceDurabilityArmor(target.EquippedArmor, target.EquippedArmor.Durability / 16);
                Comm.SendToIfPlayer(target, "Your armors durability has been reduced to " + target.EquippedArmor.CurrentDurability);
            }
            else if (attackRoll == 1)
            {
                if (attacker is Player player)
                    player.WriteLine($"You missed {target.Name} and hit yourself in the face!");
                totalAttack = 0;
                attacker.DropItem(weapon);
                attacker.TakeDamage(1);
                attacker.ReduceDurabilityWeapon(weapon, (weapon.Durability / 16));
                Comm.SendToIfPlayer(target, "Your weapons durability has been reduced to " + weapon.CurrentDurability);
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
        public void OutOfCombatStatusProcessing()
        {
            if (!InCombat)
            {
                ProcessStatusEffects();
            }
        }
        public void ProcessStatusEffects()
        {
            if (IsPetrified)
            {
                Petrified();
            }
            if (IsBleed)
            {
                Bleed();
            }
            if (IsBlind)
            {
                Blinded();
            }
            if (IsBurn)
            {
                Burn();
            }
            if (IsDeafened)
            {
                Deafened();
            }
            if (IsFreightened)
            {
                Freightened();
            }
            if (IsGappled)
            {
                Grappled();
            }
            if (IsParalyzed)
            {
                Paralyzed();
            }
            if (IsPoisoned)
            {
                Poisoned();
            }
            if (IsStun)
            {
                Stun();
            }
            if (IsUnconcious)
            {
                Unconscious();
            }
            if (IsIncapacitated)
            {
                Incapacitated();
            }
        }
    }
}
    

