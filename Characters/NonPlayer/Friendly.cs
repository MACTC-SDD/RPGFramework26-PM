using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework
{
    internal class Friendly : NonPlayer
    {
        public int FriendlyId { get; set; }
        public bool Hostile { get; set; } = false;
    }
}
