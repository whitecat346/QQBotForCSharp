using System.Runtime.Loader;
using Makabaka.Configurations;
using Makabaka.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QQBotForCSharp.Functions;
using Serilog;

namespace QQBotForCSharp;

public static class BotInit
{
    public static Dictionary<long, BotFunctions> GroupBotFunctions = new();

    public static void GroupManagerInit()
    {
        if ( !File.Exists( "groupManager.json" ) ) { return; }

        var groupManager = JArray.Parse( File.ReadAllText( "groupManager.json" ) );
        foreach ( var valuePair in groupManager )
        {
            var     tempBotFunctions = new BotFunctions();
            JObject info             = (JObject)valuePair;
            JObject funcInfo         = (JObject)( info ["funcInfo"] ?? throw new InvalidOperationException() );
            long    groupId          = (long)( info ["groupId"] ?? throw new InvalidOperationException() );

            foreach ( string funcName in BotFunctionsInfo.AllFunctionNameList )
            {
                if ( funcInfo.TryGetValue( funcName, out var funcInfoValue ) )
                    tempBotFunctions.SetFuncState( funcName, (bool)funcInfoValue );
                else
                    throw new Exception( "未找到该功能!" );
            }

            GroupBotFunctions.Add( groupId, tempBotFunctions );
        }

        Console.WriteLine( "Loaded!" );
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
        Program.Service.OnGroupMessage += QqBotMessage.OnGroupMessage;
        //Program.Service.OnPrivateMessage      += QqBotMessage.OnPrivateMessage;
        Program.Service.OnGroupMemberIncrease += QqBotMessage.OnGroupMemberIncrease;

        var cts = new CancellationTokenSource();
        AssemblyLoadContext.Default.Unloading += _ => cts.Cancel();
        Console.CancelKeyPress                += ( _, _ ) => cts.Cancel();


        GroupManagerInit();

        await Program.Service.StartAsync();
    }
}
