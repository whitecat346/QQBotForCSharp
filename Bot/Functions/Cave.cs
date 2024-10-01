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

        public static async void Cave( string [ ] msg, GroupMessageEventArgs eventArgs )
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
                    await Program.CaveDb.Cave.AddAsync( new CaveDbStruct
                                                        {
                                                            Context = MakeCaveContext( msg.Skip( 2 ).ToArray() ),
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
