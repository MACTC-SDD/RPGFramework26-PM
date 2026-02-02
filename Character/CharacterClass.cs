
namespace RPGFramework
{
    /// <summary>
    /// Type or class of character (e.g., Warrior, Mage, Thief).
    /// </summary>
    internal class CharacterClass
    {
        public string Name { get; set; } = "";
        public string Description { get;set; } = "";
        public int StrengthMod { get; set; } = 0;
        public int DexterityMod { get; set; } = 0;
        public int ConstitutionMod { get; set; } = 0;
        public int IntelligenceMod { get; set; } = 0;
        public int WisdomMod { get; set;} = 0;
        public int CharismaMod { get; set; } = 0;

        
    } 
}
    

