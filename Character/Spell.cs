using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework
{
    internal partial class Spell
    {
        public bool IsHeal { get; set; } = false;
        public int ManaCost { get; set; } = 5;
        public bool HasSave { get; set; } = false;
        public SavingThrowType SaveType { get; set; }
        public int MaxDice { get; set; }
        public int MaxDamage { get; set; }
        public List<string> Effects { get; set; } = [];
        public int RollDamageS(int dice, int damage, double mod)
        {
            int totalDamage = 0;

            for (int i = 0; i < dice; i++)
            {
                Random rand = new Random();
                int roll = rand.Next(1, damage);
                totalDamage += roll;
            }
            totalDamage = (int)Math.Ceiling(totalDamage * mod);
            return totalDamage;
        }
        public int SaveSpell(Character a, Spell s, Character e)
        {
            if (!CheckMana(s.ManaCost, a))
                return 0;
            a.Mana -= s.ManaCost;
            bool save = false;
            int finalDamage = 0;
            switch (s.SaveType)
            {
                case SavingThrowType.STR:
                    save = e.STRSavingThrow(10 + (int)Math.Ceiling(((double)a.Intelligence - 10) / 2), e);
                    break;
                case SavingThrowType.INT:
                    save = e.INTSavingThrow(10 + (int)Math.Ceiling(((double)a.Intelligence - 10) / 2), e);
                    break;
                case SavingThrowType.WIS:
                    save = e.WISSavingThrow(10 + (int)Math.Ceiling(((double)a.Intelligence - 10) / 2), e);
                    break;
                case SavingThrowType.DEX:
                    save = e.DEXSavingThrow(10 + (int)Math.Ceiling(((double)a.Intelligence - 10) / 2), e);
                    break;
                case SavingThrowType.CON:
                    save = e.CONSavingThrow(10 + (int)Math.Ceiling(((double)a.Intelligence - 10) / 2), e);
                    break;
                case SavingThrowType.CHA:
                    save = e.CHASavingThrow(10 + (int)Math.Ceiling(((double)a.Intelligence - 10) / 2), e);
                    break;
                default:
                    break;
            }
            if (save)
            {
                finalDamage = s.RollDamageS(s.MaxDice, s.MaxDamage, 0.5);
                s.HandleSpellEffects(e);
            }
            else
            {
                finalDamage = s.RollDamageS(s.MaxDice, s.MaxDamage, 1);
            }
                return 0;
        }

        public bool CheckMana(int amount, Character p)
        {
            if (p.Mana >= amount)
            {
                return true;
            }
            else
            {
                return false;
            } 
        }

        public void HandleSpellEffects(Character target)
        {
            foreach (string effect in Effects)
            {
                switch (effect)
                {
                    case "Burn":
                        target.IsBurn = true;
                        break;
                    case "Bleed":
                        target.IsBurn = true;
                        break;
                    case "Stun":
                        target.IsBurn = true;
                        break;
                    case "Blind":
                        target.IsBurn = true;
                        break;
                    case "Poisoned":
                        target.IsBurn = true;
                        break;
                    case "Freightened":
                        target.IsBurn = true;
                        break;
                    case "Deafened":
                        target.IsBurn = true;
                        break;
                    case "Incapacitated":
                        target.IsBurn = true;
                        break;
                    case "Paralyzed":
                        target.IsBurn = true;
                        break;
                    case "Petrified":
                        target.IsPetrified = true;
                        break;
                    case "Unconcious":
                        target.IsUnconcious = true;
                        break;
                    case "Death":
                        target.Health = 0;
                        target.Alive = false;
                        break;
                }
            }
        }
        public int HealSpell(Character a, Spell s)
        {
            if (!CheckMana(s.ManaCost, a))
                return 0;
            a.Mana -= s.ManaCost;
            int healAmount = s.RollDamageS(s.MaxDice, s.MaxDamage, 1);
            a.Heal(healAmount);
            if (s.Effects.Contains("Cure"))
            {
                ClearStatusConditions(a);
            }
            else if (s.Effects.Contains("GreaterHeal"))
            {
                a.IsBurn = false;
                a.IsBleed = false;
                a.IsPoisoned = false;
                a.IsBlind = false;
                a.IsDeafened = false;
            }
            else if (s.Effects.Contains("LesserHeal"))
            {
                a.IsBurn = false;
                a.IsBleed = false;
                Random rand = new Random();
                int bonusHeal = rand.Next(1, 4);
                if (bonusHeal == 1)
                    a.IsBlind = false;
                else if (bonusHeal == 2)
                    a.IsDeafened = false;
                else if (bonusHeal == 3)
                    a.IsPoisoned = false;
                else if (bonusHeal == 4)
                    a.Heal((int)Math.Ceiling((double)a.MaxHealth / 10));
            }
            else if (s.Effects.Contains("Heal"))
            {
                a.IsBleed = false;
                a.IsBurn = false;
            }
            return healAmount;
        }

        public void ClearStatusConditions(Character target)
        {
            target.IsBurn = false;
            target.IsBleed = false;
            target.IsStun = false;
            target.IsBlind = false;
            target.IsPoisoned = false;
            target.IsFreightened = false;
            target.IsDeafened = false;
            target.IsIncapacitated = false;
            target.IsParalyzed = false;
        }
    }
}
