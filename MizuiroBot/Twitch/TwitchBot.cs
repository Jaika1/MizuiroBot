using MizuiroBot.Shared;
using MizuiroBot.Tools;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace MizuiroBot.Twitch
{
    public class TwitchBot
    {
        TwitchClient botUser = new TwitchClient();
        private char CommandPrefix => Program.Config.CommandPrefix;

        public void Init(string botUsername, string oauth2)
        {
            CVTS.WriteLineTwitch("Initialising the Twitch bot...");
            botUser.Initialize(new ConnectionCredentials(botUsername, oauth2));
            botUser.AddChatCommandIdentifier(CommandPrefix);
            botUser.OnConnected += OnConnected;
            botUser.OnJoinedChannel += OnJoinedChannel;
            botUser.OnChatCommandReceived += OnChatCommandReceived;
            botUser.OnBeingHosted += OnBeingHosted;
            botUser.OnNewSubscriber += OnNewSubscriber;
            botUser.OnGiftedSubscription += OnGiftedSubscription;
            botUser.OnRaidNotification += OnRaidNotification;
            CVTS.WriteLineDiscord("Initialization of the Twitch bot has completed!");
        }

        public void Start()
        {
            CVTS.WriteLineTwitch("Connecting to twitch...");
            botUser.Connect();
        }

        public bool JoinChannel(string channel)
        {
            try
            {
                CVTS.WriteLineTwitch($"Joining channel {channel}...");
                botUser.JoinChannel(channel);
                CVTS.WriteLineTwitch($"Joined channel {channel} successfully!");
                return true;
            } catch
            {
                CVTS.WriteLineTwitch($"Failed to join channel {channel}!");
                return false;
            }
        }

        private void OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
        {
            CVTS.WriteLineTwitch("Joining all specified channels in shared channel info...");
            foreach(SharedBotInfo shared in SharedBotInfo.SharedInfo)
            {
                if (!string.IsNullOrWhiteSpace(shared.TwitchChannelName))
                {
                    JoinChannel(shared.TwitchChannelName);
                }
                else
                {
                    CVTS.WriteLineTwitch($"Channel not found in shared info.");
                }
            }
            CVTS.WriteLineOk("Twitch bot started successfully!");
        }

        private void OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
        {
            JoinedChannel joinedChannel = botUser.GetJoinedChannel(e.Channel);
            botUser.SendMessage(joinedChannel, $"Mizuiro, ret-2-go! I am now ready to accept commands, prefixed with '{CommandPrefix}'");
        }

        private void OnBeingHosted(object sender, TwitchLib.Client.Events.OnBeingHostedArgs e)
        {
            if (!e.BeingHostedNotification.IsAutoHosted)
            {
                JoinedChannel hostedChannel = botUser.GetJoinedChannel(e.BeingHostedNotification.Channel);
                botUser.SendMessage(hostedChannel, $"{e.BeingHostedNotification.HostedByChannel} is bringing their shoal of {e.BeingHostedNotification.Viewers} fellow cephalopods to the stream!");
            }
        }

        private void OnRaidNotification(object sender, TwitchLib.Client.Events.OnRaidNotificationArgs e)
        {
            //TODO
            //JoinedChannel raidedChannel = botUser.GetJoinedChannel(e.Channel);
            //botUser.SendMessage(raidedChannel, $"{e.RaidNotification.DisplayName} is bringing their shoal of {e.BeingHostedNotification.Viewers} fellow cephalopods to the stream!");
        }

        private void OnNewSubscriber(object sender, TwitchLib.Client.Events.OnNewSubscriberArgs e)
        {
            JoinedChannel subbedChannel = botUser.GetJoinedChannel(e.Channel);
            botUser.SendMessage(subbedChannel, $"{e.Subscriber.DisplayName} just subscribed to the channel! Enjoy the emotes and lack of ads!");
        }

        private void OnGiftedSubscription(object sender, TwitchLib.Client.Events.OnGiftedSubscriptionArgs e)
        {
            //Blank for now
        }

        private void OnChatCommandReceived(object sender, TwitchLib.Client.Events.OnChatCommandReceivedArgs e)
        {
            JoinedChannel commandChannel = botUser.GetJoinedChannel(e.Command.ChatMessage.Channel);
            switch (e.Command.CommandText)
            {
                case "fc":
                    botUser.SendMessage(commandChannel, "Jaika★ SW-2318-9798-0489 <--> ジャイカ★ SW-0855-8018-0899 <--> Jaika3 SW-4531-8713-2907 <--> Jaika4 SW-5530-5326-3525 <--> Jaika5 SW-5249-3417-8304");
                    break;
                case "discord":
                    botUser.SendMessage(commandChannel, "Join the discord server using this link! Prepare yourself for a world of peak inactivity! https://discord.gg/sq45W4B ");
                    break;
            }
        }
    }
}
