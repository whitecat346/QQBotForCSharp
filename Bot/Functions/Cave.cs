using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;
using QQBotForCSharp.DataBase.Context;

namespace QQBotForCSharp.Functions;

public partial class BotFunctions
{
    private static string MakeCaveContext( string [ ] msg )
    {
        return msg.Aggregate( string.Empty, ( current, s ) => current + ( " " + s ) );
    }

    private readonly System.Timers.Timer _timer = new( 10000 );

    public async void Cave( string [ ] msg, GroupMessageEventArgs eventArgs )
    {
        if ( msg.Length != 1 )
        {
            // if parma is speacial command
            if ( msg [1] is "del" or "count" or "edit" )
            {
                await GetCave( msg, eventArgs );
                goto setTimer;
            }
        }

        if ( _timer.Enabled )
        {
#if DEBUG
            await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( " Cave冷却中，请稍后再试！" ) ] );
#endif
            return;
        }
        else await GetCave( msg, eventArgs );

    setTimer:

        _timer.Enabled = true;
        _timer.Elapsed += ( _, _ ) =>
                          {
                              _timer.Enabled = false;
                          };
        _timer.Start();
    }

    private async Task GetCave( string [ ] msg, GroupMessageEventArgs eventArgs )
    {
        if ( msg.Length == 1 )
        {
            if ( Program.CaveDb == null ) { throw new Exception( "CaveDb is null!" ); }

            var tableCount = Program.CaveDb.Cave.Count();
            var caveAt     = Random.Shared.Next( 0, tableCount );
            var caveInfo   = Program.CaveDb.Cave.Single( cave => cave.ID == caveAt );
            var caveStr = $"""
                           盗版回声洞({caveAt}):

                           {caveInfo.Context}

                           -- {caveInfo.Sender}
                           """;

            await eventArgs.ReplyAsync( new TextSegment( caveStr ) );

            return;

            //case 2 : // form error
            //{
            //    await eventArgs.ReplyAsync( new TextSegment( "格式错误！" ) );
            //    return;
            //}
        }

        switch ( msg [1] )
        {
            #region AddFunction

            case "add" :
            {
                if ( msg.Length < 3 )
                {
                    await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( "  格式错误！" ) ] );
                    return;
                }

                string contextInfo = MakeCaveContext( msg.Skip( 2 ).ToArray() );

                var replyMsg = CaveFunciton.Add( contextInfo, eventArgs.Sender.NickName );

                await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( replyMsg ) ] );

                break;
            }

            #endregion

            #region DelFunction

            case "del" :
            {
                if ( msg.Length < 3 )
                {
                    await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( " 格式错误！" ) ] );
                    return;
                }

                var caveId = msg [2];
                if ( int.TryParse( caveId, out int caveIdResult ) ) // not a number
                {
                    var replyMsg = CaveFunciton.Delete( caveIdResult );

                    await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( replyMsg ) ] );
                }
                else
                {
                    await eventArgs.ReplyAsync( new TextSegment( "输入的ID不合法，请重新输入！" ) );
                }

                break;
            }

            #endregion

            #region AtFunction

            case "at" :
            {
                if ( msg.Length < 3 )
                {
                    await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( " 格式错误！" ) ] );
                }

                var caveId = msg [2];
                if ( int.TryParse( caveId, out int caveIdInt ) ) // not a number
                {
                    var replyMsg = CaveFunciton.At( caveIdInt );

                    await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( replyMsg ) ] );
                }

                break;
            }

            #endregion

            #region CountFunction

            case "count" :
            {
                if ( msg.Length > 2 )
                {
                    await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( " 包含多余参数！" ) ] );
                    return;
                }

                var replyMsg = CaveFunciton.Count();

                await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( replyMsg ) ] );

                break;
            }

            #endregion

            #region EditFunction

            case "edit" :
            {
                if ( eventArgs.UserId != 2710458198 )
                {
                    await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( " 非所有者禁止使用!" ) ]
                                              );
                    return;
                }
                // 0     1    2 3      4       5     6
                // #cave edit 0 sender context hello wrold

                if ( msg.Length < 5 )
                {
                    await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( " 参数不足!" ) ] );
                    return;
                }

                if ( int.TryParse( msg [2], out int result ) )
                {
                    var sender  = msg [3];
                    var context = msg.Skip( 4 ).Aggregate( string.Empty, ( s, s1 ) => s + ( s1 + ' ' ) );

                    var response = CaveFunciton.Edit( result, context, sender );
                    await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( response ) ] );
                }
                else
                {
                    await eventArgs.ReplyAsync( [
                                                   new AtSegment( eventArgs.UserId ),
                                                   new TextSegment( " 输入的ID不合法，请重新输入！" )
                                               ]
                                              );
                    return;
                }

                break;
            }

            #endregion

            default : // argv not found
                await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( "未找到指定的参数！" ) ] );
                break;
        }
    }
}
