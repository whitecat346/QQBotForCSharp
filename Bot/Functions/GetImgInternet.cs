using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QQBotForCSharp.Functions
{
    public partial class BotFunctions
    {
        public async void GetImgInternet( string [ ] msg, GroupMessageEventArgs eventArgs )
        {
            // Create Temp Image Path
            var imagePath = new string( Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ) )
                          + @"\BotImageTempFolder";
            Directory.CreateDirectory( imagePath );

            try
            {
                using HttpClient client   = new();
                using var        response = await client.GetAsync( "https://api.wolfx.jp/img.php" );
                response.EnsureSuccessStatusCode();

                // read file info
                var imageBytes = await response.Content.ReadAsByteArrayAsync();

                // save file
                var             tempFilePath = imagePath + "\\" + Guid.NewGuid() + ".jpg";
                await using var fs           = new FileStream( tempFilePath, FileMode.Create );
                {
                    fs.Write( imageBytes, 0, imageBytes.Length );
                }

                // Delete Image File
                Thread.Sleep( 10000 );
                File.Delete( tempFilePath );
            }
            catch ( IOException ioException )
            {
                Console.WriteLine( ioException );
                await eventArgs.ReplyAsync( new TextSegment( "I/O操作错误：\n" + ioException.Message ) );
            }
            catch ( HttpRequestException e )
            {
                Console.WriteLine( e );
                await eventArgs.ReplyAsync( new TextSegment( "网络图片获取失败\n" + e.Message ) );
            }
            catch ( UnauthorizedAccessException e )
            {
                Console.WriteLine( e );
                await eventArgs.ReplyAsync( new TextSegment( "访问被拒绝：\n" + e.Message ) );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e );
                await eventArgs.ReplyAsync( new TextSegment( "未知错误：\n" + e.Message ) );
            }
        }
    }
}
