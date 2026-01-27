
using RPGFramework.Enums;
using RPGFramework.Geography;
using RPGFramework.Combat;

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
    internal abstract class Character
    {
        #region --- Properties ---
        public bool Alive { get; set; } = true;
        public int AreaId { get; set; } = 0;
        public string Description { get; set; } = "";
        public string Element { get; set; } = string.Empty;
        public int Gold { get; set; } = 0;
        public int Health { get; protected set; } = 0;
        public bool IsEngaged { get; protected set; } = false;
        public int Level { get; protected set; } = 1;
        public int LocationId { get; set; } = 0;
        public int MaxHealth { get; protected set; } = 0;
        public string Name { get; set; } = "";
        public int XP { get; protected set; } = 0;
        public CharacterClass Class { get; set; } = CharacterClass.None;
        public List<Armor> EquippedArmor { get; set; } = [];
        public Weapon PrimaryWeapon { get; set; }
        public int Initiative { get; set; }
        #endregion

        #region --- Skill Attributes --- (0-20)
        public int Strength { get;  set { field = Math.Clamp(value, 0, 20); } } = 0;
        public int Dexterity { get;  set { field = Math.Clamp(value, 0, 20); } } = 0;
        public int Constitution { get;  set { field = Math.Clamp(value, 0, 20); } } = 0;
        public int Intelligence { get;  set { field = Math.Clamp(value, 0, 20); } } = 0;
        public int Wisdom { get;  set { field = Math.Clamp(value, 0, 20); } } = 0;
        public int Charisma { get;  set { field = Math.Clamp(value, 0, 20); } } = 0;
        #endregion


        public Character()
        {
            Health = MaxHealth;
            Weapon w = new() 
              { Damage = 2, Description = "A fist", Name = "Fist", Value = 0, Weight = 0 };
            PrimaryWeapon = w;
        }

        // Things to do when a character engages in combat. This may be overridden by subclasses.
        public void EngageCombat(bool inCombat)
        {
            IsEngaged = inCombat;

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
                health = 0;           

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
            SetHealth(Health - damage);
        }

        // Add some amount to health
        public void Heal(int heal)
        {
            SetHealth(Health + heal);
        }

        
    }
}
