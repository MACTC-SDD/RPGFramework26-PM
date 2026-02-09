using System;
using System.Collections.Generic;
using System.Text;
using RPGFramework.Enums;

namespace RPGFramework
    // Written by Aidan-- this is mainly for spells but could be used for enviroment or something else
{
    internal class SavingThrows
    {
        public bool STRSavingThrow(int DC, Player Target)
        {
            Random rand = new Random();
            int Roll = rand.Next(1, 20);
            int RollMod = ((Target.Strength - 10) / 2);
            int totalRoll = Roll + RollMod;
            if (totalRoll >= DC)
            {
                Target.WriteLine("You Succeded your Strength Saving throw.");
                return true;
            }
            else
            {
                Target.WriteLine("You have failed your Strength Saving Throw.");
                return false;
            }
        }
        public bool INTSavingThrow(int DC, Player Target)
        {
            Random rand = new Random();
            int Roll = rand.Next(1, 20);
            int RollMod = ((Target.Intelligence - 10) / 2);
            int totalRoll = Roll + RollMod;
            if (totalRoll >= DC)
            {
                Target.WriteLine("You Succeded your Inteligence Saving throw.");
                return true;
            }
            else
            {
                Target.WriteLine("You have failed your Inteligence Saving Throw.");
                return false;
            }
        }
        public bool WISSavingThrow(int DC, Player Target)
        {
            Random rand = new Random();
            int Roll = rand.Next(1, 20);
            int RollMod = ((Target.Wisdom - 10) / 2);
            int totalRoll = Roll + RollMod;
            if (totalRoll >= DC)
            {
                Target.WriteLine("You Succeded your Wisdom Saving throw.");
                return true;
            }
            else
            {
                Target.WriteLine("You have failed your Wisdom Saving Throw.");
                return false;
            }
        }
        public bool CONSavingThrow(int DC, Player Target)
        {
            Random rand = new Random();
            int Roll = rand.Next(1, 20);
            int RollMod = ((Target.Constitution - 10) / 2);
            int totalRoll = Roll + RollMod;
            if (totalRoll >= DC)
            {
                Target.WriteLine("You Succeded your Constitution Saving throw.");
                return true;
            }
            else
            {
                Target.WriteLine("You have failed your Dextarity Saving Throw.");
                return false;
            }
        }
        public bool DEXSavingThrow(int DC, Player Target)
        {
            Random rand = new Random();
            int Roll = rand.Next(1, 20);
            int RollMod = ((Target.Dexterity - 10) / 2);
            int totalRoll = Roll + RollMod;
            if (totalRoll >= DC)
            {
                Target.WriteLine("You Succeded your Dextarity Saving throw.");
                return true;
            }
            else
            {
                Target.WriteLine("You have failed your Dextarity Saving Throw.");
                return false;
            }
        }
        public bool CHASavingThrow(int DC, Player Target)
        {
            Random rand = new Random();
            int Roll = rand.Next(1, 20);
            int RollMod = ((Target.Charisma - 10) / 2);
            int totalRoll = Roll + RollMod;
            if (totalRoll >= DC)
            {
                Target.WriteLine("You Succeded your Charisma Saving throw.");
                return true;
            }
            else
            {
                Target.WriteLine("You have failed your Charisma Saving Throw.");
                return false;
            }
        }
    }
}
