
namespace RPGFramework
{
    /// <summary>
    /// Type or class of character (e.g., Warrior, Mage, Thief).
    /// </summary>
    internal class CharacterClass
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int StrengthMod { get; private set; } = 0;
        public int DexterityMod { get; private set; } = 0;
        public int ConstitutionMod { get; private set; } = 0;
        public int IntelligenceMod { get; private set; } = 0;
        public int WisdomMod { get; private set;} = 0;
        public int CharismaMod { get; private set; } = 0;

        
    } 
}
    

