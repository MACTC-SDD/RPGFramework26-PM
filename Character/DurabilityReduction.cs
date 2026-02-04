using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;

namespace RPGFramework
{
    internal partial class Character
    {
        public void ReduceDurabilityArmor(Armor ca, int amout)
        {
            ca.CurrentDurability -= amout;
        }
        public void ReduceDurabilityWeapon(Weapon cw, int amout)
        {
            cw.CurrentDurability -= amout;
        }
    }
}
