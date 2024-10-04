using System.Diagnostics.CodeAnalysis;
using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;
using Newtonsoft.Json;

namespace QQBotForCSharp.Functions;

public partial class BotFunctions
{
    #region McVerisonJson

    [SuppressMessage( "ReSharper", "InconsistentNaming" )]
    private class McVersionLatest
    {
        public required string release  { get; set; }
        public required string snapshot { get; set; }
    }

    [SuppressMessage( "ReSharper", "InconsistentNaming" )]
    private class McVersionInfo
    {
        public required string id          { get; set; }
        public required string type        { get; set; }
        public required string url         { get; set; }
        public required string time        { get; set; }
        public required string releaseTime { get; set; }
    }

    [SuppressMessage( "ReSharper", "InconsistentNaming" )]
    private class McVersionJsonStruct
    {
        public required McVersionLatest     latest   { get; set; }
        public required List<McVersionInfo> versions { get; set; }
    }

    #endregion

    public async void McVersion( string [ ] msg, GroupMessageEventArgs eventArgs )
    {
        try
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response
                = await client.GetAsync( "https://launchermeta.mojang.com/mc/game/version_manifest.json" );

            response.EnsureSuccessStatusCode();

            var responseInfo = await response.Content.ReadAsStringAsync();

            var mcVersionInfo = JsonConvert.DeserializeObject<McVersionJsonStruct>( responseInfo );

            var mcVersion = $"""
                             当前MC最新正式版本:
                             {mcVersionInfo?.latest.release}
                             当前MC最新预览版本:
                             {mcVersionInfo?.latest.snapshot}
                             """;

            await eventArgs.ReplyAsync( new TextSegment( mcVersion ) );
        }
        catch ( HttpRequestException httpRequestException )
        {
            await eventArgs.ReplyAsync( new TextSegment( httpRequestException.Message
                                                       + '\n'
                                                       + httpRequestException.StackTrace
                                                       )
                                      );

            Console.WriteLine( httpRequestException );
        }
    }
}
