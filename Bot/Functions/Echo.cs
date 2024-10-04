using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQBotForCSharp.Functions
{
    public partial class BotFunctions
    {
        public async void Echo( string [ ] msg, GroupMessageEventArgs eventArgs )
        {
            // #echo info

            if ( msg.Length == 1 || msg [1] == "" )
            {
                await eventArgs.ReplyAsync( new TextSegment( "[ERROR] 请输入指令参数！" ) );
                return;
            }

            var tempMsg = msg.Skip( 1 ).Aggregate( string.Empty, ( current, s ) => current + ( s + ' ' ) );

            await eventArgs.ReplyAsync( new TextSegment( tempMsg.Remove( 0, 1 ) ) );
        }
    }
}
