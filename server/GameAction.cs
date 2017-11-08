namespace cardGamesServer {
    public class GameAction
    {
        public Player player { get; set; }
        public Packet.Types.Type type { get; set; }

        public void notifyPlayer()
        {
            switch (type)
            {
                case Packet.Types.Type.Bid:
                    player.sendBidStart();
                    break;
                case Packet.Types.Type.Stage:
                    player.sendActionStart();
                    break;
            }
        }

    }
}

