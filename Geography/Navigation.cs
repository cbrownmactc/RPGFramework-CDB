using RPGFramework.Enums;

namespace RPGFramework.Geography
{
    /// <summary>
    /// Primarily respoonsible for handling move commands (n, e, s, w, u, d)
    /// </summary>
    internal class Navigation
    {

        /// <summary>
        /// Move the character in the specified direction if possible, otherwise, send error.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="direction"></param>
        public static void Move(Character character, Direction direction)
        {
            Room currentRoom = character.GetRoom();
            Exit? exit = currentRoom.GetExits().FirstOrDefault(e => e.ExitDirection == direction);

            // If invalid exit, send error message (if player)
            if (exit == null)
            {
                Comm.SendToIfPlayer(character, "You can't go that way.");
                return;
            }

            Room destinationRoom = GameState.Instance.Areas[character.AreaId].Rooms[exit.DestinationRoomId];

            currentRoom.LeaveRoom(character);
            destinationRoom.EnterRoom(character);
            
            character.AreaId = destinationRoom.AreaId;
            character.LocationId = exit.DestinationRoomId;
        }

        public static Direction GetOppositeDirection(Direction direction)
        {
            return direction switch
            {
                Direction.North => Direction.South,
                Direction.South => Direction.North,
                Direction.East => Direction.West,
                Direction.West => Direction.East,
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                _ => Direction.None,
            };
        }
    }
}
