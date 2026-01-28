namespace RPGFramework.Geography
{
    internal class Area
    {
        #region --- Properties ---
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "Void Area";
        public string Description { get; set; } = "Start Area";

        public Dictionary<int, Exit> Exits { get; set; } = new();

        public Dictionary<int, Room> Rooms { get; set; } = new();

        public string Weather { get; set; } = "clear skies";
        public void UpdateWeather()
        {
            // choose random from list(or enum), apply to area
            // repeat for every area (handled in gamestate task)
            int randomWeatherIndex = new Random().Next(0, weatherStates.Count - 1);
            string newWeather = weatherStates[randomWeatherIndex];
            // apply newWeather to area
            // decide on how weather effects things like combat, npcs, visibility, movement, etc.
            // figure out how to implement those effects later, probably within combat and npc methods
        }
        //placeholder weather states, await build teams final choices
        // maybe make an enum instead
        List<string> weatherStates = new List<string>()
        {
            "Sunny",
            "Cloudy",
            "Rainy",
            "Stormy",
            "Snowy",
            "Windy"
        };
        #endregion
    }
}
