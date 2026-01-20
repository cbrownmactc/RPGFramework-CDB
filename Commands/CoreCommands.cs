
using RPGFramework.Interfaces;

namespace RPGFramework.Commands
{
    /// <summary>
    /// Provides access to the set of built-in core command implementations.
    /// </summary>
    /// <remarks>The <c>CoreCommands</c> class exposes static methods for retrieving all available core
    /// commands. These commands represent fundamental operations supported by the system </remarks>
    internal class CoreCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return
            [
                new AFKCommand(),
                new IpCommand(),
                new LookCommand(),
                new QuitCommand(),
                new SayCommand(),
                new TimeCommand(),
                // Add other core commands here as they are implemented
            ];
        }
    }

    #region AFKCommand Class
    internal class AFKCommand : ICommand
    {
        public string Name => "afk";
        public IEnumerable<string> Aliases => [];
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.IsAFK = !player.IsAFK;
                player.WriteLine($"You are now {(player.IsAFK ? "AFK" : "no longer AFK")}.");
                return true;
            }
            return false;
        }
    }
    #endregion

    #region IpCommand Class
    internal class IpCommand : ICommand
    {
        public string Name => "ip";
        public IEnumerable<string> Aliases => [];
        public bool Execute(Character character, List<string> parameters)
        {
            return Comm.SendToIfPlayer(character, $"Your IP address is {((Player)character).GetIPAddress()}");
        }
    }
    #endregion

    #region LookCommand Class
    internal class LookCommand : ICommand
    {
        public string Name => "look";
        public IEnumerable<string> Aliases => [ "l" ];
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                // For now, we'll ignore the command and just show the room description
                player.WriteLine($"{player.GetRoom().Description}");
                player.WriteLine("Exits:");
                foreach (var exit in player.GetRoom().GetExits())
                {
                    player.WriteLine($"{exit.Description} to the {exit.ExitDirection}");
                }
                return true;
            }
            return false;
        }
    }
    #endregion

    #region QuitCommand Class
    internal class QuitCommand : ICommand
    {
        public string Name => "quit";
        public IEnumerable<string> Aliases => ["exit"];

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.Logout();
                return true;
            }
            return false;
        }
    }
    #endregion

    #region SayCommand Class
    internal class SayCommand : ICommand
    {
        public string Name => "say";
        public IEnumerable<string> Aliases => [ "\"", "'" ];
        public bool Execute(Character character, List<string> parameters)
        {
            // If no message and it's a player, tell them to say something
            if (parameters.Count < 2)
            {
                return Comm.SendToIfPlayer(character, "Say what?");
            }

            Comm.RoomSay(character.GetRoom(), parameters[1], character);
            return true;
        }
    }
    #endregion

    #region TimeCommand Class
    internal class TimeCommand : ICommand
    {
        public string Name => "time";
        public IEnumerable<string> Aliases => [];
        public bool Execute(Character character, List<string> parameters)
        {
            return Comm.SendToIfPlayer(character, $"The time is {GameState.Instance.GameDate:t}");
        }
    }
    #endregion


}
