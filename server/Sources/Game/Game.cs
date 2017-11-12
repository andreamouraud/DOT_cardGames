using System;
using System.Collections.Generic;

namespace cardGamesServer
{
    public class Game
    {
        private Room room;
        private Heap heap = new Heap();
        private GameAction nextAction = new GameAction();
        private int currentPlayer ;
        private int value;
        private Bid.Types.Atout atout;
        private int nbOfPass;
        private int teamWhoBid;
        private readonly List<Card> cardsOnBoard = new List<Card>();
        private int scoreTeam1;
        private int scoreTeam2;

        public Game(Room room) {
            this.room = room;
        }

        public void run() {
            foreach (var player in room.players)
            player.hand.cards.Clear();
            currentPlayer = 0;
            value = 0;
            nbOfPass = 0;
            teamWhoBid = 0;
            atout = Bid.Types.Atout.Unknown;
            cardsOnBoard.Clear();
            room.writeToRoom("New round starting", true, false);
            heap.create();
            room.writeToRoom("Dealing cards", true, false);
            heap.deal(room.players);
            foreach (var player in room.players)
                player.displayHand();
            setNextAction(Packet.Types.Type.Bid);
            nextAction.notifyPlayer();
        }

        private bool isBidEnded() {
            return nbOfPass >= 3 || value == 650;
        }

        public void setBid(Player player, Packet packet) {
            if (packet.Type.Equals(Packet.Types.Type.Unknown))
            {
                nextAction.notifyPlayer();
                return;
            }
            var valueFromPacket = packet.Bid.Value;
            if (packet.Bid.Action == Bid.Types.Action.Bid)
            {
                Console.WriteLine(packet);
                if (value < valueFromPacket && (valueFromPacket % 10) == 0
                    && valueFromPacket >= 80 && valueFromPacket <= 650)
                {
                    atout = packet.Bid.Atout;
                    value = packet.Bid.Value;
                    teamWhoBid = player.team;
                    nbOfPass = 0;
                }
                else
                {
                    nextAction.player.write("Wrong bid, must be 10 by 10 and higher than the last one", true, true);
                    nextAction.notifyPlayer();
                    return;
                }
            }
            else
                nbOfPass++;
            if (isBidEnded())
            {
                currentPlayer = 0;
                room.writeToRoom("Bid stage is over", true, false);
                room.writeToRoom("Current bid is : " + atout + " - " + value, true, true);
            }
            setNextAction(isBidEnded() ? Packet.Types.Type.Stage : Packet.Types.Type.Bid);
            nextAction.notifyPlayer();
        }

        private string whatCardsOnTable() {
            var cards = "Cards on the table : \n";

            if (cardsOnBoard.Count == 0)
                cards += "\t\tNone";
            else
                for (var i = 0; i < cardsOnBoard.Count; i++)
                    cards += "\t\t" + whatPlayer(room.players[i]) + cardsOnBoard[i].rank.name +
                             " of " + cardsOnBoard[i].suit.name + "\n";
            return cards;
        }

        private void setNextAction(Packet.Types.Type type) {
            nextAction.player = room.players[currentPlayer];
            nextAction.type = type;
            currentPlayer++;
            if (currentPlayer > 3)
                currentPlayer = 0;
            if (type == Packet.Types.Type.Bid) {
                nextAction.player.write("Your turn to bid (Current bid is " + atout + " : " + value + ") :", true, true);
                room.writeToRoomBut(nextAction.player, whatPlayer(nextAction.player) + "is bidding", true, true);
            }
            else {
                nextAction.player.write("Your turn to play : ", true, false);
                nextAction.player.write(whatCardsOnTable(), true, true);
                nextAction.player.displayHand();
                room.writeToRoomBut(nextAction.player, whatPlayer(nextAction.player) + "is playing", true, true);
            }
        }

        private bool allPlayerPlayed() {
            return cardsOnBoard.Count == 4;
        }

        private bool isGameEnded() {
            return scoreTeam1 >= 701 || scoreTeam2 >= 701;
        }

        private int winningTeam() {
            return scoreTeam1 > scoreTeam2 ? 1 : 2;
        }

        private void endGame() {
            room.writeToTeam(1, winningTeam() == 1 ? "You won" : "You lost", true, false);
            room.writeToTeam(2, winningTeam() == 2 ? "You won" : "You lost", true, false);
            room.writeToRoom("Thanks for playing, leaving game soon...", true, true);
            PlayerManager.instance.removeRoom(room);
            DisplayManager.print("Game ended\t: ", DisplayManager.BLUE, false);
            DisplayManager.print("Room " + room.id, DisplayManager.WHITE, true);
        }

        private void additionCards() {
            var winningTeam = 0;
            var highest = 0;
            var scorePli = 0;

            for (var i = 0; i < 4; i++) {
                var value = cardsOnBoard[i].getValue(atout.ToString());

                if (value > highest) {
                    highest = value;
                    winningTeam = room.players[i].team;
                }
                scorePli += value;
            }
            if (room.players[0].hand.cards.Count == 0) {
                if (winningTeam == 1) {
                    scoreTeam1 += 10;
                    room.writeToRoom("Team 1 won the Dix de Der", true, true);
                }
                else {
                    scoreTeam2 += 10;
                    room.writeToRoom("Team 2 won the Dix de Der", true, true);
                }
                if (!isGameEnded())
                    run();
                else
                    endGame();
            }
            if (winningTeam == 1)
                scoreTeam1 += scorePli;
            else
                scoreTeam2 += scorePli;
            room.writeToRoom("Scores :\n\t\tTeam 1 : " + scoreTeam1 + "points\n\t\tTeam 2 : " + scoreTeam2 + "points",
                true, true);
            cardsOnBoard.Clear();
        }

        private string whatPlayer(Player player) {
            return "Player " + player.name + " from Team " + player.team + " : ";
        }

        private string whatCard(Card card) {
            return "Put the " + card.rank.name + " of " + card.suit.name + " card on the table";
        }

        private void lookForAnnonces() {
            var found = false;
            
            room.writeToRoom("Looking for annonces", true, false);
            foreach (var player in room.players) {
                Annonce annonce = AnnonceManager.instance.lookForAnnonce(player.hand);
                if (annonce.getValue() != 0)
                {
                    player.write("You have a " + annonce.type.name + " worth " + annonce.getValue(), true, false);
                    room.writeToRoomBut(player,
                        whatPlayer(player) + "has a " + annonce.type.name + " worth " + annonce.getValue(), true, false);
                    if (player.team == 1)
                        scoreTeam1 += annonce.getValue();
                    else
                        scoreTeam2 += annonce.getValue();
                    found = true;
                }
            }
            if (!found)
                room.writeToRoom("No annonces were found", true, false);
            room.writeToRoom("Scores :\n\t\tTeam 1 : " + scoreTeam1 + "points\n\t\tTeam 2 : " + scoreTeam2 + "points", true, true);
        }

        public void putcard(Player player, Packet packet) {
            if (packet.Type.Equals(Packet.Types.Type.Unknown))
            {
                nextAction.notifyPlayer();
                return;
            }
            var card = player.hand.getCard(packet.Action);
            if (card != null)
            {
                cardsOnBoard.Add(card);
                player.hand.cards.Remove(card);
                room.writeToRoomBut(player, whatPlayer(player) + whatCard(card), true, true);
                player.write("Your card has been placed on the table", true, true);
                if (player.hand.cards.Count == 6 && currentPlayer == 1)
                    lookForAnnonces();
                setNextAction(Packet.Types.Type.Stage);
            }
            if (allPlayerPlayed())
                additionCards();
            nextAction.notifyPlayer();
        }
    }
}