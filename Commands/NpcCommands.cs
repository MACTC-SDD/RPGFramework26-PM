using System;
using System.Collections.Generic;
using System.Text;

namespace RPGFramework.Commands
{
    internal class NPCSummonCommand : ICommand
    {
        public string Name => "/summon";
        public IEnumerable<string> Aliases => Array.Empty<string>();
        public bool Execute(Character character, List<string> parameters)
        {
            return false;
        }

    }
}
