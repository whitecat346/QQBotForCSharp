﻿using Makabaka.Services;
using QQBotForCSharp.DataBase;

namespace QQBotForCSharp
{
    public class Program
    {
        public static IWebSocketService? Service { get; set; }
        public static CaveContext        CaveDb  { get; } = new();

        public static async Task Main( string [ ] args )
        {
            await BotInit.InitBot();

            Console.WriteLine( "WelCome to QQBotForCSharp!" );

            while ( Console.ReadKey().Key != ConsoleKey.Escape )
            {
                Console.WriteLine( "Stopping..." );
                await Service?.StopAsync()!;
            }

            await CaveDb.DisposeAsync();
        }
    }
}
