using Newtonsoft.Json;

namespace QQBotForCSharp
{
    public class ConfigStruct
    {
        public required string CaveDb { set; get; } = @"G:\QQBotForCSharp\Cave.db";
    }

    public class Config
    {
        public static string? CaveDbPath { get; set; }

        public static void LoadConfig( string path )
        {
            if ( !File.Exists( path ) )
                throw new Exception( "botConfig.json 不存在!" );

            var configFile = File.ReadAllText( "botConfig.json" );
            var configJson = JsonConvert.DeserializeObject<ConfigStruct>( configFile );

            CaveDbPath = configJson?.CaveDb.Replace( "/", "\\" );
        }
    }
}
