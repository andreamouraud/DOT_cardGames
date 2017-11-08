using System;
using System.Collections.Generic;
using System.Linq;
using DotNetty.Transport.Channels;

namespace cardGamesServer {
    public class Room {
        public List<Player> players { get; set; }
        public int id { get; }
        public Game game { get; }

        public Room(int id) {
            this.id = id;
            players = new List<Player>();
            game = new Game(this);
        }

        public bool isFull() {
            return players.Count >= 4;
        }
        
        private bool namesValid() {
            foreach (var player in players)
                if (player.name == null)
                    return false;
            return true;
        }

        public void initPlayer(Player player, string name) {
            player.name = name;
            if (checkPlayers())
                startGame();
        }

        private bool checkPlayers() {
            string message;

            if (!isFull())
                message = "Waiting for players : " + (4 - players.Count) + " remaining";
            else
                message = "Room is now full";
            writeToRoom(message, true, true);
            return isFull() && namesValid();
        }

        public void addPlayer(Player player) {
            players.Add(player);
            player.write("You have been added to Room " + id, true, true);
            writeToRoomBut(player, "A player joined the game", true, false);
        }

        public void writeToRoom(string message, bool newline, bool flush) {
            foreach (var player in players) 
                player.write(message, newline, flush);
        }


        public void writeToRoomBut(Player ignore, string message, bool newline, bool flush) {
            foreach (var player in players) 
                if (!player.Equals(ignore))
                    player.write(message, newline, flush);
        }


        public void writeToTeam(int team, string message, bool newline, bool flush) {
            foreach (var player in players) 
                if (player.team == team)
                    player.write(message, newline, flush);
        }

        private void startGame(){
            DisplayManager.print("Game starting\t: ", DisplayManager.BLUE, false);
            DisplayManager.print("Room " + id, DisplayManager.WHITE, true);
            createTeams();
            game.run();
        }

        public bool removePlayer(IChannel channel)
        {
            for (var i = 0; i < players.Count; i++)
                if (players[i].channel.Equals(channel)) {
                    players.RemoveAt(i);
                    return true;
                }
            return false;
        }

        private void createTeams() {
            var rand = new Random();
            players.randomize();

            for (var i = 0; i < players.Count; i++)
                players[i].team = i % 2 == 0 ? 1 : 2;
            writeToRoom("Teams created :", true, false);
            writeToRoom("\t\tTeam 1 : " + players[0].name + " - " + players[2].name, true, false);
            writeToRoom("\t\tTeam 2 : " + players[1].name + " - " + players[3].name, true, true);
            }
        }
}
