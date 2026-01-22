using RPGFramework;
using RPGFramework.Combat;
using RPGFramework.Commands;
using System.Numerics;

namespace RPGFramework.Workflows
{
    internal partial class CombatWorkflow : IWorkflow
    {

        public int CurrentStep { get; set; } = 0;
        public string Description { get; } = "Manages the sequence of actions during a combat turn.";
        public string Name { get; } = "Combat Turn Workflow";
        public List<ICommand> PreProcessCommands { get; private set; } = new List<ICommand>();
        public List<ICommand> PostProcessCommands { get; private set; } = new List<ICommand>();

        public Dictionary<string, object> WorkflowData { get; set; } = new Dictionary<string, object>();

        public List<Character> Combatants = new List<Character>();
        public async Task CombatInitialization(Character attacker, Character enemy)
        {
            Combatants.Add(attacker);
            Combatants.Add(enemy);
            foreach (NonPlayer npc in attacker.GetRoom().GetNonPlayers())
            {
                //if (npc.Hostile == true || npc.Army == true)
                {
                    Combatants.Add(npc);
                }
            }
            foreach (Character c in Combatants)
            {
                Random rand = new Random();
                int initiativeRoll = rand.Next(1, 20);
                int dexterityModifier = (c.Dexterity - 10) / 2;
                c.Initiative = initiativeRoll + dexterityModifier;
            }
            InitiativeOrder(Combatants);
            foreach (Character c in Combatants)
            {
                c.EngageCombat(true);
                c.CurrentWorkflow = this;
            }
        }
        public Character ActiveCombatant { get; set; } = null!;

        public List<Character> Elves = new List<Character>();
        public List<Character> Monsters = new List<Character>();
        public List<Character> Constructs = new List<Character>();
        public List<Character> Bandits = new List<Character>();
        public List<Character> Army = new List<Character>();
        public void SortCombatants()
        {
            foreach (Character c in Combatants)
            {
                if (c is NonPlayer npc)
                {
                    if (npc.IsHumanoid)
                    {
                        if (npc.IsElf)
                            Elves.Add(c);
                        else
                            Bandits.Add(c);

                    }
                    if (npc is Mob m)
                    {
                        if (m.IsMonster)
                        {
                            Monsters.Add(c);
                        }
                        if (m.IsConstruct)
                        {
                            Constructs.Add(c);
                        }
                    }
                    if (npc.IsArmy)
                    {
                        Army.Add(c);
                    }
                }
                else
                    continue;
            }
        }

        public void InitiativeOrder(List<Character> combatants)
        {
            Combatants = Combatants.OrderByDescending(c => c.Initiative).ToList();
        }
        public static CombatWorkflow Create(Character attacker, Character enemy)
        {
            CombatWorkflow combat = new CombatWorkflow();
            GameState.Instance.Combats.Add(combat);
            combat.CombatInitialization(attacker, enemy);
            return combat;

        }
        public Weapon? selectedWeapon = null;
        public Spell? selectedSpell = null;
        

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

            
            CombatWorkflow? currentCombat = null;
            foreach (CombatWorkflow combat in GameState.Instance.Combats)
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
                                Player.FleeCombat(player, currentCombat);
                                player.CurrentWorkflow = null;
                                break;
                        }
                        
                    }
                    break;
                case 2:
                    // second step of attack action
                    ChooseWeapon(player, parameters);
                    break;
                case 3:
                // second step of cast spell action
                    ChooseSpell(player, parameters);
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
                        foreach (CombatWorkflow combat in GameState.Instance.Combats)
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
                            Player.RollToHit(player, selectedWeapon, chosenTarget);
                            
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
                        foreach (CombatWorkflow combat in GameState.Instance.Combats)
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
                            Player.RollToHitS(player, selectedSpell, chosenTarget);

                            CurrentStep = 0;
                            // CurrentStep = 0; // End turn
                        }
                        else
                        {
                            player.WriteLine("Invalid target selected!");
                        }
                    }
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
