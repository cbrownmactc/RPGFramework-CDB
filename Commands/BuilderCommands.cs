
using RPGFramework.Enums;
using RPGFramework.Geography;
using RPGFramework.Interfaces;
using Spectre.Console;

namespace RPGFramework.Commands
{
    internal class BuilderCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return
            [
                new RoomBuilderCommand(),
                // Add more builder commands here as needed
            ];
        }
    }

    #region RoomBuilderCommand Class
    /// <summary>
    /// /room command for building and editing rooms.
    /// </summary>
    internal class RoomBuilderCommand : ICommand
    {
        public string Name => "/room";

        public IEnumerable<string> Aliases => [];

        #region Execute Method
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is not Player player)            
                return false;

            if (!Utility.CheckPermission(player, PlayerRole.Admin))
            {
                player.WriteLine("You do not have permission to do that.");
                return false;
            }

            if (parameters.Count < 2)
            {
                WriteUsage(player);
                return false;
            }

            switch (parameters[1].ToLower())
            {
                case "create":
                    return RoomCreate(player, parameters);                    
                case "set":
                    return RoomSet(player, parameters);
                case "delete":
                    break;
                case "list":
                    return RoomList(player, parameters);
                default:
                    WriteUsage(player);
                    break;
            }

            return true;
        }
        #endregion

        #region WriteUsage Method
        // TODO Should we make a Help string part of the ICommand interface?
        private static void WriteUsage(Player player)
        {
            player.WriteLine("Usage: ");
            player.WriteLine("/room description '<set room desc to this>'");
            player.WriteLine("/room name '<set room name to this>'");
            player.WriteLine("/room create '<name>' '<description>' <exit direction> '<exit description>'");
        }
        #endregion

        #region RoomCreate Method
        private static bool RoomCreate(Player player, List<string> parameters)
        {
            // 0: /room
            // 1: create
            // 2: name
            // 3: description
            // 4: exit direction
            // 5: exit description
            if (parameters.Count < 6)
            {
                player.WriteLine("Usage: /room create '<name>' '<description>' <exit direction> '<exit description>'");
                return false;
            }

            if (!Enum.TryParse(parameters[4], true, out Direction exitDirection))
            {
                player.WriteLine("Invalid exit direction.");
                return false;
            }

            try
            {
                Room room = Room.CreateRoom(player.AreaId, parameters[2], parameters[3]);

                player.GetRoom().AddExits(player, exitDirection, parameters[5], room);
                player.WriteLine("Room created.");
                return true;
            }
            catch (Exception ex)
            {
                player.WriteLine($"Error creating room: {ex.Message}");
                player.WriteLine(ex.StackTrace ?? "");
            }
            return false;
        }
        #endregion

        #region RoomList Method
        private static bool RoomList(Player player, List<string> _)       
        {
            var table = new Table();
            table.AddColumn("Room ID");
            table.AddColumn("Name");
            table.AddColumn("Description");

            foreach (var room in 
                GameState.Instance.Areas[player.GetRoom().AreaId].Rooms.Values.OrderBy(r => r.Id))
            {
                table.AddRow(room.Id.ToString(), room.Name, room.Description);
            }
            player.Write(table);
            return true;
        }
        #endregion

        #region RoomSet Method
        private static bool RoomSet(Player player, List<string> parameters)
        {
            if (parameters.Count < 4)
            {
                WriteUsage(player);
                return false;
            }

            switch (parameters[2].ToLower())
            {
                case "description":
                    player.GetRoom().Description = parameters[3];
                    return true;
                case "name":
                    player.GetRoom().Name = parameters[3];
                    return true;
                case "tags":
                    player.GetRoom().Tags = [.. parameters[3].Split(',').Select(t => t.Trim())];
                    return true;
                default:
                    WriteUsage(player);
                    return false;
            }
        }
        #endregion
    }
    #endregion
}

