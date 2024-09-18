﻿using Makabaka.Configurations;
using Makabaka.Models.API.Responses;
using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;
using Makabaka.Services;
using Newtonsoft.Json;
using Serilog;
using System.Runtime.Loader;
 using Makabaka.Network;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using QQBotForCSharp;

namespace QQBotForCSharp
{

    public class Program
    {
        public static IWebSocketService? Service;

        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .CreateLogger();
            ForwardWebSocketServiceConfig? config = null;
            if (File.Exists("config.json"))
            {
                config = JsonConvert.DeserializeObject<ForwardWebSocketServiceConfig>(await File.ReadAllTextAsync("config.json"));
            }

            config ??= new();
            await File.WriteAllTextAsync("config.json", JsonConvert.SerializeObject(config, Formatting.Indented));

            Service = ServiceFactory.CreateForwardWebSocketService(config);
            
            // Config OnMessage Event
            Service.OnGroupMessage += QQBotMessage.OnGroupMessage;
            Service.OnPrivateMessage += QQBotMessage.OnPrivateMessage;
            Service.OnGroupMemberIncrease += QQBotMessage.OnGroupMemberIncrease;

          var cts = new CancellationTokenSource();
            AssemblyLoadContext.Default.Unloading += ctx => cts.Cancel();
            Console.CancelKeyPress += (sender, eventArgs) => cts.Cancel();

            await Service.StartAsync();

            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                Console.WriteLine("Stopping...");
                await Service.StopAsync();
            }

        }
    }
}
