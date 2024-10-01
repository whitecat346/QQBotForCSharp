using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;
using QQBotForCSharp.DataBase.Context;

namespace QQBotForCSharp.Functions
{
    public static partial class BotFunctions
    {
        private static string MakeCaveContext( string [ ] msg )
        {
            return msg.Aggregate( string.Empty, ( current, s ) => current + ( " " + s ) );
        }

        private static System.Timers.Timer _timer = new( 60000 );

        public static async void Cave( string [ ] msg, GroupMessageEventArgs eventArgs )
        {
            if ( _timer.Enabled ) { return; }

            GetCave( msg, eventArgs );
            _timer.Enabled = true;
            _timer.Elapsed += ( sender, e ) =>
                              {
                                  _timer.Enabled = false;
                              };
            _timer.Start();
        }

        public static async void GetCave( string [ ] msg, GroupMessageEventArgs eventArgs )
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
            }

            if ( msg.Length < 3 )
            {
                await eventArgs.ReplyAsync( new TextSegment( "格式错误！" ) );
            }

            switch ( msg [1] )
            {
                case "add" :
                    var contextInfo = MakeCaveContext( msg.Skip( 2 ).ToArray() );
                    if ( SegmentMessage.IsCqCode( contextInfo ) )
                    {
                        await eventArgs.ReplyAsync( [
                                                       new AtSegment( eventArgs.UserId ),
                                                       new TextSegment( " 禁止使用 CQ 代码形式的消息（图片，视频等）! " )
                                                   ]
                                                  );
                        return;
                    }

                    if ( contextInfo == string.Empty )
                    {
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
                case "del" :
                    var caveId = msg [2];
                    if ( int.TryParse( caveId, out int caveIdResult ) )
                    {
                        var waitToRemove = Program.CaveDb.Cave.Single( c => c.ID == caveIdResult );
                        Program.CaveDb.Cave.Remove( waitToRemove );
                        await Program.CaveDb.SaveChangesAsync();
                        await eventArgs.ReplyAsync( [
                                                       new AtSegment( eventArgs.UserId ),
                                                       new TextSegment( " 已从回声洞数据库中删除!" )
                                                   ]
                                                  );
                    }
                    else
                    {
                        await eventArgs.ReplyAsync( new TextSegment( "输入的ID不合法，请重新输入！" ) );
                    }

                    break;
                default :
                    await eventArgs.ReplyAsync( new TextSegment( "命令未找到！" ) );
                    break;
            }
        }
    }
}
