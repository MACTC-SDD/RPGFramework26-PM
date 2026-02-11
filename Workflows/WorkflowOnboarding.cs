using RPGFramework.Commands;
using RPGFramework.Enums;
namespace RPGFramework.Workflows
{
    internal class WorkflowOnboarding : IWorkflow
    {
        public int CurrentStep { get; set; } = 0;
        public string Description => "Guides new players through the initial setup and familiarization with the game mechanics.";
        public string Name => "Onboarding Workflow";
        public List<ICommand> PreProcessCommands { get; private set; } = [];
        public List<ICommand> PostProcessCommands { get; private set; } = [];

        public Dictionary<string, object> WorkflowData { get; set; } = [];
        public void Execute(Player player, List<string> parameters)
        {
            // 1. We'll assume we didn't get here if player exists, if they did that will have authenticated instead
            // 2. Gather player class
            // 3. Roll stats and loop until accepted
            // 4. Introduce basic commands

            // TODO: Rather than this giant switch statement , consider breaking each step
            // into its own method for clarity and maintainability.
            // TODO: Determine what happens if player logs out while workflow is active?
            //   Should Logout/Disconnect check for workflow? Should Workflow have a Rollback method?
            //   Or, should we Serialize Workflow with Player, at least for onboarding. Might be confusing.

            // The action we take will depend on the CurrentStep, we will store progress in WorkflowData
            player.WriteLine($"Workflow: {Name}, Step: {CurrentStep}");
            string classList = string.Join(", ", GameState.Instance.CCCatalog.Keys.OrderBy(x => x));

            switch (CurrentStep)
            {
                case 0:
                    // TODO: Make the name of the game configurable
                    player.WriteLine(Name + ": Hello and welcome to the RPG World!");
                    player.WriteLine("Let's secure your account first. Please enter a password.");

                    GameState.Log(DebugLevel.Debug, $"{player.Name} is starting onboarding workflow.");
                    CurrentStep++;
                    break;
                case 1:
                    if (parameters.Count == 0)
                        player.WriteLine("No blank passwords allowed!");
                    else
                    {
                        player.SetPassword(parameters[0]);
                        player.WriteLine(Name + ": Welcome to the game! Let's start by choosing your character class.");

                        player.WriteLine($"Please choose from the following classes: {classList}");
                        //player.WriteLine("Available classes: Warrior, Mage, Rogue, Paladin, Bard, Druid, Necromancer.");
                        CurrentStep++;
                    }
                    break;

                case 2:
                    // Step 2: Gather player class and validate
                    string chosenClass = parameters.Count > 0 ? parameters[0].ToLower() : string.Empty;
                    if (!GameState.Instance.CCCatalog.TryGetValue(chosenClass, out CharacterClass? charClass)
                        || charClass == null)
                    {
                        player.WriteLine($"Invalid class chosen. Please choose from: {classList}.");
                        break;
                    }

                    player.Class = charClass;
                    WorkflowData["ChosenClass"] = chosenClass;
                    CurrentStep++;

                    player.WriteLine($"You have chosen the {chosenClass} class.");
                    // If class is valid, proceed, otherwise print message and stay on this step
                    // Placeholder logic
                    break;     
                case 3:
                    // Step 2: Roll stats and loop until accepted
                    // Placeholder logic
                   // player.Initiative = GameState.Instance.Random.Next(1, 20);
                    player.Strength = GameState.Instance.Random.Next(1, 20);
                    player.Dexterity = GameState.Instance.Random.Next(1, 20);
                    player.Intelligence = GameState.Instance.Random.Next(1, 20);
                    player.Wisdom = GameState.Instance.Random.Next(1, 20);
                    player.Constitution = GameState.Instance.Random.Next(1, 20);
                    player.Charisma = GameState.Instance.Random.Next(1, 20);
                    player.WriteLine($"{player.Name}'s rolled stats: S:{player.Strength},D:{player.Dexterity},I:{player.Intelligence},W:{player.Wisdom},Co:{player.Constitution},Ch:{player.Charisma}");
                    player.WriteLine($"Do you accept these stats?  (y/n)");

                    CurrentStep++;
                    break;
                case 4:
                    // Step 3: Introduce basic commands
                    // accept if "y" or "yes", re-roll if "n" or "no"
                    if (parameters[0].ToLower() == "y" || parameters[0].ToLower() == "yes")
                    {
                        player.WriteLine("Stats accepted.");
                        player.SetCarryCapacity();
                        string raceList = string.Join(", ", GameState.Instance.RaceCatalog.Keys.OrderBy(x => x));
                        player.WriteLine($"Please choose from the following races: {raceList} ");
                        CurrentStep++;
                    }
                    else 
                    {
                        player.WriteLine("Re-rolling stats...");
                        CurrentStep++;
                        // Stay on this step to re-roll
                    }
                    break;
                    case 5:
                    // step 4 player chooses race 
                    string chosenRace = parameters.Count > 0 ? parameters[0].ToLower() : string.Empty;
                    if (!GameState.Instance.RaceCatalog.TryGetValue(chosenRace, out Race? charRace)
                      || charRace == null)
                    {
                        string raceList = string.Join(", ", GameState.Instance.RaceCatalog.Keys.OrderBy(x => x));
                        player.WriteLine($"Please choose from the following races: {raceList} ");
                        break;
                    }
                    else
                    {
                        // add any attribute bonuses that come from race
                        player.Strength += charRace.StrengthIncr;
                            player.Dexterity += charRace.DexterityIncr;
                            player.Intelligence += charRace.IntelligenceIncr;
                            player.Wisdom += charRace.WisdomIncr;
                            player.Constitution += charRace.ConstitutionIncr;
                            player.Charisma += charRace.CharismaIncr;
                        player.WriteLine("You Have Successfully Chosen A Race");
                        player.WriteLine("Hit |Enter| To Continue");
                    }

                        player.Race = charRace;
                    WorkflowData["ChosenRace"] = chosenRace;
                    CurrentStep++;
                    

                    break;

                default:
                    // Onboarding complete
                    // TODO: Set PlayerClass (or maybe do that in step above) and save Player
                    player.WriteLine(Name + ": Onboarding complete! You are now ready to explore the game world.");
                    player.WriteLine("Your class is: " + player.Class.Name);
                    player.WriteLine("Your Race is: " + player.Race.Name);
                    player.WriteLine("Type 'help' to see a list of available commands.");
                    player.CurrentWorkflow = null;
                    break;
            }
            
        }    
    }
}
