using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Core
{
    internal class HelpEntry
    {
        public required string Topic { get; set; }
        public required string Category { get; set; }
        public required string Content { get; set; }
    }
}
