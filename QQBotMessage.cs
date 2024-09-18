using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        public static Dictionary<string, string> FunctionDocument = new Dictionary<string, string>
        {
            { "#echo", "复读机 [复读内容]" },
            { "#randImg", "本地随机图片 [无参数]" },
            { "#netRandImg", "网络随机图片 [无参数]" },
            { "#earthQuake", "最新地震 [无参数]" },
            { "#ban", "禁言[解禁用户(需要@对方)] [时间(格式为 1s 或 1m 或 1h 或 1d ；分别为 1秒 或 1分钟 或 1小时 或 1天，数字可更换)]" },
            { "#help", "帮助 [无参数]" },
            { "#unban", "解禁 [解禁用户(需要@对方)]" }
        };

        public static Dictionary<string, FunctionInfo> InitDictionary()
        {
            return new Dictionary<string, FunctionInfo>
            {
                { "#echo", new FunctionInfo { IsEnable = true, Function = BotFunction.Echo } },
                { "#randImg", new FunctionInfo { IsEnable = true, Function = BotFunction.RandomImage } },
                { "#netRandImg", new FunctionInfo { IsEnable = true, Function = BotFunction.GetImgInternet } },
                { "#earthQuake", new FunctionInfo { IsEnable = true, Function = BotFunction.GetLatestEarthQuake } },
                { "#ban", new FunctionInfo { IsEnable = true, Function = BotFunction.Ban } },
                { "#help", new FunctionInfo { IsEnable = true, Function = BotFunction.Help } },
                { "#unban", new FunctionInfo { IsEnable = true, Function = BotFunction.UnBan } }
            };
        }

        private static readonly Dictionary<string, FunctionInfo> TempDictionary = InitDictionary();
        public static Dictionary<string, FunctionInfo> Functions = TempDictionary;

        public static string[] AnalyzeMessage(string msg)
        {
            ArrayList argv = new(msg.Split(' '));
            for (int i = 0; i < argv.Count; i++)
            {
                if ((string)argv[i]! == "")
                {
                    argv.RemoveAt(i);
                    i--;
                }
            }

            return (string[]) argv.ToArray(typeof(string));
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

        public static async void OnGroupMemberIncrease( object? sender, GroupMemberIncreaseEventArgs e )
        {
            await e.ReplyAsync( [new AtSegment( e.UserId ), new TextSegment( "欢迎来到这个群聊哦~" )] );
        }
    }
}
