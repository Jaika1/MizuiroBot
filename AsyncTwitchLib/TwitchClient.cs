using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTwitchLib
{
    public class TwitchClient
    {
        private TwitchIrcClient client;
        private char prefixChar = '!';

        public TwitchClient()
        {
            client = new TwitchIrcClient();
            client.ChatMessageReceived += ChatMessageReceived;
        }

        public void SetPrefix(char prefix)
        {
            prefixChar = prefix;
        }

        public async Task Connect(string username, string authToken)
        {
            await client.Connect(username, authToken);
        }

        public async Task JoinChannel(string channel)
        {
            await client.JoinChannel(channel);
        }

        private Task ChatMessageReceived(object sender, ChatMessageReceivedEventArgs e)
        {
            Console.WriteLine($"{e.Channel,-24}{e.User,-24}{e.Content}");
            return Task.CompletedTask;
        }
    }
}
