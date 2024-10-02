using System.Reflection;
using AtCode;
using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;
using Newtonsoft.Json;

namespace QQBotForCSharp.Functions;

public partial class BotFunctions
{
    public delegate void FuncPtr( string [ ] msg, GroupMessageEventArgs eventArgs );

    public struct FuncPtrInfo
    {
        public required FuncPtr Func { set; get; }

        public required bool IsEnable { set; get; }
    }

    internal readonly Dictionary<string, FuncPtrInfo>? FuncPtrInfos;

    public BotFunctions( Dictionary<string, FuncPtrInfo>? funcPtrInfo = null )
    {
        if ( funcPtrInfo is null )
        {
            FuncPtrInfos = new Dictionary<string, FuncPtrInfo>
                           {
                               { "#echo", new FuncPtrInfo { IsEnable       = true, Func = this.Echo } },
                               { "#randImg", new FuncPtrInfo { IsEnable    = true, Func = this.RandomImage } },
                               { "#netRandImg", new FuncPtrInfo { IsEnable = true, Func = this.GetImgInternet } },
                               { "#earthQuake", new FuncPtrInfo { IsEnable = true, Func = this.GetLatestEarthQuake } },
                               { "#ban", new FuncPtrInfo { IsEnable        = true, Func = this.Ban } },
                               { "#help", new FuncPtrInfo { IsEnable       = true, Func = this.Help } },
                               { "#unban", new FuncPtrInfo { IsEnable      = true, Func = this.UnBan } },
                               { "#stopBot", new FuncPtrInfo { IsEnable    = true, Func = this.StopBot } },
                               { "#enable", new FuncPtrInfo { IsEnable     = true, Func = this.Enable } },
                               { "#disable", new FuncPtrInfo { IsEnable    = true, Func = this.Disable } },
                               { "#cave", new FuncPtrInfo { IsEnable       = true, Func = this.Cave } }
                           };
        }
        else
        {
            FuncPtrInfos = funcPtrInfo;
        }
    }

    public async void InvokeFuncPtr( string [ ] msg, GroupMessageEventArgs eventArgs )
    {
        if ( FuncPtrInfos.TryGetValue( msg [0], out FuncPtrInfo funcPtrInfo ) )
        {
            if ( funcPtrInfo.IsEnable == false )
                await eventArgs.ReplyAsync( new TextSegment( "指令已被禁用!" ) );
            else funcPtrInfo.Func.Invoke( msg.ToArray(), eventArgs );
        }
        else
        {
            await eventArgs.ReplyAsync( new TextSegment( "指令不存在!" ) );
        }
    }
}
