using System.Collections;
using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;
using QQBotForCSharp.Functions;

namespace QQBotForCSharp
{
    public class QqBotMessage
    {
        public delegate void Function( string [ ] msg, GroupMessageEventArgs e );

        #region AnalyzeMessage

        public static string [ ] AnalyzeMessage( string msg )
        {
            ArrayList argv = new( msg.Split( ' ' ) );
            for ( int i = 0; i < argv.Count; i++ )
            {
                if ( (string)argv [i]! != "" )
                    continue;

                argv.RemoveAt( i );
                i--;
            }

            return (string [ ])argv.ToArray( typeof( string ) );
        }

        #endregion

        public static void OnGroupMessage( object? sender, GroupMessageEventArgs e )
        {
            var tempMsg = e.Message.ToString();

            if ( tempMsg [0] != '#' ) return;
            var argv = AnalyzeMessage( e.Message.ToString() );

            if ( BotInit.GroupBotFunctions.TryGetValue( e.GroupId, out var botFunction ) )
            {
                botFunction.InvokeFuncPtr( argv, e );
            }
            else
            {
                BotInit.GroupBotFunctions.Add( e.GroupId, new BotFunctions() );

                BotInit.GroupBotFunctions [e.GroupId].InvokeFuncPtr( argv, e );
            }
        }

        public static async void OnPrivateMessage( object? sender, PrivateMessageEventArgs e )
        {
            await e.ReplyAsync( new TextSegment( "不支持私聊消息!" ) );
        }

        public static async void OnGroupMemberIncrease( object? sender, GroupMemberIncreaseEventArgs e )
        {
            await e.ReplyAsync( [ new AtSegment( e.UserId ), new TextSegment( "欢迎来到这个群聊哦~" ) ] );
        }
    }
}
