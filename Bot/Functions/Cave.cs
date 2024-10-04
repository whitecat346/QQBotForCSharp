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
            if ( msg [1] == "del" || msg [1] == "count" )
            {
                await GetCave( msg, eventArgs );
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
            var tableCount = Program.CaveDb.Cave.Count();
            var caveAt     = Random.Shared.Next( 0, tableCount );

            var caveInfo = Program.CaveDb.Cave.Single( cave => cave.ID == caveAt );

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

                var contextInfo = MakeCaveContext( msg.Skip( 2 ).ToArray() );
                if ( SegmentMessage.IsCqCode( contextInfo ) )
                {
                    await eventArgs.ReplyAsync( [
                                                   new AtSegment( eventArgs.UserId ),
                                                   new TextSegment( " 禁止使用 CQ 代码形式的消息（图片，视频等）! "
                                                                  ) // cq code not alloud
                                               ]
                                              );
                    return;
                }

                if ( contextInfo == string.Empty )
                {
                    // context is empty
                    await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( " 内容不能为空！" ) ]
                                              );
                }

                await Program.CaveDb.Cave.AddAsync( new CaveDbStruct
                                                    {
                                                        Context = contextInfo,
                                                        Sender  = eventArgs.Sender.NickName,
                                                        ID      = Program.CaveDb.Cave.Count()
                                                    }
                                                  );
                await Program.CaveDb.SaveChangesAsync();
                await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( " 已添加至回声洞数据库!" ) ]
                                          );
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
                    if ( caveIdResult >= Program.CaveDb.Cave.Count() ) // out of range
                    {
                        await eventArgs.ReplyAsync( [
                                                       new AtSegment( eventArgs.UserId ),
                                                       new TextSegment( " 输入的ID不合法，请重新输入！" )
                                                   ]
                                                  );
                        return;
                    }

                    var waitToRemove = Program.CaveDb.Cave.Single( c => c.ID == caveIdResult );
                    Program.CaveDb.Cave.Remove( waitToRemove );
                    await Program.CaveDb.SaveChangesAsync();
                    await eventArgs.ReplyAsync( [
                                                   new AtSegment( eventArgs.UserId ), new TextSegment( " 已从回声洞数据库中删除!" )
                                               ]
                                              );
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
                    if ( caveIdInt >= Program.CaveDb.Cave.Count() ) // out of range
                    {
                        await eventArgs.ReplyAsync( [
                                                       new AtSegment( eventArgs.UserId ),
                                                       new TextSegment( " 输入的ID不合法，请重新输入！" )
                                                   ]
                                                  );
                        return;
                    }

                    var caveInfo = Program.CaveDb.Cave.Single( cave => cave.ID == caveIdInt );
                    var caveStr = $"""
                                   盗版回声洞({caveIdInt}):

                                   {caveInfo.Context}

                                   -- {caveInfo.Sender}
                                   """;

                    await eventArgs.ReplyAsync( new TextSegment( caveStr ) );
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

                var tableCount = Program.CaveDb.Cave.Count();
                await eventArgs.ReplyAsync( [
                                               new AtSegment( eventArgs.UserId ),
                                               new TextSegment( $"回声洞库共有 {tableCount} 条信息！" )
                                           ]
                                          );
                break;
            }

            #endregion

            default : // argv not found
                await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( "未找到指定的参数！" ) ] );
                break;
        }
    }
}
