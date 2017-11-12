using System.Net.Mime;

namespace cardGamesClient
{
    using System;
    using DotNetty.Transport.Channels;

    public class ClientHandler : SimpleChannelInboundHandler<Packet>
    {
        protected override void ChannelRead0(IChannelHandlerContext context, Packet packet)
        {
            Console.Write(packet.Message);
            PacketManager.instance.treatPacket(context.Channel, packet);
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
           Console.Write("Enter your name: ");
           var name = Console.ReadLine();
            var packet = new Packet
            {
                Type = Packet.Types.Type.Command,
                Command = new Command {Type = Command.Types.Type.Name, Argument = name }
            };
            context.WriteAndFlushAsync(packet);
        }
        
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine(exception.Message);
            GameManager.instance.running = false;
            GameManager.instance.type = GameManager.Type.GAME;
        }
    }
}