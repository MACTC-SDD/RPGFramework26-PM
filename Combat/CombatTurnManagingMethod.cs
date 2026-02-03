using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RPGFramework.Workflows
{
    internal partial class CombatWorkflow : IWorkflow
    {
        public int ActiveFactions { get; private set; } = 0;

        /// <summary>
        /// Process a combat turn, removing dead combatants and counting active factions
        /// </summary>
        /// <returns>true if the battle is still ongoing, otherwise, false</returns>
        public bool Process()
        {
            if (ActiveCombatant == Combatants[0])
                RoundCounter++;
            BringOutYourDead();
            ActiveFactions = CountActiveFactions();

            if (IsCombatOver())
            {
                EndCombat();
                return false;
            }

            UpdatePlayerTurn();

            return true;
        }

        public void EndTurn()
        {
            int nextCombatantIndex = Combatants.IndexOf(ActiveCombatant) + 1;
            if (nextCombatantIndex >= Combatants.Count)
            {
                nextCombatantIndex = 0;
            }
            ActiveCombatant = Combatants[nextCombatantIndex];
        }

        private void UpdatePlayerTurn()
        {
            // at the start of combat assign first active combatant based on initiative order
            ActiveCombatant ??= Combatants[0];
            // run npc turn if npc, otherwise wait for 30 seconds to pass for player turns
            if (ActiveCombatant is NonPlayer npc)
            {
                ProcessStatusEffects(npc);
                NonPlayer.TakeTurn(npc, this);
                TurnTimer++;
            }
            else
            {
                if (PreviousActingCharacter != null)
                {

                    if (PreviousActingCharacter == ActiveCombatant)
                    {
                        // update timer for player turns if it is the same player as the last run of this task
                        if (TurnTimer == 1)
                            ProcessStatusEffects(ActiveCombatant);
                        TurnTimer++;
                        if (TurnTimer >= 30)
                        {
                            // end player turn if 30 seconds have passed
                            int indexOfNextCombatant = Combatants.IndexOf(ActiveCombatant) + 1;
                            if (indexOfNextCombatant > Combatants.Count - 1)
                                indexOfNextCombatant = 0;
                            ActiveCombatant = Combatants[indexOfNextCombatant];
                        }
                        else if (PreviousActingCharacter != ActiveCombatant)
                        {
                            // update so that new player gets full turn time
                            PreviousActingCharacter = ActiveCombatant;
                            TurnTimer = 1;
                        }
                    }

                }
            }

        }

        private bool IsCombatOver()
        {
            return (ActiveFactions <= 1 && Miscellaneous.Count <= 0) || (ActiveFactions <= 0 && Miscellaneous.Count <= 1);
        }

        public void EndCombat()
        {
            foreach (Character c in Combatants)
            {
                c.CurrentWorkflow = null;
                Combatants.Remove(c);
            }

            GameState.Instance.Combats.Remove(this);
        }

        private void BringOutYourDead()
        {
            foreach (Character c in Combatants)
            {
                if (!c.Alive)
                {
                    Combatants.Remove(c);
                }
            }
        }

        private int CountActiveFactions()
        {
            int af = 0;
            if (Elf.Count > 0)
                af++;
            if (Bandit.Count > 0)
                af++;
            if (Monster.Count > 0)
                af++;
            if (Construct.Count > 0)
                af++;
            if (Army.Count > 0)
                af++;

            return af;
        }
        #region StatusEffectMethods
        public void ProcessStatusEffects(Character c)
        {
            if (c.IsPetrified)
            {
                c.Petrified();
            }
            if (c.IsBleed)
            {
                c.Bleed();
            }
            if (c.IsBlind)
            { 
                c.Blinded();
            }
            if (c.IsBurn)
            {
                c.Burn();
            }
            if (c.IsDeafened)
            {
                c.Deafened();
            }
            if (c.IsFreightened)
            {
                c.Freightened();
            }
            if (c.IsGappled)
            { 
                c.Grappled();
            }
            if (c.IsParalyzed)
            {
                c.Paralyzed();
            }
            if (c.IsPoisoned)
            { 
                c.Poisoned();
            }
            if (c.IsStun)
            { 
                c.Stun();
            }
            if (c.IsUnconcious)
            {
                c.Unconscious();
            }
            if (c.IsIncapacitated)
            {
                c.Incapacitated();
            }
        }
        /*private void ClearStatusEffects(Character c)
        {
            c.IsBleed = false;
            c.IsBlind = false;
            c.IsBurn = false;
            c.IsDeafened = false;
            c.IsFreightened = false;
            c.IsGappled = false;
            c.IsIncapacitated = false;
            c.IsParalyzed = false;
            c.IsPoisoned = false;
            c.IsPetrified = false;
            c.IsStun = false;
            c.IsUnconcious = false;
        }*/
        
        #endregion
    }
}
