namespace cardGamesServer {
    using System.Collections.Generic;

    public class Rank {
        public static readonly Rank SEVEN = new Rank(1, "SEVEN", 0, 0);
        public static readonly Rank EIGHT = new Rank(2, "EIGHT", 0, 0);
        public static readonly Rank NINE = new Rank(3, "NINE", 0, 9);
        public static readonly Rank TEN = new Rank(4, "TEN", 10, 5);
        public static readonly Rank JACK = new Rank(5, "JACK", 2, 14);
        public static readonly Rank QUEEN = new Rank(6, "QUEEN", 3, 2);
        public static readonly Rank KING = new Rank(7, "KING", 4, 3);
        public static readonly Rank ACE = new Rank(8, "ACE", 19, 7);
        
        public int valueSansAtout { get; }
        public int valueAvecAtout { get; }
        public string name { get; }
        public int id { get; }
        
        Rank(int id, string name, int valueSansAtout, int valueAvecAtout)
        {
            this.id = id; 
            this.name = name;
            this.valueSansAtout = valueSansAtout;
            this.valueAvecAtout = valueAvecAtout;
        }
        
        public static IEnumerable<Rank> values {
            get {
                yield return SEVEN;
                yield return EIGHT;
                yield return NINE;
                yield return TEN;
                yield return JACK;
                yield return QUEEN;
                yield return KING;
                yield return ACE;
            }
        }
    }
    
    public class Suit {
        public static readonly Suit HEARTS = new Suit("HEARTS");
        public static readonly Suit SPADES = new Suit("SPADES");
        public static readonly Suit DIAMONDS = new Suit("DIAMONDS");
        public static readonly Suit CLUBS = new Suit("CLUBS");
        
        public string name { get; }
        
        Suit(string name) {
            this.name = name;
        }
        
        public static IEnumerable<Suit> values {
            get {
                yield return HEARTS;
                yield return SPADES;
                yield return DIAMONDS;
                yield return CLUBS;
            }
        }
    }
    
    public class Card {
        public Rank rank { get; }
        public Suit suit { get; }

        public Card(Rank rank, Suit suit) {
            this.rank = rank;
            this.suit = suit;
        }
        
        public int getValue(string suit) {
            return this.suit.name.Equals(suit) ? rank.valueAvecAtout : rank.valueSansAtout;
        }
    }
}