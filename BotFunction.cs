using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;
using Makabaka.Models.API.Requests;

namespace QQBotForCSharp
{
    public class NetInfo
    {
        public static async void DeleteImage(string path)
        {
            Thread.Sleep(10000);

            File.Delete(path);
        }
    }

    public class EarthQuakeInfo
    {
        public int ID { get; set; }
        public string EventID { get; set; }
        public string ReportTime { get; set; }
        public int ReportNum { get; set; }
        public string OriginTime { get; set; }
        public string HypoCenter { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Magunitude { get; set; }
        public double MaxIntensity { get; set; }
    }
}

namespace QQBotForCSharp
{
    public class BotFunction
    {
        public static async void Echo(string[] msg, GroupMessageEventArgs eventArgs)
        {
            // #echo info

            if (msg.Length == 1 || msg[1] == "")
            {
                await eventArgs.ReplyAsync(new TextSegment("[ERROR] 请输入指令参数！"));
                return;
            }

            string tempMsg = string.Empty;

            foreach (var s in msg.Skip(1))
            {
                tempMsg += " " + s;
            }

            await eventArgs.ReplyAsync(new TextSegment(tempMsg.Remove(0, 1)));
        }

        public static async void RandomImage(string[] msg, GroupMessageEventArgs eventArgs)
        {
            var path = @"C:\Users\WhiteCAT\Pictures\useForBot";
            try
            {
                var files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);

                var random = new Random();
                var index = random.Next(files.Length);
                await eventArgs.ReplyAsync(new ImageSegment(files[index]));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        public static async void GetImgInternet(string[] msg, GroupMessageEventArgs eventArgs)
        {
            // Create Temp Image Path
            var imagePath = Path.GetTempPath() + @"\BotImageTempFolder";
            Directory.CreateDirectory(imagePath);

            try
            {
                using HttpClient client = new();
                HttpResponseMessage response = await client.GetAsync("https://api.wolfx.jp/img.php");
                response.EnsureSuccessStatusCode();

                // read file info
                var imageBytes = await response.Content.ReadAsByteArrayAsync();

                // save file
                var tempFilePath = imagePath + "\\" + Guid.NewGuid().ToString() + ".jpg";
                using (FileStream fs = new FileStream(tempFilePath, FileMode.Create))
                {
                    fs.Write(imageBytes, 0, imageBytes.Length);
                }

                await eventArgs.ReplyAsync(new ImageSegment(tempFilePath));

                NetInfo.DeleteImage(tempFilePath);
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
        }

        public static async void GetLatestEarthQuake(string[] msg, GroupMessageEventArgs eventArgs)
        {
            var getUrl = "https://api.wolfx.jp/sc_eew.json";

            try
            {
                using (HttpClient client = new())
                {
                    HttpResponseMessage response = await client.GetAsync(getUrl);
                    response.EnsureSuccessStatusCode();

                    var jsonInfo = await response.Content.ReadAsStringAsync();

                    EarthQuakeInfo? info = JsonConvert.DeserializeObject<EarthQuakeInfo>(jsonInfo);

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

        public static async void Ban(string[] msg, GroupMessageEventArgs eventArgs)
        {

        }
    }
}
