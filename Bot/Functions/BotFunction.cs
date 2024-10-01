using System.Reflection;
using AtCode;
using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;
using Newtonsoft.Json;

namespace QQBotForCSharp.Functions;



public partial class BotFunctions
{
    public static async void TestFunc( string [ ] msg, GroupMessageEventArgs eventArgs )
    {
        //if (eventArgs.UserId != 2710458198)
        //    await eventArgs.ReplyAsync( new TextSegment( "非所有者禁止使用！" ) );

        await eventArgs.ReplyAsync( new TextSegment( "Stop Use!" ) );
    }
}
