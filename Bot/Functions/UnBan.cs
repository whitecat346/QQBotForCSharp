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
    public partial class BotFunctions
    {
        public async void UnBan( string [ ] msg, GroupMessageEventArgs eventArgs )
        {
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
            await Program.Service?.Contexts [0]
                         .UnmuteGroupMemberAsync( eventArgs.GroupId,
                                                  Convert.ToInt64( atInfo?.Data.QQ )
                                                )!;
        }
    }
}
