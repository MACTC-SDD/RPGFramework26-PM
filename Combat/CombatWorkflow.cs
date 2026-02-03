using RPGFramework.Commands;
using RPGFramework.Geography;
using RPGFramework;

namespace RPGFramework.Workflows
{
    // probably move this and other partial classes for this class to the combat folder,
    // this one may be fine to stay since its the main workflow
    internal partial class CombatWorkflow : IWorkflow
    {
        public int CurrentStep { get; set; } = 0;
        public string Description { get; } = "Manages the sequence of actions during a combat turn.";
        public string Name { get; } = "Combat Turn Workflow";
        public List<ICommand> PreProcessCommands { get; private set; } = [];
        public List<ICommand> PostProcessCommands { get; private set; } =
        [
                new AnnounceCommand(),
                new ShutdownCommand(),
                new WhereCommand(),
                new WhoCommand(),
                new GoToCommand(),
                new SaveAll(),
                new SummonCommand(),
                new KickCommand(),
                new RoleCommand(),
                new RenameCommand(),
                new HelpEditCommand(),
                new AFKCommand(),
                new IpCommand(),
                new LookCommand(),
                new QuitCommand(),
                new SayCommand(),
                new TimeCommand(),
                new StatusCommand(),
                new HelpCommand(),
                new UXCommand(),
                new UXColorCommand(),
                new UXDecorationCommand(),
                new UXPanelCommand(),
                new UXTreeCommand(),
                new UXBarChartCommand(),
                new UXCanvasCommand(),
                new CombatAdminControlsCommand()
        ];

        public Dictionary<string, object> WorkflowData { get; set; } = [];

        public int TurnTimer { get; set; } = 0;
        public int RoundCounter { get; set; } = 0;

        public Character? PreviousActingCharacter { get; set; }

        public List<Character> Combatants = [];
        public async Task CombatInitialization(Character attacker, Character enemy)
        {
            Combatants.Add(attacker);
            Combatants.Add(enemy);
            foreach (NonPlayer npc in attacker.GetRoom().NonPlayers)
            {
                //if (npc.Hostile == true || npc.Army == true)
                {
                    Combatants.Add(npc);
                }
            }
            foreach (Character c in Combatants)
            {
                Random rand = new();
                int initiativeRoll = rand.Next(1, 20);
                int dexterityModifier = (c.Dexterity - 10) / 2;
                c.Initiative = initiativeRoll + dexterityModifier;
            }
            //InitiativeOrder(Combatants);
            InitiativeOrder();
            foreach (Character c in Combatants)
            {
                c.EngageCombat(true);
                c.CurrentWorkflow = this;
            }
        }

        public void RollInitiative(Character c)
        {
            Random rand = new();
            int initiativeRoll = rand.Next(1, 20);
            int dexterityModifier = (c.Dexterity - 10) / 2;
            c.Initiative = initiativeRoll + dexterityModifier;
        }
        public Character ActiveCombatant { get; set; } = null!;

        public List<Character> Elf = [];
        public List<Character> Monster = [];
        public List<Character> Bandit = [];
        public List<Character> Construct = [];
        public List<Character> Army = [];
        public List<Character> Miscellaneous = [];
        public List<Character> Players = [];
        public void SortCombatants()
        {
            foreach (Character c in Combatants)
            {
                if (c is NonPlayer npc)
                {
                    if (npc.IsHumanoid)
                    {
                        if (npc.IsElf)
                        {
                            npc.CombatFaction = Enums.CombatFaction.Elf;
                            Elf.Add(npc);
                        }

                        else
                        {
                            npc.CombatFaction = Enums.CombatFaction.Bandit;
                            Bandit.Add(npc);
                        }

                    }
                    else if (npc is Mob m)
                    {
                        if (m.IsMonster)
                        {
                            m.CombatFaction = Enums.CombatFaction.Monster;
                            Monster.Add(m);
                        }
                        if (m.IsConstruct)
                        {
                            m.CombatFaction = Enums.CombatFaction.Construct;
                            Construct.Add(npc);
                        }
                    }
                    else if (npc.IsArmy)
                    {
                        npc.CombatFaction = Enums.CombatFaction.Army;
                        Army.Add(npc);
                    }
                    else
                    {
                        npc.CombatFaction = Enums.CombatFaction.Miscellaneous;
                        Miscellaneous.Add(npc);
                    }
                }
                else
                {
                    c.CombatFaction = Enums.CombatFaction.PlayerCharacter;
                    Players.Add(c);
                }
            }
        }

        public void InitiativeOrder()
        {
            Combatants = [.. Combatants.OrderByDescending(c => c.Initiative)];
        }
        public static CombatWorkflow CreateCombat(Character attacker, Character enemy)
        {
            CombatWorkflow combat = new();
            GameState.Instance.Combats.Add(combat);
            combat.CombatInitialization(attacker, enemy);
            return combat;

        }
        public Weapon? selectedWeapon;
        public Spell? selectedSpell;


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
            if (ActiveCombatant != player)
            {
                player.WriteLine("It's not your turn!");
                return;
            }
            // Process any pre-process commands, if it matches, we'll execute it and return
            if (CommandManager.ProcessSpecificCommands(player, parameters, PreProcessCommands))
                return;


            CombatWorkflow? currentCombat = this;

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
                            case "1":
                                player.WriteLine($"What do you attack with?");
                                foreach (Weapon weapon in player.Inventory)
                                {
                                    player.WriteLine($"- {weapon.Name}");
                                }
                                CurrentStep = 2;
                                break;
                            case "spell":
                            case "cast":
                            case "2":
                            case "cast spell":
                                player.WriteLine($"Which spell do you want to cast?");
                                foreach (Spell spell in player.Spellbook)
                                {
                                    player.WriteLine($"- {spell.Name}");
                                }
                                CurrentStep = 3;
                                break;
                            case "inventory":
                            case "3":
                            case "items":
                                player.WriteLine("You open your inventory:");
                                // Rylan - See my note in ChooseItem about filtering by type.
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
                            case "4":
                            case "run":
                                player.WriteLine("You attempt to flee from combat!");
                                // TODO Player.FleeCombat(player, currentCombat);
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
                    ChooseItem(player, parameters);
                    break;
                case 5:
                    // targeting phase for attack

                    TargetWeapon(player, parameters);
                    break;
                case 6:
                    // targeting phase for spell
                    TargetSpell(player, parameters);
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

