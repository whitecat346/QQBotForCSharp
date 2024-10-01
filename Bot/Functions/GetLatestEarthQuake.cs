using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;
using Newtonsoft.Json;

#region EarthQuakeInfo

public class EarthQuakeInfo
{
    public required int ID { get; set; }
    public required string EventID { get; set; }
    public required string ReportTime { get; set; }
    public required int ReportNum { get; set; }
    public required string OriginTime { get; set; }
    public required string HypoCenter { get; set; }
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
    public required double Magunitude { get; set; }
    public required double MaxIntensity { get; set; }
}

#endregion

namespace QQBotForCSharp.Functions
{
    public partial class BotFunctions
    {
        public static async void GetLatestEarthQuake( string [ ] msg, GroupMessageEventArgs eventArgs )
        {
            var getUrl = "https://api.wolfx.jp/sc_eew.json";

            try
            {
                using HttpClient client = new();
                using var response = await client.GetAsync( getUrl );
                response.EnsureSuccessStatusCode();

                var jsonInfo = await response.Content.ReadAsStringAsync();

                var info = JsonConvert.DeserializeObject<EarthQuakeInfo>( jsonInfo );

                if (info == null)
                {
                    await eventArgs.ReplyAsync( new TextSegment( "获取报文失败!\nINFO结构体为NULL" ) );
                }
                else
                {
                    var esInfo = "\n最近的一次地震: \n" +
                                 $"EEW发报ID：{info.ID}\n" +
                                 $"EEW发报事件ID：{info.EventID}\n" +
                                 $"EEW发报时间：{info.ReportTime}\n" +
                                 $"EEW发报数：{info.ReportNum}\n" +
                                 $"发震时间：{info.OriginTime}\n" +
                                 $"震源地：{info.HypoCenter}\n" +
                                 $"纬度：{info.Latitude}\n" +
                                 $"经度：{info.Longitude}\n" +
                                 $"震级：{info.Magunitude}\n" +
                                 $"最大烈度：{info.MaxIntensity}";

                    await eventArgs.ReplyAsync( [new AtSegment( eventArgs.Sender.UserId ), new TextSegment( esInfo )] );
                }
            }
            catch (Exception e)
            {
                Console.WriteLine( e );
                await eventArgs.ReplyAsync( new TextSegment( "获取报文失败：\n" + e.Message ) );
            }
        }
    }
}
