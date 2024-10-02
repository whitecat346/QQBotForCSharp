using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;

namespace QQBotForCSharp.Functions
{
    public partial class BotFunctions
    {
        public async void Disable( string [ ] msg, GroupMessageEventArgs eventArgs )
        {
            if ( eventArgs.UserId != 2710458198 )
                await eventArgs.ReplyAsync( new TextSegment( "非所有者禁止使用！" ) );

            if ( msg.Length != 2 ) await eventArgs.ReplyAsync( new TextSegment( "格式错误!" ) );

            if ( msg [1] == "enable" || msg [1] == "disable" )
                await eventArgs.ReplyAsync( new TextSegment( "不可对enable或disable方法进行启用或禁用操作！" ) );

            if ( this.FuncPtrInfos.TryGetValue( msg [1], out var value ) )
            {
                if ( value.IsEnable == false )
                    await eventArgs.ReplyAsync( [
                                                   new AtSegment( eventArgs.UserId ),
                                                   new TextSegment( $"功能 {msg [1]} 已经禁用！" )
                                               ]
                                              );
                else
                {
                    var temFuncPtrInfo = this.FuncPtrInfos [msg [1]];
                    temFuncPtrInfo.IsEnable     = false;
                    this.FuncPtrInfos [msg [1]] = temFuncPtrInfo;

                    await eventArgs.ReplyAsync( [
                                                   new AtSegment( eventArgs.UserId ),
                                                   new TextSegment( $" 功能 {msg [1]} 已禁用！" )
                                               ]
                                              );
                }
            }
            else
            {
                await eventArgs.ReplyAsync( new TextSegment( $"功能 {msg [1]} 未找到！" ) );
            }
        }
    }
}
