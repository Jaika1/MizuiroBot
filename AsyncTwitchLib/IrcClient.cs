using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AsyncTwitchLib
{
    public class IrcClient
    {
        private string username;

        private TcpClient client;
        private StreamReader inStream;
        private StreamWriter outStream;

        public delegate void IrcMessageRecievedEvent(object sender, string message);
        public event IrcMessageRecievedEvent IrcMessageReceived;

        /// <summary>
        /// Attempts to connect to the specified address and log in
        /// </summary>
        /// <param name="remoteEp">The remote end point to connect to</param>
        /// <param name="username">The username to login with. Will also be used for your nickname.</param>
        /// <param name="password">The credetials that shall be used to verify your identity.</param>
        /// <param name="channel">The IRC channel to join.</param>
        /// <returns></returns>
        public async Task Connect(string hostname, int port, string username, string password)
        {
            this.username = username;

            client = new TcpClient();
            await client.ConnectAsync(hostname, port);
            inStream = new StreamReader(client.GetStream());
            outStream = new StreamWriter(client.GetStream());

            await outStream.WriteLineAsync($"PASS {password}");
            await outStream.WriteLineAsync($"NICK {username}");
            await outStream.WriteLineAsync($"USER {username} 8 * :{username}");
            await outStream.FlushAsync();

            _ = ReadMessageLoop(); // use of discard token
        }

        public async Task SendIrcMessage(string msg)
        {
            await outStream.WriteLineAsync(msg);
            await outStream.FlushAsync();
        }

        private async Task ReadMessageLoop()
        {
            while (true)
            {
                //if (!inStream.EndOfStream)
                //{
                string msg = await inStream.ReadLineAsync();
                IrcMessageReceived?.Invoke(this, msg);
                //}
                //else
                //{
                //    await Task.Delay(10);
                //}
            }
        }
    }

    public class IrcMessageReceivedEventArgs // This comes tomorrow
    {
        //string source;
        //string messageId;
        //string user;
    }
}
