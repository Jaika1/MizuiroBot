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

        public delegate Task IrcMessageRecievedEvent(object sender, IrcMessageRecievedEventArgs e);
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
                string msg = await inStream.ReadLineAsync();
                IrcMessageReceived?.Invoke(this, new IrcMessageRecievedEventArgs(msg));
            }
        }
    }

    public struct IrcMessageRecievedEventArgs
    {
        public readonly string RawMessage;
        public readonly string Prefix;
        public readonly string Command;
        public readonly string Parameters;

        internal IrcMessageRecievedEventArgs(string raw)
        {
            RawMessage = raw;

            if (raw.StartsWith(":"))
            {
                int preIndex = raw.IndexOf(' ');
                int cmdIndex = raw.IndexOf(' ', preIndex + 1);

                Prefix = raw.Substring(0, preIndex).Replace(":", null);
                Command = raw.Substring(preIndex + 1, cmdIndex - preIndex - 1);
                Parameters = raw.Substring(cmdIndex + 1);
            } else
            {
                int cmdIndex = raw.IndexOf(' ');

                Prefix = "";
                Command = raw.Substring(0, cmdIndex);
                Parameters = raw.Substring(cmdIndex + 1);
            }

        }

        internal IrcMessageRecievedEventArgs(string raw, string pre, string cmd, string param)
        {
            RawMessage = raw;
            Prefix = pre;
            Command = cmd;
            Parameters = param;
        }
    }
}
