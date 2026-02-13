using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework
{
    internal partial class Spell
    {
        public void SpellConstructor()
        {
            Spell Fireball = new Spell
            {
                Name = "Fireball",
                ManaCost = 20,
                HasSave = true,
                SaveType = SavingThrowType.DEX,
                MaxDice = 10,
                MaxDamage = 6,
                Effects = new List<string> { "Burn" }
            };
            GameState.Instance.SpellCatalog.Add(Fireball.Name, Fireball);
            Spell FireBolt = new Spell
            {
                Name = "Fire Bolt",
                ManaCost = 5,
                HasSave = false,
                SaveType = SavingThrowType.None,
                MaxDice = 2,
                MaxDamage = 10,
                Effects = new List<string> { "Burn" }
            };
            GameState.Instance.SpellCatalog.Add(FireBolt.Name, FireBolt);
            Spell PowerWordKill = new Spell
            {
                Name = "Power Word: Kill",
                ManaCost = 90,
                HasSave = true,
                SaveType = SavingThrowType.CON,
                MaxDice = 0,
                MaxDamage = 0,
                Effects = new List<string> { "Death" }
            };
            GameState.Instance.SpellCatalog.Add(PowerWordKill.Name, PowerWordKill);
            Spell ChillTouch = new Spell
            {
                Name = "Chill Touch",
                ManaCost = 5,
                HasSave = true,
                SaveType = SavingThrowType.STR,
                MaxDice = 2,
                MaxDamage = 12,
                Effects = new List<string> { }
            };
            GameState.Instance.SpellCatalog.Add(ChillTouch.Name, ChillTouch);
            Spell InflictWounds = new Spell
            {
                Name = "Inflict Wounds",
                ManaCost = 10,
                HasSave = true,
                SaveType = SavingThrowType.STR,
                MaxDice = 4,
                MaxDamage = 8,
                Effects = new List<string> { "Bleed" }
            };
            GameState.Instance.SpellCatalog.Add(InflictWounds.Name, InflictWounds);
            Spell TimeRavage = new Spell
            {
                Name = "Time Ravage",
                ManaCost = 65,
                HasSave = true,
                SaveType = SavingThrowType.CON,
                MaxDice = 12,
                MaxDamage = 12,
                Effects = new List<string> { "Freightened", "Incapacitated" }
            };
            GameState.Instance.SpellCatalog.Add(TimeRavage.Name, TimeRavage);
            Spell Sleep = new Spell
            {
                Name = "Sleep",
                ManaCost = 10,
                HasSave = true,
                SaveType = SavingThrowType.WIS,
                MaxDice = 0,
                MaxDamage = 0,
                Effects = new List<string> { "Unconcious" }
            };
            GameState.Instance.SpellCatalog.Add(Sleep.Name, Sleep);
            Spell FleshToStone = new Spell
            {
                Name = "Flesh to Stone",
                ManaCost = 30,
                HasSave = true,
                SaveType = SavingThrowType.CON,
                MaxDice = 1,
                MaxDamage = 6,
                Effects = new List<string> { "Petrified" }
            };
            GameState.Instance.SpellCatalog.Add(FleshToStone.Name, FleshToStone);
            Spell RayOfSickness = new Spell
            {
                Name = "Ray of Sickness",
                ManaCost = 15,
                HasSave = true,
                SaveType = SavingThrowType.CON,
                MaxDice = 2,
                MaxDamage = 6,
                Effects = new List<string> { "Freightened", "Deafened", "Poisoned" }
            };
            GameState.Instance.SpellCatalog.Add(RayOfSickness.Name, RayOfSickness);
            Spell Contagion = new Spell
            {
                Name = "Contagion",
                ManaCost = 15,
                HasSave = true,
                SaveType = SavingThrowType.CON,
                MaxDice = 2,
                MaxDamage = 6,
                Effects = new List<string> { "Poisoned", "Bleed" }
            };
            GameState.Instance.SpellCatalog.Add(Contagion.Name, Contagion);
            Spell CallOfApocalypse = new Spell
            {
                Name = "Call of Apocalypse",
                ManaCost = 150,
                HasSave = true,
                SaveType = SavingThrowType.CON,
                MaxDice = 8,
                MaxDamage = 10,
                Effects = new List<string> { "Poisoned", "Bleed", "Blinded", "Deafened", "Burn", "Paralyzed", "Unconcious" }
            };
            GameState.Instance.SpellCatalog.Add(CallOfApocalypse.Name, CallOfApocalypse);
            Spell Entangle = new Spell
            {
                Name = "Entangle",
                ManaCost = 15,
                HasSave = true,
                SaveType = SavingThrowType.STR,
                MaxDice = 4,
                MaxDamage = 8,
                Effects = new List<string> { "Grappled" }
            };
            GameState.Instance.SpellCatalog.Add(Entangle.Name, Entangle);
            Spell Fear = new Spell
            {
                Name = "Fear",
                ManaCost = 10,
                HasSave = true,
                SaveType = SavingThrowType.WIS,
                MaxDice = 4,
                MaxDamage = 8,
                Effects = new List<string> { "Freigthened", "Incapacitated" }
            };
            GameState.Instance.SpellCatalog.Add(Fear.Name, Fear);
            Spell IllusionaryDragon = new Spell
            {
                Name = "Illusionary Dragon",
                ManaCost = 20,
                HasSave = true,
                SaveType = SavingThrowType.WIS,
                MaxDice = 6,
                MaxDamage = 10,
                Effects = new List<string> { "Freightened", "Burn" }
            };
            GameState.Instance.SpellCatalog.Add(IllusionaryDragon.Name, IllusionaryDragon);
            Spell MagicMissile = new Spell
            {
                Name = "Magic Missile",
                ManaCost = 0,
                HasSave = false,
                SaveType = SavingThrowType.None,
                MaxDice = 2,
                MaxDamage = 6,
                Effects = new List<string> { }
            };
            GameState.Instance.SpellCatalog.Add(MagicMissile.Name, MagicMissile);
            Spell WitchBolt = new Spell
            {
                Name = "Witch Bolt",
                ManaCost = 5,
                HasSave = false,
                SaveType = SavingThrowType.None,
                MaxDice = 2,
                MaxDamage = 10,
                Effects = new List<string> { }
            };
            GameState.Instance.SpellCatalog.Add(WitchBolt.Name, WitchBolt);
            Spell CauseFear = new Spell
            {
                Name = "Cause Fear",
                ManaCost = 5,
                HasSave = true,
                SaveType = SavingThrowType.WIS,
                MaxDice = 2,
                MaxDamage = 6,
                Effects = new List<string> { "Freightened" }
            };
            GameState.Instance.SpellCatalog.Add(CauseFear.Name, CauseFear);
            Spell EldritchBlast = new Spell
            {
                Name = "Eldritch Blast",
                ManaCost = 0,
                HasSave = false,
                SaveType = SavingThrowType.None,
                MaxDice = 1,
                MaxDamage = 10,
                Effects = new List<string> { }
            };
            GameState.Instance.SpellCatalog.Add(EldritchBlast.Name, EldritchBlast);
            Spell ShockingGrasp = new Spell
            {
                Name = "Shocking Grasp",
                ManaCost = 5,
                HasSave = true,
                SaveType = SavingThrowType.WIS,
                MaxDice = 2,
                MaxDamage = 12,
                Effects = new List<string> { }
            };
            GameState.Instance.SpellCatalog.Add(ShockingGrasp.Name, ShockingGrasp);
            Spell PowerWordStun = new Spell
            {
                Name = "Power Word: Stun",
                ManaCost = 55,
                HasSave = true,
                SaveType = SavingThrowType.CON,
                MaxDice = 5,
                MaxDamage = 8,
                Effects = new List<string> { "Stun" }
            };
            GameState.Instance.SpellCatalog.Add(PowerWordStun.Name, PowerWordStun);
            Spell PowerWordPain = new Spell
            {
                Name = "Power Word: Pain",
                ManaCost = 25,
                HasSave = true,
                SaveType = SavingThrowType.CON,
                MaxDice = 4,
                MaxDamage = 8,
                Effects = new List<string> { "Bleed" }
            };
            GameState.Instance.SpellCatalog.Add(PowerWordPain.Name, PowerWordPain);
            Spell WrathOfNature = new Spell
            {
                Name = "Wrath of Nature",
                ManaCost = 105,
                HasSave = true,
                SaveType = SavingThrowType.CON,
                MaxDice = 8,
                MaxDamage = 12,
                Effects = new List<string> { "Poisoned", "Bleed", "Blinded", "Deafened", "Burn", "Grappled" }
            };
            GameState.Instance.SpellCatalog.Add(WrathOfNature.Name, WrathOfNature);
            Spell Disintegrate = new Spell
            {
                Name = "Disintegrate",
                ManaCost = 50,
                HasSave = true,
                SaveType = SavingThrowType.DEX,
                MaxDice = 8,
                MaxDamage = 12,
                Effects = new List<string> { "Burn" }
            };
            GameState.Instance.SpellCatalog.Add(Disintegrate.Name, Disintegrate);
            Spell BlindnessDeafness = new Spell
            {
                Name = "Blindness/Deafness",
                ManaCost = 15,
                HasSave = true,
                SaveType = SavingThrowType.CON,
                MaxDice = 2,
                MaxDamage = 6,
                Effects = new List<string> { "Blinded", "Deafened" }
            };
            GameState.Instance.SpellCatalog.Add(BlindnessDeafness.Name, BlindnessDeafness);
            Spell Darkness = new Spell
            {
                Name = "Darkness",
                ManaCost = 15,
                HasSave = false,
                SaveType = SavingThrowType.None,
                MaxDice = 4,
                MaxDamage = 8,
                Effects = new List<string> { "Blinded" }
            };
            GameState.Instance.SpellCatalog.Add(Darkness.Name, Darkness);
            Spell HoldMonster = new Spell
            {
                Name = "Hold Monster",
                ManaCost = 70,
                HasSave = true,
                SaveType = SavingThrowType.CON,
                MaxDice = 5,
                MaxDamage = 8,
                Effects = new List<string> { "Paralyzed" }
            };
            GameState.Instance.SpellCatalog.Add(HoldMonster.Name, HoldMonster);
            Spell PowerWordHeal = new Spell
            {
                Name = "Power Word: Heal",
                ManaCost = 65,
                IsHeal = true,
                HasSave = false,
                SaveType = SavingThrowType.None,
                MaxDice = 8,
                MaxDamage = 10,
                Effects = new List<string> { "Cure" }
            };
            GameState.Instance.SpellCatalog.Add(PowerWordHeal.Name, PowerWordHeal);
            Spell CureWounds = new Spell
            {
                Name = "Cure Wounds",
                ManaCost = 5,
                IsHeal = true,
                HasSave = false,
                SaveType = SavingThrowType.None,
                MaxDice = 2,
                MaxDamage = 8,
                Effects = new List<string> { "Heal" }
            };
            GameState.Instance.SpellCatalog.Add(CureWounds.Name, CureWounds);
            Spell LesserRestoration = new Spell
            {
                Name = "Lesser Restoration",
                ManaCost = 15,
                IsHeal = true,
                HasSave = false,
                SaveType = SavingThrowType.None,
                MaxDice = 4,
                MaxDamage = 8,
                Effects = new List<string> { "LesserHeal" }
            };
            GameState.Instance.SpellCatalog.Add(LesserRestoration.Name, LesserRestoration);
            Spell GreaterRestoration = new Spell
            {
                Name = "Greater Restoration",
                ManaCost = 30,
                IsHeal = true,
                HasSave = false,
                SaveType = SavingThrowType.None,
                MaxDice = 6,
                MaxDamage = 10,
                Effects = new List<string> { "GreaterHeal" }
            };
            GameState.Instance.SpellCatalog.Add(GreaterRestoration.Name, GreaterRestoration);
            Spell SmiteOfDivinity = new Spell
            {
                Name = "Smite of Divinity",
                ManaCost = 30,
                HasSave = false,
                SaveType = SavingThrowType.None,
                MaxDice = 5,
                MaxDamage = 8,
                Effects = new List<string> { "Burn" }
            };
            GameState.Instance.SpellCatalog.Add(SmiteOfDivinity.Name, SmiteOfDivinity);
            Spell BeamOfPurity = new Spell
            {
                Name = "Beam of Purity",
                ManaCost = 45,
                HasSave = true,
                SaveType = SavingThrowType.DEX,
                MaxDice = 8,
                MaxDamage = 10,
                Effects = new List<string> { "Burn", "Blinded" }
            };
            GameState.Instance.SpellCatalog.Add(BeamOfPurity.Name, BeamOfPurity);
            Spell EternalRadiance = new Spell
            {
                Name = "Eternal Radiance",
                ManaCost = 85,
                HasSave = true,
                SaveType = SavingThrowType.CON,
                MaxDice = 12,
                MaxDamage = 12,
                Effects = new List<string> { "Burn", "Blinded", "Deafened", "Paralyzed" }
            };
            GameState.Instance.SpellCatalog.Add(EternalRadiance.Name, EternalRadiance);
            Spell UnlimitedVoid = new Spell
            {
                Name = "Unlimited Void",
                ManaCost = 100,
                HasSave = true,
                SaveType = SavingThrowType.CON,
                MaxDice = 12,
                MaxDamage = 12,
                Effects = new List<string> { "Freightened", "Paralyzed", "Unconcious", "Blinded", "Deafened" }
            };
            GameState.Instance.SpellCatalog.Add(UnlimitedVoid.Name, UnlimitedVoid);
            /*Fire Bolt
            Power Word: Kill
            Chill Touch
            Inflict Wounds
            Time Ravage
            Sleep
            Flesh to Stone
            Ray of Sickness
            Contagious
            Entangle
            Fear
            Illusionary Dragon
            Magic Missle
            Witch Bolt
            Cause Fear
            Fireball
            Eldritch Blast
            Shocking Grasp
            Power Word: Stun
            Power Word: Pain
            Wrath of Nature
            Disintegrate
            Blindness/Deafness
            Darkness
            Hold Monster
            */
        }
    }
}
