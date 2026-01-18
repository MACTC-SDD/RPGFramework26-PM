using RPGFramework.Combat;
using RPGFramework.Commands;

namespace RPGFramework.Workflows
{
    internal class CombatTurnWorkflow : IWorkflow
    {

        public int CurrentStep { get; set; } = 0;
        public string Description { get; } = "Manages the sequence of actions during a combat turn.";
        public string Name { get; } = "Combat Turn Workflow";
        public List<ICommand> PreProcessCommands { get; private set; } = new List<ICommand>();
        public List<ICommand> PostProcessCommands { get; private set; } = new List<ICommand>();

        public Dictionary<string, object> WorkflowData { get; set; } = new Dictionary<string, object>();


        // CODE REVIEW: Rylan - This needs to be broken down into smaller chunks.        
        // Consider starting with a method for each case in the switch statement.
        // A good rule of thumb is that if a method is longer than 20-30 lines it
        // is probably too long.
        // NOTE: You have some warnings about possible null references. These are because
        // you are defining things like selectedWeapon as nullable types, but then using
        // them as non-nullable later (ie. passing them to RollToHit). See me for help
        // if this doesn't make sense. 
        public void Execute(Player player, List<string> parameters)
        {
            // Process any pre-process commands, if it matches, we'll execute it and return
            if (CommandManager.ProcessSpecificCommands(player, parameters, PreProcessCommands))
                return;

            Weapon? selectedWeapon = null;
            Spell? selectedSpell = null;
            CombatObject? currentCombat = null;
            foreach (CombatObject combat in GameState.Instance.Combats)
            {
                if (combat.Combatants.Contains(player))
                {
                    currentCombat = combat;
                    break;
                }
            }
            switch (CurrentStep)
            {
                case 0:
                    player.WriteLine(Name + ": It's your turn! Choose an action: \n1. Attack \n2. Cast Spell \n3. Inventory \n4. Flee.");
                    CurrentStep++;
                    break;
                case 1:
                    if (parameters.Count == 0)
                    {
                        player.WriteLine("You must choose an action!");
                    }
                    else
                    {
                        string action = parameters[0].ToLower();
                        // Process the chosen action
                        player.WriteLine($"You chose to {action}.");
                        // After processing, end the turn
                        


                        switch (action)
                        {
                            case "attack":
                                player.WriteLine($"What do you attack with?");
                                foreach (Weapon weapon in player.Inventory)
                                {
                                    player.WriteLine($"- {weapon.Name}");
                                }
                                CurrentStep = 2;
                                break;

                            case "cast spell":
                                player.WriteLine($"Which spell do you want to cast?");
                                foreach (Spell spell in player.Spellbook)
                                {
                                    player.WriteLine($"- {spell.Name}");
                                }
                                CurrentStep = 3;
                                break;
                            case "inventory":
                                player.WriteLine("You open your inventory:");
                                foreach (Consumable item in player.Inventory)
                                {
                                    player.WriteLine($"- {item.Name}");
                                }
                                player.WriteLine("Which item do you want to use?");
                                foreach (Consumable item in player.GetConsumables())
                                {
                                    player.WriteLine($"- {item.Name}");
                                }
                                CurrentStep = 4;
                                break;
                            case "flee":
                                player.WriteLine("You attempt to flee from combat!");
                                currentCombat.FleeCombat(player);
                                player.CurrentWorkflow = null;
                                break;
                        }
                        
                    }
                    break;
                case 2:
                    // second step of attack action

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
                            break;
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
                            foreach (CombatObject combat in GameState.Instance.Combats)
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
                    break;
                case 3:
                // second step of cast spell action
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
                            break;
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
                            foreach (CombatObject combat in GameState.Instance.Combats)
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
                    break;
                case 4:
                    // second step of inventory action
                    List<Consumable> consumables = new List<Consumable>();
                    foreach (Consumable item in player.Inventory)
                    {
                        consumables.Add(item);
                    }
                    string itemName = parameters[0].ToLower();
                    if (itemName == "back" || itemName == "exit")
                    {
                        CurrentStep = 1; // go back to action selection
                        break;
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
                        player.Inventory.Remove(chosenItem); // Remove used item from inventory
                        CurrentStep = 0; // End turn
                    }
                    else
                    {
                        player.WriteLine("You don't have that item!");
                        CurrentStep = 4; // stay in item selection
                    }
                    player.CurrentWorkflow = null;
                    break;
                case 5:
                    // targeting phase for attack
                    
                    if (parameters.Count == 0)
                    {
                        player.WriteLine("You must choose a target!");
                    }
                    else
                    {
                        string targetName = parameters[0].ToLower();
                        Character? chosenTarget = null;
                        foreach (CombatObject combat in GameState.Instance.Combats)
                        {
                            if (combat.Combatants.Contains(player))
                            {
                                foreach (Character target in combat.Combatants)
                                {
                                    if (target.Name.ToLower() == targetName && target != player)
                                    {
                                        chosenTarget = target;
                                        break;
                                    }
                                }
                            }
                        }
                        if (chosenTarget != null)
                        {
                            player.WriteLine($"You target {chosenTarget.Name}!");
                            // Here you would add logic to apply the attack or spell effects to the chosen target
                            CombatObject.RollToHit(player, selectedWeapon, chosenTarget);
                            
                            CurrentStep = 0;
                            // CurrentStep = 0; // End turn
                        }
                        else
                        {
                            player.WriteLine("Invalid target selected!");
                        }
                    }
                    player.CurrentWorkflow = null;
                    break; 
                    case 6:
                    // targeting phase for spell
                    if (parameters.Count == 0)
                    {
                        player.WriteLine("You must choose a target!");
                    }
                    else
                    {
                        string targetName = parameters[0].ToLower();
                        Character? chosenTarget = null;
                        foreach (CombatObject combat in GameState.Instance.Combats)
                        {
                            if (combat.Combatants.Contains(player))
                            {
                                foreach (Character target in combat.Combatants)
                                {
                                    if (target.Name.ToLower() == targetName && target != player)
                                    {
                                        chosenTarget = target;
                                        break;
                                    }
                                }
                            }
                        }
                        if (chosenTarget != null)
                        {
                            player.WriteLine($"You target {chosenTarget.Name}!");
                            // Here you would add logic to apply the attack or spell effects to the chosen target
                            CombatObject.RollToHitS(player, selectedSpell, chosenTarget);

                            CurrentStep = 0;
                            // CurrentStep = 0; // End turn
                        }
                        else
                        {
                            player.WriteLine("Invalid target selected!");
                        }
                    }
                    player.CurrentWorkflow = null;
                    break;
                default:
                    player.WriteLine("Invalid step in combat turn workflow.");
                    break;
            }

            // Process any pre-process commands, if it matches, we'll execute it and return
            if (CommandManager.ProcessSpecificCommands(player, parameters, PostProcessCommands))
                return;
        }
    }
}
