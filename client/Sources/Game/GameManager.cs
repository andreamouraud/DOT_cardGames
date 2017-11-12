using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading;
using DotNetty.Transport.Channels;

namespace cardGamesClient
{
    public class GameManager
    {
        public IChannel channel { get; set; }
        public bool running { get; set; } = true;
        
        public enum Type
        {
            UNKNOWN,
            BID,
            GAME
        }

        public Type type { get; set; } = Type.UNKNOWN;
        public static GameManager instance { get; } = new GameManager();

        private bool validInput(string[] input)
        {
            return input.Length > 2 && (input[0].Equals("BID") || input[0].Equals("PUTCARD"));
        }

        private Bid.Types.Atout toAtout(string input)
        {
            Bid.Types.Atout atout;
            
            switch (input)
            {
                case "SPADES":
                    atout = Bid.Types.Atout.Spades;
                    break;
                case "HEARTS":
                    atout = Bid.Types.Atout.Hearts;
                    break;
                case "CLUBS":
                    atout = Bid.Types.Atout.Clubs;
                    break;
                case "DIAMONDS":
                    atout = Bid.Types.Atout.Diamonds;
                    break;
                default:
                    atout = Bid.Types.Atout.Unknown;
                    break;
            }
            return atout;
        }

        private Packet putcardPacket(string[] input)
        {
            var packet = new Packet
            {
                Type = Packet.Types.Type.Action,
                Action = new Action {Rank = input[1], Suit = input[2], Type = Action.Types.Type.Putcard}
            };
            return packet;
        }
        
        private Packet bidPacket(string[] input)
        {
            var value = 9999;
            
            try { value = int.Parse(input[2]);}
            catch (FormatException) {}
            var packet = new Packet
            {
                Type = Packet.Types.Type.Bid,
                Bid = new Bid {Action = Bid.Types.Action.Bid, Atout = toAtout(input[1]), Value = value }
            };
            return packet;
        }
        private Packet toPacket(string[] input)
        {
            return input[0].Equals("BID") ? bidPacket(input) : putcardPacket(input);
        }
        
        private void askForInput()
        {
            string[] input = { string.Empty };

            while (!validInput(input))
            {
                Console.Write(" > ");
                input = Console.ReadLine().Split(' ');
            }
            channel.WriteAndFlushAsync(toPacket(input));
        }
        
        public void run()
        {
            while (running)
            {
                SpinWait.SpinUntil(() => type != Type.UNKNOWN);
                if (running)
                    askForInput();
                type = Type.UNKNOWN;
            }
        }
    }
}