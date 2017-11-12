namespace cardGamesServer {
    using System.Collections.Generic;

    public class Heap {
        private List<Card> cards = new List<Card>();

        public void create() {
            foreach (var suit in Suit.values) 
                foreach (var rank in Rank.values) 
                    cards.Add(new Card(rank, suit));
            cards.randomize();
        }

        public void deal(List<Player> players) {
            pickCards(3, players);
            pickCards(2, players);
            pickCards(3, players);
        }

        private void pickCards(int nb, List<Player> players) {
            for (var picked = 0; picked < nb; picked++)
                foreach (var player in players) 
                    pickCard(player);
        }

        private void pickCard(Player player) {
            player.hand.add(cards[0]);
            cards.RemoveAt(0);
        } 
    }
}