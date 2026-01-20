
using RPGFramework.Display;
using RPGFramework.Interfaces;

namespace RPGFramework.Commands
{
    internal class AdminCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return
            [
                new AnnounceCommand(),
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
