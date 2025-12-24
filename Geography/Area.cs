namespace RPGFramework.Geography
{
    internal class Area
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "Void Area";
        public string Description { get; set; } = "Start Area";

        public Dictionary<int, Exit> Exits { get; set; } = new();

        public Dictionary<int, Room> Rooms { get; set; } = new();
    }
}
