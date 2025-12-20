using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGFramework.Geography;

namespace RPGFramework
{
    // Probably abstrctt
    // This is meant to hold all of the commmon elements for
    // players, NPCs, etc.
    public class Character
    {
        public bool Alive { get; set; } = true;
        public int AreaId { get; set; } = 0;
        public int Gold { get; set; } = 0;
        public int Health { get; set; } = 0;
        public int Level { get; set; } = 1;
        public int LocationId { get; set; } = 0;
        public int MaxHealth { get; set; } = 0;
        public string Name { get; set; } = "";
        public int XP { get; set; } = 0;

        // --- Skill Attributes ---
        public int Strength { get; set; } = 0;
        public int Dexterity { get; set; } = 0;
        public int Constitution { get; set; } = 0;
        public int Intelligence { get; set; } = 0;
        public int Wisdom { get; set; } = 0;
        public int Charisma { get; set; } = 0;

        // --- Custom objects, move these to the main attributes list later ---
        public CharacterClass Class { get; set; } = new CharacterClass();
        public List<Armor> EquippedArmor { get; set; } = new List<Armor>();
        public Weapon PrimaryWeapon { get; set; }

        public Character()
        {
            Health = MaxHealth;
            Weapon w = new Weapon() 
              { Damage = 2, Description = "A fist", Name = "Fist", Value = 0, Weight = 0 };
            PrimaryWeapon = w;
        }

        /// <summary>
        /// Get Room object of current location.
        /// </summary>
        /// <returns></returns>
        public Room GetRoom()
        {
            return GameState.Instance.Areas[AreaId].Rooms[LocationId];
        }

        // Set Health to a specific value
        public void SetHealth(int health)
        {
            // Doesn't make sense if player is dead
            if (Alive == false)
            {
                return;
            }

            // Can't have health < 0
            if (health < 0)
            {
                health = 0;
            }

            // Can't have health > MaxHealth
            if (health > MaxHealth)
            {
                health = MaxHealth;
            }

            Health = health;

            // If Health == 0, Make Unalive
            if (Health == 0)
            {
                Alive = false;
            }
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
