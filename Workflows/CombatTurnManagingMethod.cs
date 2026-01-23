using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RPGFramework.Workflows
{
    internal partial class CombatWorkflow : IWorkflow
    {
        // replaced by combat manager task in gamestate, keeping for now, delete before merging w/ master
        public async Task CombatLoopRunner()
        {
            int ActiveFactions = 0;
            while (ActiveFactions > 1 && Miscellaneous.Count > 1)
            {
                foreach (Character c in Combatants)
                {
                    if (!c.Alive)
                    {
                        Combatants.Remove(c);
                    }
                }
                foreach (Character c in Combatants)
                {
                    ActiveCombatant = c;
                    if (c is Player p)
                    {
                        for (int i = 0; i < 30; i++)
                        {
                            // fx later, forgot how Mr. Brown suggested to make a turn timer
                            await Task.Delay(1000);
                        }
                    }
                    else
                    {
                        if (c is NonPlayer npc)
                            NonPlayer.TakeTurn(npc, this);
                    }
                }
                ActiveFactions = 0;
                if (Elf.Count > 0) 
                    ActiveFactions++;
                if (Bandit.Count > 0)
                    ActiveFactions++;
                if (Monster.Count > 0)
                    ActiveFactions++;
                if (Construct.Count > 0)
                    ActiveFactions++;
                if (Army.Count > 0)
                    ActiveFactions++;
            }
        }
    }
}
