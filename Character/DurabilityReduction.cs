using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;

namespace RPGFramework.Character
{
    internal class DurabilityReduction
    {
        public void ReduceDurabilityArmor(Armor ca, int amout)
        {
            ca.CurrentDurability -= amout;
        }
        public void ReduceDurabilityWeapon(Armor cw, int amout)
        {
            cw.CurrentDurability -= amout;
        }
    }
}
