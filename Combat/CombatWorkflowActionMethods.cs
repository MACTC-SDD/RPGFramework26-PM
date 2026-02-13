using RPGFramework;
using RPGFramework.Workflows;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Workflows
{
    // CODE REVIEW: Rylan - The steps might be well served by an enum instead of magic numbers
    // because enums are really just ints, you could create an enum like:
    // enum CombatStep { ActionSelection = 0, WeaponSelection = 2, SpellSelection = 3, ItemSelection = 4, Targeting = 5 }
    // This would make the code more readable and maintainable (since you wouldn't have to remember what each number means or renumber)
    // Since these states are only used within this class, you could make the enum private within this file.
    internal partial class CombatWorkflow : IWorkflow
    {
        private bool TargetSpell(Player player, List<string> parameters)
        {
            if (parameters.Count == 0)
            {
                player.WriteLine("You must choose a target!");
                return false;
            }
            else
            {
                string targetName = parameters[0].ToLower();
                Character? chosenTarget = null;
                foreach (Character target in this.Combatants)
                {
                    if (target.Name.ToLower() == targetName && target != player)
                    {
                        chosenTarget = target;
                        break;
                    }
                }
                if (chosenTarget != null)
                {
                    player.WriteLine($"You target {chosenTarget.Name}!");
                    // Here you would add logic to apply the attack or spell effects to the chosen target
                    // TODO Player.RollToHitS(player, selectedSpell, chosenTarget);

                    if (selectedSpell.HasSave == true)
                    {
                        selectedSpell.SaveSpell(player, selectedSpell, chosenTarget);
                    }
                    else if (selectedSpell.IsHeal == true)
                    {
                        selectedSpell.HealSpell(player, selectedSpell);
                    }
                    else
                    {
                        // Won't compile until there is a three param method 
                        // Character.RollToHitS(player, selectedSpell, chosenTarget);
                    }

                    CurrentStep = 0;
                    EndTurn();
                    return true;
                    // CurrentStep = 0; // End turn
                }
                else
                {
                    player.WriteLine("Invalid target selected!");
                    return false;
                }
            }
        }
        private bool TargetWeapon(Player player, List<string> parameters)
        {
            if (parameters.Count == 0)
            {
                player.WriteLine("You must choose a target!");
                return false;
            }
            else
            {
                string targetName = parameters[0].ToLower();
                Character? chosenTarget = null;
                foreach (Character target in this.Combatants)
                {
                    if (target.Name.ToLower() == targetName && target != player)
                    {
                        chosenTarget = target;
                        break;
                    }
                }
                if (chosenTarget != null)
                {
                    player.WriteLine($"You target {chosenTarget.Name}!");
                    if (selectedWeapon != null)
                        Player.RollToHitW(player, selectedWeapon, chosenTarget);

                    CurrentStep = 0;
                    EndTurn();
                    return true;
                    // CurrentStep = 0; // End turn
                }
                else
                {
                    player.WriteLine("Invalid target selected!");
                    return false;
                }
            }
        }
        private bool ChooseItem(Player player, List<string> parameters)
        {
            // Rylan - see my notes in ChooseWeapon about getting just consumables
            List<Consumable> consumables = [];
            foreach (Consumable item in player.BackPack.Items)
            {
                consumables.Add(item);
            }
            string itemName = parameters[0].ToLower();
            if (itemName == "back" || itemName == "exit")
            {
                CurrentStep = 0; // go back to action selection
                return false;
            }
            Consumable? chosenItem = null;
            foreach (Consumable item in consumables)
            {
                if (item.Name.ToLower() == itemName)
                {
                    chosenItem = item;
                    break;
                }
            }
            if (chosenItem != null)
            {
                player.WriteLine($"You use the {chosenItem.Name}!");
                // Here you would add logic to apply the item's effects
                player.Heal(chosenItem.HealAmount);
                player.BackPack.Items.Remove(chosenItem); // Remove used item from inventory
                CurrentStep = 0; // End turn
                EndTurn();
                return true;
            }
            else
            {
                player.WriteLine("You don't have that item!");
                CurrentStep = 4; // stay in item selection
                return false;
            }
        }
        private void ChooseSpell(Player player, List<string> parameters)
        {
            List<Spell> spells = [];
            foreach (Spell spell in player.Spellbook)
            {
                spells.Add(spell);
            }
            if (parameters.Count == 0)
            {
                player.WriteLine("You must choose a spell to cast!");
            }
            else
            {
                string spellName = parameters[0].ToLower();
                if (spellName == "back" || spellName == "exit")
                {
                    CurrentStep = 0; // go back to action selection
                    return;
                }

                foreach (Spell spell in spells)
                {
                    if (spell.Name.ToLower() == spellName)
                    {
                        selectedSpell = spell;
                        break;
                    }
                }
                if (selectedSpell != null)
                {
                    player.WriteLine($"You cast {selectedSpell.Name}!");

                    player.WriteLine("Select your target:");
                    foreach (CombatWorkflow combat in GameState.Instance.Combats)
                    {
                        if (combat.Combatants.Contains(player))
                        {
                            foreach (Character target in combat.Combatants)
                            {
                                if (target != player)
                                {
                                    player.WriteLine($"- {target.Name}");
                                }
                            }
                        }
                    }
                    CurrentStep = 5; // targeting phase next
                }
                else
                {
                    player.WriteLine("You don't know that spell!");
                    CurrentStep = 3; // stay in spell selection
                }
            }
        }
        private void ChooseWeapon(Player player, List<string> parameters)
        {
            // CODE REVIEW: Rylan - I moved this check to the top to remove nesting
            if (parameters.Count == 0)
            {
                player.WriteLine("You must choose a weapon to attack with!");
                return;
            }

            // CODE REVIEW: Rylan - Not everything in inventory will be a weapon
            // Here is a way to get a list of just the weapons
            List<Weapon> weapons = [.. player.BackPack.Items.OfType<Weapon>()];

            /*List<Weapon> weapons = new List<Weapon>();

            foreach (Weapon weapon in player.Inventory)
            {
                weapons.Add(weapon);
            }*/

            string weaponName = parameters[0].ToLower();
            if (weaponName == "back" || weaponName == "exit")
            {
                CurrentStep = 0; // go back to action selection
                return;
            }

            // CODE REVIEW: Rylan - Here is a shorter way to find the weapon
            Weapon? selectedWeapon = weapons.FirstOrDefault(w => w.Name.ToLower() == weaponName);
            /*foreach (Weapon weapon in weapons)
            {
                if (weapon.Name.ToLower() == weaponName)
                {
                    selectedWeapon = weapon;
                    break;
                }
            }*/

            // Moved this above to reduce nesting
            if (selectedWeapon == null)
            {
                player.WriteLine("You don't have that weapon!");
                CurrentStep = 2; // stay in weapon selection
                return;
            }

            player.WriteLine($"You attack with your {selectedWeapon.Name}!");

            // CODE REVIEW: Rylan - A little confusing since this shows targers from all combats
            // a player might be in. This might be by design...
            player.WriteLine("Select your target:");
            foreach (CombatWorkflow combat in GameState.Instance.Combats)
            {
                if (combat.Combatants.Contains(player))
                {
                    foreach (Character target in combat.Combatants)
                    {
                        if (target != player)
                        {
                            player.WriteLine($"- {target.Name}");
                        }
                    }
                }
            }
            CurrentStep = 5; // targeting phase next
        }
    }
}
