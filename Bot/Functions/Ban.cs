using AtCode;
using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQBotForCSharp.Functions
{
    public static partial class BotFunctions
    {
        public static async void Ban( string [ ] msg, GroupMessageEventArgs eventArgs )
        {
            //var temp = msg.Skip(1).Aggregate(string.Empty, (current, s) => current + (s + " "));
            //await eventArgs.ReplyAsync(new TextSegment(temp));

            // style: #ban @sb. time
            // time: 1d 1h 1m 1s

            if ( eventArgs.Sender.Role != "owner" || eventArgs.Sender.Role != "admin" )
            {
                await eventArgs.ReplyAsync( new TextSegment( "权限不足！请提权！" ) );
                return;
            }

            if ( !SegmentMessage.IsCqCode( msg [1] ) )
            {
                await eventArgs.ReplyAsync( new TextSegment( "请遵循正确的格式！" ) );
                return;
            }

            var atInfo = JsonConvert.DeserializeObject<CodeInfo>( SegmentMessage.GetJObject( msg [1] ) );
            if ( atInfo == null )
            {
                await eventArgs.ReplyAsync( new TextSegment( "序列化JSON时错误!" ) );
                return;
            }

            if ( msg.Length < 3 )
            {
                await eventArgs.ReplyAsync( new TextSegment( "请指定时间!" ) );
                return;
            }

            var time = msg [2];

            switch ( time.Substring( time.Length - 1 ) )
            {
                case "s" :
                    await Program.Service!.Contexts [0]
                                 .MuteGroupMemberAsync(
                                                       eventArgs.GroupId,
                                                       Convert.ToInt64( atInfo!.Data.QQ ),
                                                       Convert.ToInt32( time.Remove( time.Length - 1 ) )
                                                      );
                    break;
                case "m" :
                    await Program.Service!.Contexts [0]
                                 .MuteGroupMemberAsync(
                                                       eventArgs.GroupId,
                                                       Convert.ToInt64( atInfo!.Data.QQ ),
                                                       Convert.ToInt32( time.Remove( time.Length - 1 ) ) * 60
                                                      );
                    break;
                case "h" :
                    await Program.Service!.Contexts [0]
                                 .MuteGroupMemberAsync(
                                                       eventArgs.GroupId,
                                                       Convert.ToInt64( atInfo!.Data.QQ ),
                                                       Convert.ToInt32( time.Remove( time.Length - 1 ) ) * 60 * 60
                                                      );
                    break;
                case "d" :
                    await Program.Service!.Contexts [0]
                                 .MuteGroupMemberAsync(
                                                       eventArgs.GroupId,
                                                       Convert.ToInt64( atInfo!.Data.QQ ),
                                                       Convert.ToInt32( time.Remove( time.Length - 1 ) ) * 60 * 60 * 24
                                                      );
                    break;
                default :
                    await eventArgs.ReplyAsync( new TextSegment( "请遵循正确的格式！" ) );
                    break;
            }
        }
    }
}
