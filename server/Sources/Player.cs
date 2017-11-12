using System;
using DotNetty.Transport.Channels;

namespace cardGamesServer {
    public class Player {
        public IChannel channel { get; }
        public string name { get; set; }
        public int team { get; set; }
        public Hand hand { get; }

        public Player(IChannel channel) {
            this.channel = channel;
            hand = new Hand();
        }

        public void write(string message, bool newline, bool flush) {
            var packet = new Packet
            {
                Message = message + (newline ? "\n" : "")
            };
            channel.WriteAsync(packet);
            if (flush)
                channel.Flush();
        }

        public void displayHand() {
            write("Your hand is :", true, false);
            foreach (var card in hand.cards) 
                write("\t\t" + card.rank.name + " of " + card.suit.name, true, false);
            channel.WriteAndFlushAsync("\n");
        }

        public void sendBidStart() {
            var packet = new Packet
            {
                Type = Packet.Types.Type.Stage,
                Stage = new Stage {Type = Stage.Types.Type.BidStart}
            };
            channel.WriteAndFlushAsync(packet);
        }

        public void sendActionStart() {
            var packet = new Packet
            {
                Type = Packet.Types.Type.Stage,
                Stage = new Stage {Type = Stage.Types.Type.GameStart}
            };
            channel.WriteAndFlushAsync(packet);
        }
    }
}
