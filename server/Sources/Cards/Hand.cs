using System.Collections.Generic;

namespace cardGamesServer {
    public class Hand {
        public List<Card> cards { get; } = new List<Card>();

        public void add(Card card) {
            cards.Add(card);
        }

        public void remove(Card card) {
            cards.Remove(card);
        }

        public Card getCard(Action action){
            var rank = action.Rank;
            var suit = action.Suit;
         
            foreach (var card in cards) 
                if (card.rank.name.Equals(rank) && card.suit.name.Equals(suit))
                    return card;
            return null;
        }
    }
}
