using Makabaka.Services;
using QQBotForCSharp.Bot;
using QQBotForCSharp.DataBase;

namespace QQBotForCSharp
{
    public class Program
    {
        public static IWebSocketService? Service;
        public static CaveDbContext      caveDb = new();

        public static async Task Main( string [ ] args )
        {
            await BotInit.InitBot();

            Console.WriteLine( "WelCome to QQBotForCSharp!" );

            while ( Console.ReadKey().Key != ConsoleKey.Escape )
            {
                Console.WriteLine( "Stopping..." );
                await Service.StopAsync();
            }

            await caveDb.DisposeAsync();
        }
    }
}
