using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;

namespace QQBotForCSharp
{
    public class QQBotMessage
    {
        public delegate void Function(string[] msg, GroupMessageEventArgs e);

        public class FunctionInfo
        {
            public required bool IsEnable { get; set; }
            public required Function Function { get; set; }
        }

        public static Dictionary<string, FunctionInfo> InitDictionary()
        {
            var tempDictionary = new Dictionary<string, FunctionInfo>
            {
                { "#echo", new FunctionInfo { IsEnable = true, Function = BotFunction.Echo } },
                { "#randImg", new FunctionInfo { IsEnable = true, Function = BotFunction.RandomImage } },
                { "#netRandImg", new FunctionInfo { IsEnable = true, Function = BotFunction.GetImgInternet } },
                { "#earthQuake", new FunctionInfo { IsEnable = true, Function = BotFunction.GetLatestEarthQuake } },
                { "#ban", new FunctionInfo { IsEnable = true, Function = BotFunction.Ban } }
            };

            return tempDictionary;
        }

        public static Dictionary<string, FunctionInfo> Functions = InitDictionary();

        public static string[] AnalyzeMessage(string msg)
        {
            ArrayList argv = new(msg.Split(' '));
            foreach (string it in argv)
            {
                if (it == "") argv.Remove(it);
            }

            return (string[])argv.ToArray();
        }

        public static async void OnGroupMessage(object? sender, GroupMessageEventArgs e)
        {
            var tempMsg = e.Message.ToString();

            if (tempMsg[0] != '#') return;
            var argv = AnalyzeMessage(e.Message.ToString());

            // Invoke Function or Reply Not Found
            if(Functions.TryGetValue(argv[0], out var value))
            {
                if (value.IsEnable == false)
                    await e.ReplyAsync(new TextSegment("指令已禁用!"));
                else value.Function.Invoke(argv.ToArray(), e);
            }
            else await e.ReplyAsync(new TextSegment("指令不存在!"));
        }

        public static async void OnPrivateMessage(object? sender, PrivateMessageEventArgs e)
        {
            await e.ReplyAsync(new TextSegment("不支持私聊消息!"));
        }
    }
}
