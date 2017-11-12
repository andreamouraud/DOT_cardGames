
namespace cardGamesServer
{
    using System;
    using System.Text;
    using DotNetty.Buffers;
    using DotNetty.Transport.Channels;

    public class ServerHandler : SimpleChannelInboundHandler<Packet>
    {
        protected override void ChannelRead0(IChannelHandlerContext context, Packet packet) {
            PacketManager.instance.treatPacket(context.Channel, packet);
        }
        
        public override void HandlerAdded(IChannelHandlerContext context) { 
            var channel = context.Channel;

            DisplayManager.print("New Player\t\t: ", DisplayManager.GREEN, false);
            DisplayManager.print(channel.ToString(),DisplayManager.WHITE,true);
            PlayerManager.instance.addPlayer(channel);
        }
        
        public override void HandlerRemoved(IChannelHandlerContext context) {
            var channel = context.Channel;
            var player = PlayerManager.instance.findPlayer(channel);

            PlayerManager.instance.findRoom(channel).writeToRoomBut(player,"Player " + player.name + " from Team " + player.team + "has left the game\nGame will restart when a new player is back", true, true);
            DisplayManager.print("Player left\t\t: ", DisplayManager.RED, false);
            DisplayManager.print(channel.ToString(),DisplayManager.WHITE,true);
            PlayerManager.instance.removePlayer(channel);
        }
        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception) {
            DisplayManager.print("Error\t\t\t: ", DisplayManager.BLACK, false);
            DisplayManager.print(exception.GetBaseException().Message,DisplayManager.WHITE,true);
            context.CloseAsync();
        }
    }
}