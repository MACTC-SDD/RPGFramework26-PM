namespace RPGFramework.Commands
{
    internal class TestCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return
            [
                new TestItemSizeCommand(),
                new ExampleCommand(),
                new MapTestCommand(),
                // Add more test commands here as needed
            ];
        }
    }

    // To add a command, create a new ICommand class for it like the example below,
    // then add it to the list in GetAllCommands
    //   MAKE SURE you are adding your acommand and editing GetAllCommands in the correct file!

    /// <summary>
    /// You can use this as a template for creating new commands.
    /// Feel free to copy/paste and modify class name, Name, Aliases, and Execute as needed.
    /// </summary>
    internal class ExampleCommand : ICommand
    {
        // This is the command a player would type to execute this command
        public string Name => "example";

        // These are the aliases that can also be used to execute this command. This can be empty.
        public IEnumerable<string> Aliases => [];
        public string Help => "";

        // What will happen when the command is executed
        public bool Execute(Character character, List<string> parameters)
        { 
            // A lot of times we want to make sure it's a Player issuing the command, but not always
            if (character is not Player player)
            {
                return false;               
            }

            player.WriteLine("This is an example command.");

            // If the command failed to run for some reason, return false
            return true;
        }
    }

    internal class MapTestCommand : ICommand
    {
        // This is the cnd a player would type to execute this command
        public string Name => "mt";

        // These are the aliases that can also be used to execute this command. This can be empty.
        public IEnumerable<string> Aliases => [];
        public string Help => "";

        // What will happen when the command is executed
        public bool Execute(Character character, List<string> parameters)
        {
            // A lot of times we want to make sure it's a Player issuing the command, but not always
            if (character is not Player player)
            {
                return false;
            }

            GameState.Instance.ItemCatalog.TryGetValue("compass", out Item? i);
            Item? i2 = Utility.Clone(i);
            player.BackPack.Items.Add(i2);

            player.WriteLine("Added compass.");

            // If the command failed to run for some reason, return false
            return true;
        }
    }

    /// <summary>
    /// Measures the memory usage of creating a large number of <see cref="Item"/> instances.
    /// </summary>
    /// <remarks>This command is intended for diagnostic or testing purposes to estimate the memory footprint
    /// of <see cref="Item"/> objects. When executed, it creates 100,000 <see cref="Item"/> instances, calculates the
    /// total memory used, and outputs the results to the player if the character is a player.</remarks>
    internal class TestItemSizeCommand : ICommand
    {
        public string Name => "testitemsize";
        public IEnumerable<string> Aliases => [];
        public string Help => "";
        public bool Execute(Character character, List<string> parameters)
        {
            long startMem = GC.GetTotalMemory(true);
            List<Item> items = [];
            for (int i = 0; i < 100000; i++)
            {
                Item item = new()
                {
                    Id = i,
                    Name = "Item " + i,
                    Description = "This is item number " + i,
                    DisplayText = "You see item number " + i,
                    Level = i % 10,
                    Value = (i % 100) * 1.5,
                    Weight = (i % 50) * 0.75,
                    IsDroppable = true,
                    IsGettable = true,
                };
                items.Add(item);
            }
            long endMem = GC.GetTotalMemory(true);

            if (character is Player player)
            {
                player.WriteLine($"Created {items.Count} items.");
                player.WriteLine($"Memory used: {endMem - startMem} bytes.");
                player.WriteLine($"Average per item: {(endMem - startMem) / (double)items.Count} bytes.");
            }
            return true;
        }
    }
}
