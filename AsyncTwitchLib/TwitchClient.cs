using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTwitchLib
{
    public class TwitchClient
    {
        private TwitchIrcClient client;
        private char prefixChar = '!';
        private Dictionary<string, MethodInfo> commandList = new Dictionary<string, MethodInfo>();

        public TwitchClient()
        {
            client = new TwitchIrcClient();
            client.ChatMessageReceived += ChatMessageReceived;

            Assembly asm = Assembly.GetEntryAssembly();
            IEnumerable<Type> commandModules = from t in asm.GetTypes()
                                               where t.GetTypeInfo().BaseType == typeof(TwitchCommandModule)
                                               select t;
            IEnumerable<MethodInfo> commandMethods = from t in commandModules
                                                     from m in t.GetMethods()
                                                     where m.GetCustomAttributes<TwitchCommand>().Count() == 1
                                                     select m;
            foreach(MethodInfo mi in commandMethods)
            {
                TwitchCommand commandInfo = mi.GetCustomAttribute<TwitchCommand>();
                commandList.Add(commandInfo.CommandName, mi);
            }
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

        public TwitchChannel GetChannel(string channel)
        {
            return client.GetChannel(channel);
        }

        public async Task PartChannel(TwitchChannel channel)
        {
            await client.PartChannel(channel.Channel);
        }

        private Task ChatMessageReceived(object sender, ChatMessageReceivedEventArgs e)
        {
            Console.WriteLine($"{e.Channel.Channel,-24}{e.User,-24}{e.Content}");
            TryProcessCommand(this, e.Channel, e.Content);
            return Task.CompletedTask;
        }

        private Task TryProcessCommand(TwitchClient tcl, TwitchChannel channel, string content)
        {
            if (content.StartsWith(prefixChar))
            {
                string[] split = content.Split(" ");
                string command = split[0].Replace("!", null);
                if (commandList.ContainsKey(command))
                {
                    MethodInfo m = commandList[command];

                    TwitchCommandModule tcm = (TwitchCommandModule)Activator.CreateInstance(m.DeclaringType);
                    tcm.Channel = channel;
                    tcm.Client = tcl;

                    object[] paramaters = new object[m.GetParameters().Length];

                    for (int i = 0; i < m.GetParameters().Length; ++i)
                    {
                        ParameterInfo p = m.GetParameters()[i];
                        Type t = p.ParameterType;
                        

                        switch (t.Name)
                        {
                            case "String":
                                paramaters[i] = split[i + 1];
                                break;
                            case "String[]":
                                paramaters[i] = split.Skip(i + 1).ToArray();
                                break;
                            case "Int32":
                                paramaters[i] = int.Parse(split[i + 1]);
                                break;
                        }
                    }

                    m.Invoke(tcm, paramaters);
                }
            }
            return Task.CompletedTask;
        }
    }
}
