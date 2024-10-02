using System.Runtime.Loader;
using Makabaka.Configurations;
using Makabaka.Services;
using Newtonsoft.Json;
using QQBotForCSharp.Functions;
using Serilog;

namespace QQBotForCSharp
{
    public static class BotInit
    {
        public static Dictionary<long, BotFunctions> GroupBotFunctions =
            new() { { 879243827, new BotFunctions() } };

        public static void GroupManagerInit()
        {
            if ( File.Exists( "groupManager.json" ) ) {}
        }

        public static async Task InitBot()
        {
            // Initialize Logger
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Verbose()
                         .WriteTo.Console()
                         .CreateLogger();

            // Config Service
            ForwardWebSocketServiceConfig? config = null;
            if ( File.Exists( "config.json" ) )
                config
                    = JsonConvert
                        .DeserializeObject<ForwardWebSocketServiceConfig>( await File.ReadAllTextAsync( "config.json" )
                                                                         );

            config ??= new();
            await File.WriteAllTextAsync( "config.json", JsonConvert.SerializeObject( config, Formatting.Indented ) );

            Program.Service = ServiceFactory.CreateForwardWebSocketService( config );

            // Config OnMessage Event
            Program.Service.OnGroupMessage        += QqBotMessage.OnGroupMessage;
            //Program.Service.OnPrivateMessage      += QqBotMessage.OnPrivateMessage;
            Program.Service.OnGroupMemberIncrease += QqBotMessage.OnGroupMemberIncrease;

            var cts = new CancellationTokenSource();
            AssemblyLoadContext.Default.Unloading += ctx => cts.Cancel();
            Console.CancelKeyPress                += ( sender, eventArgs ) => cts.Cancel();

            await Program.Service.StartAsync();
        }
    }
}
