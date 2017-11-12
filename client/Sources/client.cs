using System.Threading;
using DotNetty.Codecs.Protobuf;

namespace cardGamesClient
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using DotNetty.Transport.Bootstrapping;
    using DotNetty.Transport.Channels;
    using DotNetty.Transport.Channels.Sockets;

    class Program
    {
        static async Task RunClientAsync()
        {
            var group = new MultithreadEventLoopGroup();

            try
            {
                var bootstrap = new Bootstrap();
                bootstrap.Group(group);
                bootstrap.Channel<TcpSocketChannel>();
                bootstrap.Option(ChannelOption.TcpNodelay, true);
                bootstrap.Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        var pipeline = channel.Pipeline; 
                        pipeline.AddLast(new ProtobufVarint32FrameDecoder());
                        pipeline.AddLast(new ProtobufDecoder(Packet.Parser));

                        pipeline.AddLast(new ProtobufVarint32LengthFieldPrepender());
                        pipeline.AddLast(new ProtobufEncoder());
                        pipeline.AddLast("echo", new EchoClientHandler());
                    }));

                IChannel clientChannel = null;
                try {
                    clientChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));
                }
                catch (AggregateException e) {
                    Console.WriteLine("Cannot connect to server");
                    return;
                }

                GameManager.instance.run();
                await clientChannel.CloseAsync();

            }
            finally
            {
                await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
        }

        static void Main() => RunClientAsync().Wait();
    }
}