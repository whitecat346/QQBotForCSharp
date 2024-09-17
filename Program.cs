﻿using Makabaka.Configurations;
using Makabaka.Models.API.Responses;
using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;
using Makabaka.Services;
using Newtonsoft.Json;
using Serilog;
using System.Runtime.Loader;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using QQBotForCSharp;

namespace QQBotForCSharp
{
    internal class Program
    {
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

            var service = ServiceFactory.CreateForwardWebSocketService(config);
            
            // Config OnMessage Event
            service.OnGroupMessage += QQBotMessage.OnGroupMessage;
            service.OnPrivateMessage += QQBotMessage.OnPrivateMessage;

           var cts = new CancellationTokenSource();
            AssemblyLoadContext.Default.Unloading += ctx => cts.Cancel();
            Console.CancelKeyPress += (sender, eventArgs) => cts.Cancel();

            await service.StartAsync();

            try
            {
                await Task.Delay(Timeout.Infinite, cts.Token);
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine(e);
            }

            await service.StopAsync();
        }
    }
}
