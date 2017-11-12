using System;
using DotNetty.Transport.Channels;

namespace cardGamesClient
{
    public class PacketManager
    {
        public static PacketManager instance { get; } = new PacketManager();

        public void executeStage(IChannel channel, Stage stage)
        {
            GameManager.instance.channel = channel;
            switch (stage.Type)
            {
                case Stage.Types.Type.BidStart:
                    GameManager.instance.type = GameManager.Type.BID;
                    break;
                case Stage.Types.Type.GameStart:
                    GameManager.instance.type = GameManager.Type.GAME;
                    break;
                default:
                    GameManager.instance.type = GameManager.Type.UNKNOWN;
                    break;        
            }
        }
        
        public void treatPacket(IChannel channel, Packet packet)
        {
            switch (packet.Type)
            {
                case Packet.Types.Type.Stage:
                    executeStage(channel, packet.Stage);
                    break;
            }
        }
    }
}