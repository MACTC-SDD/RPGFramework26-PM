using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework
{
    internal partial class Character
    {
        public bool IsPoisoned { get; set; } = false;
        public bool IsBleed { get; set; } = false;
        public bool IsBurn { get; set; } = false;
        public bool IsStun { get; set; } = false;
        public bool IsBlind { get; set; } = false;
        public bool IsCharmed { get; set; } = false;
        public bool IsDeafened { get; set; } = false;
        public bool IsFreightened { get; set; } = false;
        public bool IsGappled { get; set; } = false;
        public bool IsIncapacitated { get; set; } = false;
        public bool IsParalyzed { get; set; } = false;
        public bool IsPetrified { get; set; } = false;
        public bool IsUnconcious { get; set; } = false;
        public int CountPoisoned { get; set; } = 0;
        public int CountBleed { get; set; } = 0;
        public int CountBurn { get; set; } = 0;
        public int CountStun { get; set; } = 0;
        public int CountBlind { get; set; } = 0;
        public int CountCharmed { get; set; } = 0;
        public int CountDeafened { get; set; } = 0;
        public int CountFreightened { get; set; } = 0;
        public int CountGappled { get; set; } = 0;
        public int CountIncapacitated { get; set; } = 0;
        public int CountParalyzed { get; set; } = 0;
        public int CountPetrified { get; set; } = 0;
        public int CountUnconcious { get; set; } = 0;
        public int Disadvantage { get; set; } = 0;
        public int Advantage { get; set; } = 0;
        public int HealPenalty { get; set; } = 0;
        public int HitPenalty { get; set; } = 0;
    }
}
