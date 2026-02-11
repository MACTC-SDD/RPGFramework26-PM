
using RPGFramework.Display;
using RPGFramework.Enums;
using RPGFramework.Items;
using RPGFramework.Geography;
using Spectre.Console;
using Spectre.Console.Rendering;
using RPGFramework.Workflows;
using System.Text.Json.Serialization;

namespace RPGFramework
{
    /// <summary>
    /// Represents a base character in the game, providing common properties and functionality for players, non-player
    /// characters (NPCs), and other entities.
    /// </summary>
    /// <remarks>This abstract class defines shared attributes such as health, level, skills, equipment, and
    /// location for all character types. Derived classes should implement specific behaviors and additional properties
    /// as needed. The class enforces valid ranges for skill attributes and manages health and alive status. Instances
    /// of this class are not created directly; instead, use a concrete subclass representing a specific character
    /// type.</remarks>
    internal abstract partial class Character
    {
        #region --- Properties ---
        public bool Alive { get; set; } = true;
        public int AreaId { get; set; } = 0;
        public double DamageResistance { get; set; } = 1.0;
        public CombatFaction CombatFaction { get; set; }
        public string Description { get; set; } = "";
        public string Element { get; set; } = string.Empty;
        public int Gold { get; set; } = 0;
        public int Health { get; set; } = 100;
        public bool IsEngaged { get; protected set; } = false;
        [JsonInclude] public Inventory BackPack { get; protected set; } = new Inventory();
        [JsonInclude] public int Level { get; protected set; } = 1;
        public int LocationId { get; set; } = 0;
        [JsonInclude] public int MaxHealth { get; set; } = 100;
        public string Name { get; set; } = "";
        [JsonInclude] public int XP { get; protected set; } = 0;
        public Armor? EquippedArmor { get; set; }
        public CharacterClass? Class { get; set; } = new();
        public Weapon PrimaryWeapon { get; set; }
        public int StatPoints { get; set; } = 0;
        public int Initiative { get; set; }
        public StatusCondition StatusConditon = StatusCondition.None;
        public string Title { get; set; } = "";
        public bool InCombat { get; set; } = false;
        public double MaxCarryWeight { get; private set; } = 150;
        public int MaxMana { get; set; } = 100;
        public int Mana { get; set; } = 100;
        public Race? Race { get; set; } = new();
        public string MostRecentSaveResult { get; set; } = "failed";
        public List<Spell> Spellbook { get; set; } = [];
        public List<Consumable> GetConsumables()
        {
            return [.. BackPack.Items.OfType<Consumable>()];
        }
        #endregion

        #region --- Skill Attributes --- (0-20)
        public int Strength { get;  set { field = Math.Clamp(value, 0, 20); } } = 0;
        public int Dexterity { get;  set { field = Math.Clamp(value, 0, 20); } } = 0;
        public int Constitution { get;  set { field = Math.Clamp(value, 0, 20); } } = 0;
        public int Intelligence { get;  set { field = Math.Clamp(value, 0, 20); } } = 0;
        public int Wisdom { get;  set { field = Math.Clamp(value, 0, 20); } } = 0;
        public int Charisma { get;  set { field = Math.Clamp(value, 0, 20); } } = 0;
        #endregion

        [JsonIgnore]
        public IWorkflow? CurrentWorkflow { get; set; } = null;

        public Character()
        {
            Health = MaxHealth;
            Weapon w = new() 
              { MaxDamage = 4, MaxDice = 1, Description = "A fist", Name = "Fist", Value = 0, Weight = 0, WeaponType = WeaponType.Hands };
            PrimaryWeapon = w;

            if (Class != null)
            {
                GameState.Instance.CCCatalog.TryGetValue(Class.Name, out CharacterClass? c);
                if (c != null)
                    Class = c;
            }
        }

        #region Consider Method
        // Consider another character and return a string describing how they compare
        public string Consider(Character targetCharacter)
        {
            string output;
            int levelDifference = targetCharacter.Level - this.Level;

            switch (levelDifference)
            {
                case int n when (n >= 5):
                    output = $"{targetCharacter.Name} looks like a formidable opponent.";
                    break;
                case int n when (n >= 2):
                    output = $"{targetCharacter.Name} looks slightly stronger than you.";
                    break;
                case int n when (n >= -1 && n <= 1):
                    output = $"{targetCharacter.Name} appears to be evenly matched with you.";
                    break;
                case int n when (n >= -4):
                    output = $"{targetCharacter.Name} seems a bit weaker than you.";
                    break;
                default:
                    output = $"{targetCharacter.Name} looks like an easy target.";
                    break;
            }

            return output;
        }
        #endregion

        // Things to do when a character engages in combat. This may be overridden by subclasses.
        public void EngageCombat(bool inCombat)
        {
            IsEngaged = inCombat;
            InCombat = inCombat;
        }

        /// <summary>
        /// Get Room object of current location.
        /// </summary>
        /// <returns></returns>
        public Room GetRoom()
        {
            return GameState.Instance.Areas[AreaId].Rooms[LocationId];
        }

        public Area GetArea()
        {
            return GameState.Instance.Areas[AreaId];
        }

        // Set Health to a specific value
        public void SetHealth(int health)
        {
            // Doesn't make sense if player is dead
            if (Alive == false)
                return;


            // Can't have health < 0
            if (health < 0)
            {
                health = 0;
            }
            // Can't have health > MaxHealth
            if (health > MaxHealth)
                health = MaxHealth;

            Health = health;

            // If Health == 0, Make Unalive
            if (Health == 0)
            {
                Alive = false;
            }
        }

        // Set Max Health to a specific value, use sparingly, mostly for creating characters
        public void SetMaxHealth(int maxHealth)
        {
            if (maxHealth < 1)
                maxHealth = 1;
            MaxHealth = maxHealth;
            // Ensure current health is not greater than new max health

            Health = MaxHealth;            
        }

        // Remove some amount from health
        public void TakeDamage(int damage)
        {
            SetHealth(Health - (int)Math.Ceiling((double)damage / DamageResistance));
        }

        // Add some amount to health
        public void Heal(int heal)
        {
            SetHealth(Health + heal - HealPenalty);
        }
        public void SetCarryCapacity()
        {
            MaxCarryWeight = Strength * 15;
        }

        public Item? FindItem(string itemName)
        {
            return BackPack.Items.Find(x => x.Name.Equals(itemName, StringComparison.CurrentCultureIgnoreCase));
        }

        public Item? FindItem(int itemId)
        {
            return BackPack.Items.Find(x => x.Id == itemId);
        }

        public Consumable? FindConsumable(string consumableName)
        {
            return BackPack.Items.OfType<Consumable>()
                .Where(x => x.Name.Equals(consumableName, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();
        }

        public Consumable? FindConsumable(int consumableId)
        {
            return BackPack.Items.OfType<Consumable>()
                .Where(x => x.Id == consumableId)
                .FirstOrDefault();
        }

        public IRenderable ShowSummary()
        { var table = new Table();
            table.AddColumn("Background");
            table.AddColumn("info");
            table.AddRow($"Name: {Name}", $"Gold: {Gold}");
            table.AddRow($"Class: {Class?.Name ?? "None"}", $"Weapon: {PrimaryWeapon.Name}");
            table.AddRow($"Health: {Health}", $"XP: {XP}");
            table.AddRow($"level: {Level}", $"Location: {LocationId}");

            string title = "Character Info";

            return RPGPanel.GetPanel(table, title);
        }
        
        // basic set up of max mana
        public void SetMaxMana( int maxMana)
        {
            if (maxMana < 100)
                maxMana = 100;
            MaxMana = maxMana;
            //mana is equal to maxmana
            Mana = MaxMana;
        }
        public void SetMana(int mana)
        {
            //mana cant be less than 0
            if (mana < 0)
            {
                Mana = 0;
            }
            //mana has to equal max mana
            if (mana > MaxMana)
            { 
                Mana = MaxMana;
            }
        }
    }
}
