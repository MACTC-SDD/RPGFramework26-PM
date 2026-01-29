using System.Reflection;
using RPGFramework.Core;

namespace RPGFramework.Commands
{
    /// <summary>
    /// Provides methods for retrieving help entries from all available command types in the specified assembly.
    /// </summary>
    internal static class CommandHelpScanner
    {
        public static List<HelpEntry> GetAllHelpEntries(Assembly? assembly = null)
        {
            assembly ??= Assembly.GetExecutingAssembly();

            List<HelpEntry> entries = [];

            IEnumerable<Type> commandTypes = assembly
                .GetTypes()
                .Where(t =>
                typeof(ICommand).IsAssignableFrom(t)
                && !t.IsAbstract
                && !t.IsInterface);

            foreach (Type t in commandTypes.OrderBy(t => t.Name))
            {
                try
                {
                    ICommand? command = (ICommand?)Activator.CreateInstance(t);

                    if (command == null)
                        continue;

                    HelpEntry h = new()
                    { Category = "Commands", Name = command.Name, Content = command.Help };

                    if (h.Name.StartsWith("/"))
                        h.Category = "AdminCommands";

                    entries.Add(h);
                }
                catch (Exception ex)
                {
                    
                    entries.Add(new HelpEntry() 
                        { Category = "Unknown", Content = $"Help unavailable for command ({t.Name}): {ex.GetType().Name}", Name = t.Name });

                }
            }
            return entries;
        }
    }
}

