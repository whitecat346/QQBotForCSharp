using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQBotForCSharp.Functions
{
    public static partial class BotFunctions
    {
        public static async void RandomImage( string [ ] msg, GroupMessageEventArgs eventArgs )
        {
            var path = @"C:\Users\WhiteCAT\Pictures\useForBot";
            if ( path == null ) throw new ArgumentNullException( nameof( path ) );
            try
            {
                var files = Directory.GetFiles( path, "*.*", SearchOption.TopDirectoryOnly );

                var index = new Random().Next( 0, files.Length );
                await eventArgs.ReplyAsync( new ImageSegment( files [index] ) );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e );
                await eventArgs.ReplyAsync( new TextSegment( "I/O操作错误：\n" + e.Message ) );
            }
        }
    }
}
