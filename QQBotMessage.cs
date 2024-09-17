using System;
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

        public static Dictionary<string, Function> InitDictionary()
        {
            var tempDictionary = new Dictionary<string, Function>();
            tempDictionary.Add("#echo", BotFunction.Echo);
            tempDictionary.Add("#randImg", BotFunction.RandomImage);
            tempDictionary.Add("#netRandImg", BotFunction.GetImgInternet);
            tempDictionary.Add("#earthQuake", BotFunction.GetLatestEarthQuake);

            return tempDictionary;
        }

        public static string[] AnalyzeMessage(string msg)
        {
            return msg.Split(' ');
        }

        public static async void OnGroupMessage(object? sender, GroupMessageEventArgs e)
        {
            var tempMsg = e.Message.ToString();
            if (tempMsg[0] == '#')
            {
                Dictionary<string, Function> functions = InitDictionary();

                var argv = AnalyzeMessage(e.Message.ToString());
                if(functions.ContainsKey(argv[0])) functions[argv[0]].Invoke(argv.ToArray(), e);
                else await e.ReplyAsync(new TextSegment("指令不存在!"));
            }
        }
    }
}
