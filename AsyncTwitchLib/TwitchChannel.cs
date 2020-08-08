﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTwitchLib
{
    public class TwitchChannel
    {
        private TwitchIrcClient client;
        public string Channel;

        public TwitchChannel(string chan, TwitchIrcClient c)
        {
            client = c;
            Channel = chan;
        }

        public async Task SendChatMessage(string msg)
        {
            await client.SendChatMessage(msg, Channel);
        }

        public async Task PartChannel()
        {
            await client.PartChannel(Channel);
        }
    }
}
