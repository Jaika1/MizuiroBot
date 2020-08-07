using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTwitchLib
{
    public class TwitchIrcClient
    {
        private string username;
        private string authToken;

        private IrcClient ircClient;

        public TwitchIrcClient()
        {
            ircClient = new IrcClient();
            ircClient.IrcMessageReceived += IrcMessageReceived;
        }

        public async Task Connect(string username, string authToken)
        {
            this.username = username;
            this.authToken = authToken;

            await ircClient.Connect("irc.chat.twitch.tv", 6667, username, $"oauth:{authToken}");
            _ = PingLoop();
        }

        public async Task JoinChannel(string channel)
        {
            await ircClient.SendIrcMessage($"JOIN #{channel}");
        }

        public async Task SendChatMessage(string msg, string channel)
        {
            await ircClient.SendIrcMessage($":{username}!{username}@{username}.tmi.twitch.tv PRIVMSG #{channel} :{msg}");
        }

        private async Task PingLoop()
        {
            while (true)
            {
                await ircClient.SendIrcMessage("PING irc.twitch.tv");
                await Task.Delay(TimeSpan.FromMinutes(5));
            }
        }

        private void IrcMessageReceived(object sender, string message)
        {
            Console.WriteLine(message);
        }
    }
}
