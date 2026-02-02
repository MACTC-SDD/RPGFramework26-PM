using RPGFramework.Commands;
using RPGFramework.Geography;
using RPGFramework.Enums;

namespace RPGFramework.Workflows
{
    internal class WorkflowAreaTravelConfirm : IWorkflow
    {
        public int CurrentStep { get; set; } = 0;
        public string Description => "Confirms player intent to travel between areas.";
        public string Name => "Area Travel Confirmation";
        public List<ICommand> PreProcessCommands { get; private set; } = [];
        public List<ICommand> PostProcessCommands { get; private set; } = [];

        public Dictionary<string, object> WorkflowData { get; set; } = new Dictionary<string, object>();

        private readonly int _exitAreaId;
        private readonly int _exitId;

        public WorkflowAreaTravelConfirm(int exitAreaId, int exitId)
        {
            _exitAreaId = exitAreaId;
            _exitId = exitId;
        }

        public void Execute(Player player, List<string> parameters)
        {
            // Expect the player to type YES (or y) to confirm.
            if (parameters == null || parameters.Count == 0)
            {
                player.WriteLine("Type YES to confirm travel to the other area. This will be irreversible.");
                return;
            }

            string response = parameters[0].Trim().ToLowerInvariant();
            if (response == "yes" || response == "y" || response == "confirm")
            {
                // Find the exit
                if (!GameState.Instance.Areas.TryGetValue(_exitAreaId, out Area? area)
                    || !area.Exits.TryGetValue(_exitId, out Exit? exit))
                {
                    player.WriteLine("Could not locate the exit. Travel aborted.");
                    player.CurrentWorkflow = null;
                    return;
                }

                // Perform the move and remove the return exit so player cannot go back.
                Navigation.PerformMoveDirect(player, exit, removeReturnExit: true);

                player.WriteLine($"You travel to Area {exit.DestinationAreaId} Room {exit.DestinationRoomId}.");
                player.CurrentWorkflow = null;
                return;
            }
            else
            {
                player.WriteLine("Travel cancelled.");
                player.CurrentWorkflow = null;
                return;
            }
        }
    }
}