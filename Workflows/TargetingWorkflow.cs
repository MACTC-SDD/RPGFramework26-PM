using RPGFramework.Combat;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Workflows
{
    internal class TargetingWorkflow : IWorkflow
    {
        public Dictionary<string, object> WorkflowData { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int CurrentStep { get; set; } = 0;
        int IWorkflow.CurrentStep { get => CurrentStep; set => CurrentStep = value; }
        string Description { get; } = "Handles the targeting process for actions such as attacks or spells.";

        string IWorkflow.Description => Description;

        string Name { get; } = "Targeting Workflow";

        string IWorkflow.Name => Name;

        public void Execute(Player player, List<string> parameters)
        {
            List<CombatObject> activeCombats = new List<CombatObject>();
            foreach (CombatObject combat in GameState.Instance.Combats)
            {
                if (combat.combatants.Contains(player))
                {
                    activeCombats.Add(combat);
                }
            }
            switch (CurrentStep)
            {
                case 0:
                    player.WriteLine("Select your target:");
                    // Logic to display potential targets
                    foreach (CombatObject combat in activeCombats)
                    {
                        foreach (Character combatant in combat.combatants)
                        {
                            if (combatant != player)
                            {
                                player.WriteLine($"- {combatant.Name}");
                            }
                        }
                    }
                    CurrentStep++;
                    break;
                case 1:
                    if (parameters.Count == 0 || parameters.Count >1)
                    {
                        player.WriteLine("Invalid Target. Please select a real target.");
                        return;
                    }
                    string target = parameters[0];
                    player.WriteLine($"You have selected {target} as your target.");
                    // Logic to confirm target selection
                    TargetedEnemy = target; 
                    CurrentStep++;
                    break;
                case 2:
                    player.WriteLine("Target confirmed. Proceeding with action.");
                    // Logic to execute the action on the selected target
                    CurrentStep = 0; // Reset for next use
                    player.CurrentWorkflow = null; // Exit workflow
                    break;
                default:
                    player.WriteLine("Invalid step in targeting workflow.");
                    CurrentStep = 0; // Reset on error
                    break;
            }
        }
    }
}
