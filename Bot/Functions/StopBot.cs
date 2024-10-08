﻿using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;

namespace QQBotForCSharp.Functions
{
    public partial class BotFunctions
    {
        public async void StopBot( string [ ] msg, GroupMessageEventArgs eventArgs )
        {
            if ( eventArgs.UserId == 2710458198 )
            {
                await eventArgs.ReplyAsync( new TextSegment( "Bot即将关机!" ) );
                await Program.Service!.StopAsync();

                BotQuit.SaveBotState();

                Environment.Exit( 0 );
            }
            else
            {
                await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( "你没有权限关机!" ) ] );
            }
        }
    }
}
