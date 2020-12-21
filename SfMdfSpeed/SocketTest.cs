using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SfMdfSpeed
{
    public class SocketTest : SpeedTest
    {
        public override async IAsyncEnumerable<SpeedResult> RunAsync()
        {
            var bytes = TestData.Repeat((byte)51, 600_000_000);

            for (int i = 0; i < 10; i++)
            {
                var (receiver, sender) = await GetClientsAsync();

                using (receiver)
                using (sender)
                {
                    yield return await RunAsync(
                        "Tcp Socket Sends and Receives",
                        async () => await RunAsync(receiver, sender, bytes),
                        bytes.Length);
                }
            }
        }

        private static async Task<(TcpClient receiver, TcpClient sender)> GetClientsAsync()
        {
            const string ipText = "127.0.0.1";
            var ipAddress = IPAddress.Parse(ipText);

            const int port = 20123;
            var listener = new TcpListener(ipAddress, port);

            listener.Start();
            try
            {
                var accept = listener.AcceptTcpClientAsync();

                var sender = new TcpClient(ipText, port);

                var receiver = await accept;

                return (receiver, sender);
            }
            finally
            {
                listener.Stop();
            }
        }

        private static async Task RunAsync(TcpClient receiver, TcpClient sender, byte[] bytes)
        {
            var send = SendAsync(sender, bytes);
            var receive = ReceiveAsync(receiver, bytes.Length);

            await send;
            await receive;
        }
        private static async Task ReceiveAsync(TcpClient client, int count)
        {
            var stream = client.GetStream();

            var buffer = new byte[1_000_000];
            var memory = new Memory<byte>(buffer);

            var read = 0;
            while (read < count)
            {
                read += await stream.ReadAsync(memory);
            }
        }
        private static async Task SendAsync(TcpClient client, byte[] bytes)
        {
            var stream = client.GetStream();

            var memory = new Memory<byte>(bytes);
            await stream.WriteAsync(memory);

            await stream.FlushAsync();

            client.Close();
        }
    }
}
