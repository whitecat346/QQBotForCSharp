using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;
using QQBotForCSharp.Bot;

namespace QQBotForCSharp.Functions
{
    public static partial class BotFunctions
    {
        #region FunctionDocument

        private static readonly Dictionary<string, string> FunctionDocumentDictionary = new()
            {
                { "#echo", "复读机 [复读内容]" },
                { "#randImg", "本地随机图片 [无参数]" },
                { "#netRandImg", "网络随机图片 [无参数]" },
                { "#earthQuake", "最新地震 [无参数]" },
                { "#ban", "禁言[禁言用户(需要@对方)] [时间(格式为 1s 或 1m 或 1h 或 1d ；分别为 1秒 或 1分钟 或 1小时 或 1天，数字可更换)]" },
                { "#help", "帮助 [无参数]" },
                { "#unban", "解禁 [解禁用户(需要@对方)]" },
                { "#stopBot", "关闭Bot（只有Bot所有者可以使用）" },
                { "#testFunc", "测试功能" },
                { "#cave", "盗版回声洞" },
                { "#enable", "启用指定功能 " },
                { "#disable", "禁用指定功能 " },
            };

        #endregion

        public static async void Help( string [ ] msg, GroupMessageEventArgs eventArgs )
        {
            if ( msg.Length > 1 )
            {
                if ( FunctionDocumentDictionary.TryGetValue( msg [1], out var value ) )
                {
                    await eventArgs.ReplyAsync( [
                                                   new AtSegment( eventArgs.UserId ), new TextSegment(
                                                        value
                                                       )
                                               ]
                                              );
                    return;
                }
                else
                {
                    await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( "指令不存在!" ) ] );
                    return;
                }
            }

            var functionsMsg = QqBotMessage.Functions.Aggregate( string.Empty,
                                                                 ( current, item ) =>
                                                                     current
                                                                   + ( item.Key
                                                                     + " :\n\t"
                                                                     + FunctionDocumentDictionary [item.Key]
                                                                     + "\n" )
                                                               );

            string remove = functionsMsg.Remove( ( functionsMsg.Length - 1 ), 1 );

            await eventArgs.ReplyAsync( new TextSegment( "当前功能列表:\n" + remove ) );
        }
    }
}
