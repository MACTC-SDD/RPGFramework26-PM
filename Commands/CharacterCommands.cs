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
            return new List<ICommand>
            {
                new CCCommand(),
                // Add more test commands here as needed
            };
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
            //CharacterClass c = new CharacterClass();
            //c.Name = character.Name;

            // A lot of times we want to make sure it's a Player issuing the command, but not always
            if (character is Player player)
            {
                player.WriteLine($"Your character class is: {character.Class.Name}");
                player.WriteLine($"Description: {character.Class.Description}");
                player.WriteLine($"Strength Modifier:{character.Class.StrengthMod}");
                player.WriteLine($"Dexterity Modifier:{character.Class.DexterityMod}");
                player.WriteLine($"Constitution Modifier:{character.Class.ConstitutionMod}");
                player.WriteLine($"Inteligence Modifier:{character.Class.IntelligenceMod}");
                player.WriteLine($"Wisdom Modifier:{character.Class.WisdomMod}");
                player.WriteLine($"Charisma Modifier:{character.Class.CharismaMod}");
            }

            // If the command failed to run for some reason, return false
            return true;
        }
    }

   
}
