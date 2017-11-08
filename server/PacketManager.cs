using DotNetty.Transport.Channels;

namespace cardGamesServer {
    public class PacketManager {
        public static PacketManager instance { get; }= new PacketManager();

        private void executeCommand(Player player, Command command, IChannel channel)
        {
            switch (command.Type)
            {
                case Command.Types.Type.Name:
                    PlayerManager.instance.findRoom(channel).initPlayer(player, command.Argument);
                    break;
                case Command.Types.Type.Help:
                    player.write("HELP", true, true);
                    break;
            }
        }

        public void treatPacket(IChannel channel, Packet packet)
        {
            var player = PlayerManager.instance.findPlayer(channel);
            switch (packet.Type)
            {
                case Packet.Types.Type.Command:
                    executeCommand(PlayerManager.instance.findPlayer(channel), packet.Command, channel);
                    break;
                case Packet.Types.Type.Bid:
                    PlayerManager.instance.findRoom(channel).game.setBid(player, packet);
                    break;
                case Packet.Types.Type.Action:
                    PlayerManager.instance.findRoom(channel).game.putcard(player, packet);
                    break;
                case Packet.Types.Type.Unknown:
                    if (packet.Message.Equals("BID"))
                        PlayerManager.instance.findRoom(channel).game.setBid(player, packet);
                    else
                        PlayerManager.instance.findRoom(channel).game.putcard(player, packet);
                    break;
            }
        }
    }
}