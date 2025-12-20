using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGFramework.Geography
{
    public class Area
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "Void Area";
        public string Description { get; set; } = "Start Area";

        public Dictionary<int, Exit> Exits { get; set; } = new();

        public Dictionary<int, Room> Rooms { get; set; } = new();
    }
}
