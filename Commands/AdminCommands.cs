
using RPGFramework.Display;
using RPGFramework.Enums;
using RPGFramework.Interfaces;
using RPGFramework.Workflows;

namespace RPGFramework.Commands
{
    internal class AdminCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return
            [
                new AnnounceCommand(),
                new ReloadSeedDataCommand(),
                new ShutdownCommand(),
                // Add more builder commands here as needed
            ];
        }
    }

    #region AnnounceCommand Class
    internal class AnnounceCommand : ICommand
    {
        public string Name => "announce";
        public IEnumerable<string> Aliases => [ "ann" ];
        public bool Execute(Character character, List<string> parameters)
        {
            Comm.Broadcast($"{DisplaySettings.AnnouncementColor}[[Announcement]]: [/][white]" + 
                $"{string.Join(' ', parameters.Skip(1))}[/]");
            return true;
        }
    }
    #endregion

    #region ReloadSeedDataCommand Class
    internal class ReloadSeedDataCommand : ICommand
    {
        public string Name => "/reloadseeddata";
        public IEnumerable<string> Aliases => [];
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)
                return false;

            if (Utility.CheckPermission(player, PlayerRole.Admin) == false)
            {
                player.WriteLine("You do not have permission to use this command.");
                return false;
            }

            player.CurrentWorkflow = new WorkflowReloadSeedData();
            player.WriteLine("Watch out, you're about to overwrite your data with the default seed files. If that's what you want, type YES!");
            return true;
        }
    }
    #endregion

    #region ShutdownCommand Class
    internal class ShutdownCommand : ICommand
    {
        public string Name => "shutdown";
        public IEnumerable<string> Aliases => [];
        public bool Execute(Character character, List<string> parameters)
        {
            Comm.Broadcast($"{DisplaySettings.AnnouncementColor}[[WARNING]]: [/][white]" +
                $"Server is shutting down. All data will be saved.[/]");

            GameState.Instance.Stop();
            return true;
        }
    }
    #endregion
}
