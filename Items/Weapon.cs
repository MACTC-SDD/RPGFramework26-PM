using RPGFramework.Enums;
using Spectre.Console;

namespace RPGFramework
{
    internal class Weapon : Item
    {
        public double Damage { get; set; } = 0;
        public WeaponType WeaponType { get; set; }
        public bool longRange { get; set; } = false;
        public int cuDurability { get; set; } = 20;
        public int lrDurability { get; set; } = 25;
        public bool ammmoleft { get; set; } = true;
        public int range { get; set; } = 0;
        public double Speed { get; set; } = 0;
        // TODO
        // Add attack properties (damage, speed, etc.)
        // Implement attack methods
        // Maybe some kind of Weapon generator (random stats, etc.)

        public void SetWeaponsSpeed()
        {
            switch (this.WeaponType)
            {
                case WeaponType.Hands:
                    Speed = 1;
                    break;


            }
        }

        

    }
}
