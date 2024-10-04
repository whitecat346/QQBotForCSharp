using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;

namespace QQBotForCSharp.Functions;

public partial class BotFunctions
{
    public async void ExceptionTest( string [ ] msg, GroupMessageEventArgs eventArgs )
    {
        try
        {
            if ( eventArgs.UserId != 2710458198 )
            {
                await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( " 非所有者禁止使用！" ) ] );
            }

            throw new Exception( "Test Exception Info" );
        }
        catch ( Exception e )
        {
            await eventArgs.ReplyAsync( [
                                           new AtSegment( eventArgs.UserId ),
                                           new TextSegment( e.Message + '\n' + e.StackTrace )
                                       ]
                                      );
        }
    }
}
