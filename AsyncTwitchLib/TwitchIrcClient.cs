using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AsyncTwitchLib
{
    public delegate Task ChatMessageReceivedEvent(object sender, ChatMessageReceivedEventArgs e);
    public delegate Task ChannelJoinedEvent(object sender, ChannelJoinedEventArgs e);
    public delegate Task ConnectedEvent(object sender);

    public class TwitchIrcClient
    {
        private string username;
        private string authToken;

        private IrcClient ircClient;
        private List<TwitchChannel> joinedChannels = new List<TwitchChannel>();

        public event ChatMessageReceivedEvent ChatMessageReceived;
        public event ChannelJoinedEvent ChannelJoined;
        public event ConnectedEvent Connected;

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
        }

        public async Task JoinChannel(string channel)
        {
            await ircClient.SendIrcMessage($"JOIN #{channel}");
        }

        public async Task SendChatMessage(string msg, string channel)
        {
            await ircClient.SendIrcMessage($"PRIVMSG #{channel} :{msg}");
        }

        public TwitchChannel GetChannel(string channelName)
        {
            return joinedChannels.Exists(x => x.Channel == channelName) ? joinedChannels.Find(x => x.Channel == channelName) : null;
        }

        public async Task PartChannel(string channel)
        {
            await ircClient.SendIrcMessage($"PART #{channel}");
            if (joinedChannels.Exists(x => x.Channel == channel)) joinedChannels.Remove(joinedChannels.Find(x => x.Channel == channel));
        }

        private async Task IrcMessageReceived(object sender, IrcMessageRecievedEventArgs e)
        {
            //Console.WriteLine($"{e.Prefix,-64}{e.Command,-16}{e.Parameters}");
            switch (e.Command) 
            {
                case "001":
                    Connected?.Invoke(this);
                    break;
                case "PING":
                    await ircClient.SendIrcMessage("PONG :tmi.twitch.tv");
                    break;
                case "PRIVMSG":
                    ChatMessageReceived?.Invoke(this, new ChatMessageReceivedEventArgs(this, e.Prefix, e.Parameters));
                    break;
                case "JOIN":
                    TwitchChannel channel = new TwitchChannel(e.Parameters.Replace("#", null), this);
                    joinedChannels.Add(channel);
                    ChannelJoined?.Invoke(this, new ChannelJoinedEventArgs(channel));
                    break;
            }
        }
    }

    public struct ChatMessageReceivedEventArgs
    {
        public readonly string Content;
        public readonly TwitchChannel Channel;
        public readonly string User;
        public readonly string IrcUser;

        public ChatMessageReceivedEventArgs(TwitchIrcClient client, string prefix, string param)
        {
            IrcUser = prefix;
            User = prefix.Remove(prefix.IndexOf('!'));
            string[] paramSplit = param.Split(" :");
            Channel = client.GetChannel(paramSplit[0].Replace("#", null));
            Content = paramSplit[1];
        }
    }

    public struct ChannelJoinedEventArgs
    {
        public readonly TwitchChannel Channel;

        public ChannelJoinedEventArgs(TwitchChannel chan)
        {
            Channel = chan;
        }
    }
}
