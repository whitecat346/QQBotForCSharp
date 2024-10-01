using System.Collections;
using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;
using QQBotForCSharp.Functions;

namespace QQBotForCSharp.Bot
{
    public class QqBotMessage
    {
        public delegate void Function( string [ ] msg, GroupMessageEventArgs e );

        #region FunctionInfo

        public class FunctionInfo
        {
            public required bool     IsEnable { get; set; }
            public required Function Function { get; set; }
        }

        #endregion

        public static Dictionary<string, FunctionInfo> InitDictionary()
        {
            return new Dictionary<string, FunctionInfo>
                   {
                       { "#echo", new FunctionInfo { IsEnable       = true, Function = BotFunctions.Echo } },
                       { "#randImg", new FunctionInfo { IsEnable    = true, Function = BotFunctions.RandomImage } },
                       { "#netRandImg", new FunctionInfo { IsEnable = true, Function = BotFunctions.GetImgInternet } },
                       {
                           "#earthQuake",
                           new FunctionInfo { IsEnable = true, Function = BotFunctions.GetLatestEarthQuake }
                       },
                       { "#ban", new FunctionInfo { IsEnable      = true, Function = BotFunctions.Ban } },
                       { "#help", new FunctionInfo { IsEnable     = true, Function = BotFunctions.Help } },
                       { "#unban", new FunctionInfo { IsEnable    = true, Function = BotFunctions.UnBan } },
                       { "#stopBot", new FunctionInfo { IsEnable  = true, Function = BotFunctions.StopBot } },
                       { "#testFunc", new FunctionInfo { IsEnable = true, Function = BotFunctions.TestFunc } },
                       { "#enable", new FunctionInfo { IsEnable   = true, Function = BotFunctions.Enable } },
                       { "#disable", new FunctionInfo { IsEnable  = true, Function = BotFunctions.Disable } },
                       { "cave", new FunctionInfo { IsEnable      = true, Function = BotFunctions.Cave } }
                   };
        }

        private static readonly Dictionary<string, FunctionInfo> TempDictionary = InitDictionary();
        public static           Dictionary<string, FunctionInfo> Functions      = TempDictionary;

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

        public static async void OnGroupMessage( object? sender, GroupMessageEventArgs e )
        {
            var tempMsg = e.Message.ToString();

            if ( tempMsg [0] != '#' ) return;
            var argv = AnalyzeMessage( e.Message.ToString() );

            // Invoke Function or Reply Not Found
            if ( Functions.TryGetValue( argv [0], out var value ) )
            {
                if ( value.IsEnable == false )
                    await e.ReplyAsync( new TextSegment( "指令已被禁用!" ) );
                else value.Function.Invoke( argv.ToArray(), e );
            }
            else await e.ReplyAsync( new TextSegment( "指令不存在!" ) );
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
