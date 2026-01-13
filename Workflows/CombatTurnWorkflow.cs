using System;
using System.Collections.Generic;
using System.Text;
using RPGFramework.Engine;
using RPGFramework.Combat;

namespace RPGFramework.Workflows
{
    internal class CombatTurnWorkflow : IWorkflow
    {

        public int CurrentStep { get; set; } = 0;
        public string
            Description => "Manages the sequence of actions during a combat turn.";
        public string Name => "Combat Turn Workflow";
        public Dictionary<string, object> WorkflowData { get; set; } = new Dictionary<string, object>();
        public void Execute(Player player, List<string> parameters)
        {
            // Placeholder for combat turn logic
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
                        CombatObject playerCombat;


                        switch (action)
                        {
                            case "attack":
                                player.WriteLine($"What do you attack with?");
                                foreach (Weapon weapon in player.Inventory)
                                {
                                    player.WriteLine($"- {weapon.Name}");
                                }
                                break;
                            case "cast spell":
                                player.WriteLine($"Which spell do you want to cast?");
                                foreach (Spell spell in player.Spellbook)
                                {
                                    player.WriteLine($"- {spell.Name}");
                                }
                                break;
                            case "inventory":
                                player.WriteLine("You open your inventory:");
                                List<Consumable> consumables = new List<Consumable>();
                                foreach (Consumable item in player.Inventory)
                                {
                                    player.WriteLine($"- {item.Name}");
                                    consumables.Add(item);
                                }
                                break;
                            case "flee":
                                player.WriteLine("You attempt to flee from combat!");
                                foreach (CombatObject combat in GameState.Instance.Combats)
                                {
                                    if (combat.combatants.Contains(player))
                                    {
                                        Random rand = new Random();
                                        int fleeRoll = rand.Next(1, 100);
                                        if (fleeRoll >= 80)
                                        {
                                            combat.combatants.Remove(player);

                                            player.WriteLine("You successfully fled the combat!");

                                        }
                                        else
                                        {

                                            player.WriteLine("You failed to flee the combat!");

                                        }
                                    }
                                }
                                
                                break;
                        }
                        CurrentStep = 0; // Reset for next turn
                    }
                    break;
                default:
                    player.WriteLine("Invalid step in combat turn workflow.");
                    break;
            }
        }
    }
}
