using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework
{
    internal class Enemy : NonPlayer
    {
        public int EnemyId { get; set; }
        public bool Hostile { set; get; } = true;
    }
}
