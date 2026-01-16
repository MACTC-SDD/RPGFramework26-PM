using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Enums
{
    internal class NPCIdleActions
    {
        public enum Actions
        {
            WalkAround,
            PatrolArea,
            Attack,
            StandGuard,
            SitAndRest,
            TalkToOtherNPCs,
            PerformTask,
            Sleep,
            Wander,
            ObserveSurroundings
        }
        public enum ArmyActions
        {
            PatrolArea,
            StandGuard,
            ObserveSurroundings
        }
    }
}
