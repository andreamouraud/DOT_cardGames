using System.Collections.Generic;
using System.Linq;

namespace cardGamesServer {
    public class AnnonceManager {
        public static AnnonceManager instance { get; } = new AnnonceManager();

        public Annonce lookForAnnonce(Hand hand) {
            var annonce = new Annonce();

            if (isCarre(hand, annonce) || isCent(hand, annonce) || isBelote(hand, annonce) || isCinquante(hand, annonce)
                || isTierce(hand, annonce))
                return annonce;
            return annonce;
        }

        private bool isCardFromAnnonce(Rank rank) {
            return rank.name.Equals("JACK")|| rank.name.Equals("NINE") || rank.name.Equals("ACE") || rank.name.Equals("TEN") ||
                   rank.name.Equals("KING") || rank.name.Equals("QUEEN");
        }

        private bool isCarre(Hand hand, Annonce annonce) {
            var map = new Dictionary<Rank, int>
            {
                {Rank.JACK, 0},
                {Rank.NINE, 0},
                {Rank.ACE, 0},
                {Rank.TEN, 0},
                {Rank.KING, 0},
                {Rank.QUEEN, 0}
            };

            foreach (var card in hand.cards)
                if (isCardFromAnnonce(card.rank))
                    map[card.rank] = map[card.rank] + 1;
            foreach(var entry in map)
                if (entry.Value == 4)
                    if (entry.Key.Equals(Rank.JACK))
                        annonce.type = Type.CARRE_VALETS;
                    else if (entry.Key.Equals(Rank.NINE))
                        annonce.type = Type.CARRE_NEUF;
                    else
                        annonce.type = Type.CARRE;
            return annonce.getValue() != 0;
        }

        private bool isCent(Hand hand, Annonce annonce) {
            var ranks = new List<Rank>();
            var needed = new List<Rank>();

            foreach(var card in hand.cards)
                ranks.Add(card.rank);
            needed.Add(Rank.SEVEN);
            needed.Add(Rank.EIGHT);
            needed.Add(Rank.NINE);
            needed.Add(Rank.TEN);
            needed.Add(Rank.JACK);
            needed.Add(Rank.QUEEN);
            needed.Add(Rank.KING);
            needed.Add(Rank.ACE);

            if (!needed.Except(ranks).Any())
                annonce.type = Type.CENT;
            return annonce.getValue() != 0;
        }

        private bool isCinquante(Hand hand, Annonce annonce) {
            var ranks = new List<Rank>();

            foreach (var card in hand.cards)
                ranks.Add(card.rank);
            ranks = ranks.OrderBy(q => q.id).ToList();
            for (var i = 0; i < ranks.Count; i++) {
                var name = ranks[i].name;
                if (i + 2 <= ranks.Count && ranks[i + 1].name.Equals(name)
                    && i + 3 <= ranks.Count &&
                    ranks[i + 2].name.Equals(name)
                    && i + 4 <= ranks.Count &&
                    ranks[i + 3].name.Equals(name))
                    annonce.type = Type.CINQUANTE;
            }
            return annonce.getValue() != 0;
        }

        private bool isTierce(Hand hand, Annonce annonce) {
            var ranks = new List<Rank>();

            foreach (var card in hand.cards)
                ranks.Add(card.rank);
            ranks = ranks.OrderBy(q => q.id).ToList();
            for (var i = 0; i < ranks.Count; i++) {
                var name = ranks[i].name;
                if (i + 2 <= ranks.Count && ranks[i + 1].name.Equals(name)
                    && i + 3 <= ranks.Count &&
                    ranks[i + 2].name.Equals(name))
                    annonce.type = Type.TIERCE;
            }
            return annonce.getValue() != 0;
        }

        private bool isBelote(Hand hand, Annonce annonce) {
            var ranks = new List<Rank>();
            var needed = new List<Rank>();

            foreach (var card in hand.cards)
                ranks.Add(card.rank);
            needed.Add(Rank.KING);
            needed.Add(Rank.QUEEN);

            if (!needed.Except(ranks).Any())
                annonce.type = Type.BELOTE;
            return annonce.getValue() != 0;
        }
    }
}