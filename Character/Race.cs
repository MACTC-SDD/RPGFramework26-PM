using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework
{
    internal class Race
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int Ability { get; set; } = 0;
        public int StrengthIncr { get; set; } = 0;
        public int DexterityIncr { get; set; } = 0;
        public int ConstitutionIncr { get; set; } = 0;
        public int IntelligenceIncr { get; set; } = 0;
        public int WisdomIncr { get; set; } = 0;
        public int CharismaIncr { get; set; } = 0;

    }
}
