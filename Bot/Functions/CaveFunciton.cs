using QQBotForCSharp.DataBase.Context;

namespace QQBotForCSharp.Functions
{
    public static class CaveFunciton
    {
        public static string Add( string caveContext, string sender )
        {
            if ( SegmentMessage.IsCqCode( caveContext ) )
            {
                return " 禁止使用 CQ 代码形式的消息（图片，视频等）! ";
            }

            if ( caveContext.Length == 0 )
            {
                return " 内容不能为空！";
            }

            if ( Program.CaveDb == null )
            {
                throw new Exception( "CaveDb is null!" );
            }

            Program.CaveDb?.Add( new CaveDbStruct
                                 {
                                     Context = caveContext, Sender = sender, ID = Program.CaveDb.Cave.Count()
                                 }
                               );
            Program.CaveDb?.SaveChanges();
            return $" 添加成功!CaveID:{Program.CaveDb?.Cave.Count() - 1}";
        }

        public static string Delete( int caveId )
        {
            if ( caveId < 0 || caveId >= Program.CaveDb?.Cave.Count() )
            {
                return " 输入ID不合法，请重新输入!";
            }

            var waitToRemove = Program.CaveDb?.Cave.Single( c => c.ID == caveId );
            if ( waitToRemove == null )
            {
                throw new Exception( "waitToRemove is null!" );
            }

            Program.CaveDb?.Cave.Remove( waitToRemove );
            Program.CaveDb?.SaveChanges();

            return $" 已从回声洞数据库中移除{caveId}号消息!";
        }

        public static string At( int caveId )
        {
            if ( caveId < 0 || caveId >= Program.CaveDb?.Cave.Count() )
            {
                return " 输入ID不合法，请重新输入!";
            }

            var caveInfo = Program.CaveDb?.Cave.Single( c => c.ID == caveId );
            var caveStr = $"""
                           盗版回声洞({caveId}):

                           {caveInfo?.Context}

                           -- {caveInfo?.Sender}
                           """;

            return caveStr;
        }

        public static string Count()
        {
            if ( Program.CaveDb == null )
            {
                throw new Exception( "CaveDb is null!" );
            }

            var tableCount = Program.CaveDb?.Cave.Count();

            return $"回声洞库共有 {tableCount} 条信息！";
        }

        public static string Edit( int caveId, string caveContext, string sender )
        {
            if ( Program.CaveDb == null )
            {
                throw new Exception( "CaveDb is null!" );
            }

            //var waitRemove = Program.CaveDb?.Cave.Single( cave => cave.ID == caveId );
            //if ( waitRemove != null )
            //{
            //    Program.CaveDb?.Cave.Remove( waitRemove );
            //    Program.CaveDb?.SaveChanges();
            //}
            //else
            //{
            //    throw new Exception( "WaitRemove is null!" );
            //}

            //var newCaveContext = new CaveDbStruct
            //                     {
            //                         ID = Program.CaveDb?.Cave.Count(), Context = caveContext, Sender = sender
            //                     };
            //Program.CaveDb?.Cave.Add( newCaveContext );

            //Program.CaveDb?.SaveChanges();

            var waitEdit = Program.CaveDb?.Cave.Single( cave => cave.ID == caveId );
            waitEdit.ID      = caveId;
            waitEdit.Context = caveContext;
            waitEdit.Sender  = sender;
            Program.CaveDb?.SaveChanges();

            return $" 编辑成功!新的CaveID:{waitEdit.ID}";
        }
    }
}
