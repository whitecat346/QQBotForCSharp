﻿using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;

namespace QQBotForCSharp.Functions
{
    public partial class BotFunctions
    {
        public async void Enable( string [ ] msg, GroupMessageEventArgs eventArgs )
        {
            if ( eventArgs.UserId != 2710458198 )
                await eventArgs.ReplyAsync( new TextSegment( "非所有者禁止使用！" ) );

            if ( msg.Length != 2 )
                await eventArgs.ReplyAsync( new TextSegment( "指令格式有误！" ) );

            if ( msg [1] == "enable" || msg [1] == "disable" )
                await eventArgs.ReplyAsync( new TextSegment( "不可对enable或disable方法进行启用或禁用操作！" ) );

            if ( QqBotMessage.Functions.TryGetValue( msg [1], out var value ) )
            {
                if ( value.IsEnable != true )
                {
                    value.IsEnable = true;
                }
                else
                    await eventArgs.ReplyAsync( new TextSegment( $"功能 ${msg [1]} 已经启用！" ) );
            }
            else
            {
                await eventArgs.ReplyAsync( new TextSegment( $"功能 ${msg [1]} 未找到！" ) );
            }
        }
    }
}
