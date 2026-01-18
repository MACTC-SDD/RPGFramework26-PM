using RPGFramework.Commands;

namespace RPGFramework.Workflows
{
    internal interface IWorkflow
    {
        int CurrentStep { get; set; }
        string Description { get; }
        string Name { get; }
        List<ICommand> PreProcessCommands { get; set; }
        List<ICommand> PostProcessCommands { get; set; }
        Dictionary<string, object> WorkflowData { get; set; }

        void Execute(Player player, List<string> parameters);



    }
}
