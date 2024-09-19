using System.Reflection;
using AtCode;
using Newtonsoft.Json;
using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;

namespace QQBotForCSharp.Functions;

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

public class BotFunction
{
    #region Echo
    public static async void Echo(string[] msg, GroupMessageEventArgs eventArgs)
    {
        // #echo info

        if (msg.Length == 1 || msg[1] == "")
        {
            await eventArgs.ReplyAsync(new TextSegment("[ERROR] 请输入指令参数！"));
            return;
        }

        var tempMsg = msg.Skip(1).Aggregate(string.Empty, (current, s) => current + (" " + s));

        await eventArgs.ReplyAsync(new TextSegment(tempMsg.Remove(0, 1)));
    }
    #endregion

    #region RandomImage

    public static async void RandomImage(string[] msg, GroupMessageEventArgs eventArgs)
    {
        var path = @"C:\Users\WhiteCAT\Pictures\useForBot";
        if (path == null) throw new ArgumentNullException(nameof(path));
        try
        {
            var files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);

            var index = new Random().Next(0, files.Length);
            await eventArgs.ReplyAsync(new ImageSegment(files[index]));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await eventArgs.ReplyAsync(new TextSegment("I/O操作错误：\n" + e.Message));
        }
    }

    #endregion

    #region GetImgInternet

    public static async void GetImgInternet(string[] msg, GroupMessageEventArgs eventArgs)
    {
        // Create Temp Image Path
        var imagePath = new string(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)) + @"\BotImageTempFolder";
        Directory.CreateDirectory(imagePath);

        try
        {
            using HttpClient client = new();
            using var response = await client.GetAsync("https://api.wolfx.jp/img.php");
            response.EnsureSuccessStatusCode();

            // read file info
            var imageBytes = await response.Content.ReadAsByteArrayAsync();

            // save file
            var tempFilePath = imagePath + "\\" + Guid.NewGuid() + ".jpg";
            await using var fs = new FileStream(tempFilePath, FileMode.Create);
            {
                fs.Write( imageBytes, 0, imageBytes.Length );
            }

            // Delete Image File
            Thread.Sleep(10000);
            File.Delete(tempFilePath);
        }
        catch (IOException ioException)
        {
            Console.WriteLine(ioException);
            await eventArgs.ReplyAsync(new TextSegment("I/O操作错误：\n" + ioException.Message));
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            await eventArgs.ReplyAsync(new TextSegment("网络图片获取失败\n" + e.Message));
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine(e);
            await eventArgs.ReplyAsync(new TextSegment("访问被拒绝：\n" + e.Message));
        }
        catch (Exception e) 
        {
            Console.WriteLine(e);
            await eventArgs.ReplyAsync(new TextSegment("未知错误：\n" + e.Message));
        }
    }

    #endregion

    #region GetLatestEarthQuake

    public static async void GetLatestEarthQuake(string[] msg, GroupMessageEventArgs eventArgs)
    {
        var getUrl = "https://api.wolfx.jp/sc_eew.json";

        try
        {
            using HttpClient client = new();
            using var response = await client.GetAsync(getUrl);
            response.EnsureSuccessStatusCode();

            var jsonInfo = await response.Content.ReadAsStringAsync();

            var info = JsonConvert.DeserializeObject<EarthQuakeInfo>(jsonInfo);

            if (info == null)
            {
                await eventArgs.ReplyAsync(new TextSegment("获取报文失败!\nINFO结构体为NULL"));
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

                await eventArgs.ReplyAsync([new AtSegment(eventArgs.Sender.UserId), new TextSegment(esInfo)]);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await eventArgs.ReplyAsync(new TextSegment("获取报文失败：\n" + e.Message));  
        }
    }

    #endregion

    #region Ban

    public static async void Ban(string[] msg, GroupMessageEventArgs eventArgs)
    {
        //var temp = msg.Skip(1).Aggregate(string.Empty, (current, s) => current + (s + " "));
        //await eventArgs.ReplyAsync(new TextSegment(temp));

        // style: #ban @sb. time
        // time: 1d 1h 1m 1s

        if (eventArgs.Sender.Role != "owner" || eventArgs.Sender.Role != "admin")
        {
            await eventArgs.ReplyAsync( new TextSegment( "权限不足！请提权！" ) );
            return;
        }

        if (!SegmentMessage.IsCqCode( msg [1] ))
        {
            await eventArgs.ReplyAsync( new TextSegment( "请遵循正确的格式！" ) );
            return;
        }

        var atInfo = JsonConvert.DeserializeObject<CodeInfo>(SegmentMessage.GetJObject( msg [1] ));
        if (atInfo == null)
        {
            await eventArgs.ReplyAsync( new TextSegment( "序列化JSON时错误!" ) );
            return;
        }

        if (msg.Length < 3)
        {
            await eventArgs.ReplyAsync(new TextSegment("请指定时间!"));
            return;
        }

        var time = msg[2];

        switch (time.Substring(time.Length - 1))
        {
            case "s":
                await Program.Service!.Contexts [0] .MuteGroupMemberAsync(
                    eventArgs.GroupId,
                    Convert.ToInt64( atInfo!.Data.QQ ),
                    Convert.ToInt32( time.Remove( time.Length - 1 ) ));
                break;
            case "m":
                await Program.Service!.Contexts [0] .MuteGroupMemberAsync(
                    eventArgs.GroupId,
                    Convert.ToInt64( atInfo!.Data.QQ ),
                    Convert.ToInt32( time.Remove( time.Length - 1 ) ) * 60);
                break;
            case "h":
                await Program.Service!.Contexts [0].MuteGroupMemberAsync(
                    eventArgs.GroupId,
                    Convert.ToInt64( atInfo!.Data.QQ ),
                    Convert.ToInt32( time.Remove( time.Length - 1 ) ) * 60 * 60 );
                break;
            case "d":
                await Program.Service!.Contexts [0] .MuteGroupMemberAsync(
                    eventArgs.GroupId,
                    Convert.ToInt64(atInfo!.Data.QQ ),
                    Convert.ToInt32( time.Remove( time.Length - 1 ) ) * 60 * 60 * 24);
                break;
            default:
                await eventArgs.ReplyAsync( new TextSegment( "请遵循正确的格式！" ) );
                break;
        }
    }

    #endregion

    #region UnBan

    public static async void UnBan( string [ ] msg, GroupMessageEventArgs eventArgs )
    {
        if (eventArgs.Sender.Role != "owner" || eventArgs.Sender.Role != "admin")
        {
            await eventArgs.ReplyAsync( new TextSegment( "权限不足！请提权！" ) );
            return;
        }

        if (!SegmentMessage.IsCqCode( msg [ 1 ] ))
        {
            await eventArgs.ReplyAsync(new TextSegment("请遵循正确的格式！"));
            return;
        }

        var atInfo = JsonConvert.DeserializeObject<CodeInfo>(SegmentMessage.GetJObject( msg [ 1 ] ));
        await Program.Service.Contexts [ 0 ] .UnmuteGroupMemberAsync( eventArgs.GroupId, 
            Convert.ToInt64( atInfo.Data.QQ ) );
    }

    #endregion

    #region Help

    public static async void Help( string[] msg, GroupMessageEventArgs eventArgs )
    {
        if (msg.Length > 1)
        {
            if (QQBotMessage.FunctionDocument.TryGetValue(msg[1], out var value))
            {

                await eventArgs.ReplyAsync( [
                    new AtSegment( eventArgs.UserId ), new TextSegment(
                        value )
                ] );
                return;
            }
            else
            {
                await eventArgs.ReplyAsync([new AtSegment(eventArgs.UserId), new TextSegment("指令不存在!")]);
                return;
            }
        }

        var functionsMsg = QQBotMessage.Functions.Aggregate( string.Empty,
            ( current, item ) => current + (item.Key + " :\n\t" + QQBotMessage.FunctionDocument [item.Key] + "\n") );

        string remove = functionsMsg.Remove( (functionsMsg.Length - 1), 1 );

        await eventArgs.ReplyAsync( new TextSegment( "当前功能列表:\n" + remove) );
    }

    #endregion

    #region StopBot

    public static async void StopBot( string [ ] msg, GroupMessageEventArgs eventArgs )
    {
        if (eventArgs.UserId == 2710458198)
        {
            await eventArgs.ReplyAsync( new TextSegment( "Bot即将关机!" ) );
            await Program.Service!.StopAsync();
            Environment.Exit( 0 );
        }
        else
        {
            await eventArgs.ReplyAsync( [new AtSegment( eventArgs.UserId ), new TextSegment( "你没有权限关机!" )] );
        }
    }

    #endregion
}
