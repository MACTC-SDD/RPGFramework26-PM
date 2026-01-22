using RPGFramework;
using RPGFramework.Workflows;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Workflows
{

    internal partial class CombatWorkflow : IWorkflow
    {
        private void ChooseSpell(Player player, List<string> parameters)
        {
            List<Spell> spells = new List<Spell>();
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
                    CurrentStep = 1; // go back to action selection
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
            List<Weapon> weapons = new List<Weapon>();
            foreach (Weapon weapon in player.Inventory)
            {
                weapons.Add(weapon);
            }
            if (parameters.Count == 0)
            {
                player.WriteLine("You must choose a weapon to attack with!");
            }
            else
            {
                string weaponName = parameters[0].ToLower();
                if (weaponName == "back" || weaponName == "exit")
                {
                    CurrentStep = 1; // go back to action selection
                }

                foreach (Weapon weapon in weapons)
                {
                    if (weapon.Name.ToLower() == weaponName)
                    {
                        selectedWeapon = weapon;
                        break;
                    }
                }
                if (selectedWeapon != null)
                {
                    player.WriteLine($"You attack with your {selectedWeapon.Name}!");

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
                    player.WriteLine("You don't have that weapon!");
                    CurrentStep = 2; // stay in weapon selection
                }
            }
        }
    }
}
