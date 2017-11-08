using System.Collections.Generic;
using DotNetty.Transport.Channels;

namespace cardGamesServer {
    public class PlayerManager
    {
        private static List<Room> rooms = new List<Room>();
        public static PlayerManager instance { get; }= new PlayerManager();

        PlayerManager() {
            rooms.Add(new Room(0));
        }

        public void addPlayer(IChannel channel) {
            if (rooms[rooms.Count - 1].isFull())
                rooms.Add(new Room(rooms.Count));
            rooms[rooms.Count - 1].addPlayer(new Player(channel));
        }

        public void removePlayer(IChannel channel) {
            foreach (var room in rooms)
                if (room.removePlayer(channel))
                    break;
        }

        public Player findPlayer(IChannel channel) {
            foreach (var room in rooms)
                foreach (var player in room.players)
                    if (player.channel.Equals(channel))
                        return player;
            return null;
        }

        public Room findRoom(IChannel channel) {
            foreach (var room in rooms)
                foreach (var player in room.players)
                    if (player.channel.Equals(channel))
                        return room;
            return null;
        }

        public void removeRoom(Room room) {
            foreach (Player player in room.players)
                player.channel.CloseAsync();
            rooms.Remove(room);
        }
    }
}