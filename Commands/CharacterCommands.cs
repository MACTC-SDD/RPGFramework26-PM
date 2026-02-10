using RPGFramework;
using RPGFramework.Commands;
using RPGFramework.Enums;
using RPGFramework.Geography;
namespace RPGFramework.Commands
{
    internal class CharacterCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return
            [
                new CCCommand(),
                // Add more test commands here as needed
            ];
        }
    }


    internal class CCCommand : ICommand
    {
        // This is the command a player would type to execute this command
        public string Name => "cc";

        // These are the aliases that can also be used to execute this command. This can be empty.
        public IEnumerable<string> Aliases => [];
        public string Help => "";

        // What will happen when the command is executed
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            if (character.Class == null)
            {
                player.WriteLine("You have not selected a character class yet.");
                return true;
            }
            player.WriteLine($"══════════ ⋆★⋆ ═════════════");
            player.WriteLine($"✦Your Character Class Is: {character.Class.Name}✦");
            player.WriteLine($"Class Description: {character.Class.Description}");
            player.WriteLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            player.WriteLine($"✦Your Race Is: {player.Race.Name}✦");
            player.WriteLine($"Race Description: {player.Race.Description}");
            player.WriteLine($"════════════════════════════");
            player.WriteLine($"°。°。°。°。°。°。°。°。°。°。");
            player.WriteLine($"══════════ ⋆★⋆ ═════════════");
            player.WriteLine("        Base Stats ");
            player.WriteLine($"════════════════════════════");
            player.WriteLine($"       Strength: {player.Strength}");
            player.WriteLine($"       Dexterity: {player.Dexterity}");
            player.WriteLine($"       Constitution: {player.Constitution}");
            player.WriteLine($"       Inteligence: {player.Intelligence}");
            player.WriteLine($"       Wisdom: {player.Wisdom}");
            player.WriteLine($"       Charisma: {player.Charisma}");
            player.WriteLine($"════════════════════════════");
            player.WriteLine($"°。°。°。°。°。°。°。°。°。°。");
            player.WriteLine($"══════════ ⋆★⋆ ═════════════");
            player.WriteLine("        Modifiers ");
            player.WriteLine($"════════════════════════════");
            player.WriteLine($"   Strength Modifier:{character.Class.StrengthMod}");
            player.WriteLine($"   Dexterity Modifier:{character.Class.DexterityMod}");
            player.WriteLine($"   Constitution Modifier:{character.Class.ConstitutionMod}");
            player.WriteLine($"   Inteligence Modifier:{character.Class.IntelligenceMod}");
            player.WriteLine($"   Wisdom Modifier:{character.Class.WisdomMod}");
            player.WriteLine($"   Charisma Modifier:{character.Class.CharismaMod}");
            player.WriteLine($"════════════════════════════");
            return true;
        }
    }

   
}
