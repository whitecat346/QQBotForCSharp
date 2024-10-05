using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;

namespace QQBotForCSharp.Functions;

public partial class BotFunctions
{
    public delegate void FuncPtr( string [ ] msg, GroupMessageEventArgs eventArgs );

    public struct FuncPtrInfo
    {
        public required FuncPtr Func { set; get; }

        public required bool IsEnable { set; get; }
    }

    internal Dictionary<string, FuncPtrInfo> FuncPtrInfos;

    public BotFunctions()
    {
        FuncPtrInfos = new Dictionary<string, FuncPtrInfo>
                       {
                           { "#echo", new FuncPtrInfo { IsEnable          = true, Func = this.Echo } },
                           { "#randImg", new FuncPtrInfo { IsEnable       = true, Func = this.RandomImage } },
                           { "#netRandImg", new FuncPtrInfo { IsEnable    = true, Func = this.GetImgInternet } },
                           { "#earthQuake", new FuncPtrInfo { IsEnable    = true, Func = this.GetLatestEarthQuake } },
                           { "#ban", new FuncPtrInfo { IsEnable           = true, Func = this.Ban } },
                           { "#help", new FuncPtrInfo { IsEnable          = true, Func = this.Help } },
                           { "#unban", new FuncPtrInfo { IsEnable         = true, Func = this.UnBan } },
                           { "#stopBot", new FuncPtrInfo { IsEnable       = true, Func = this.StopBot } },
                           { "#enable", new FuncPtrInfo { IsEnable        = true, Func = this.Enable } },
                           { "#disable", new FuncPtrInfo { IsEnable       = true, Func = this.Disable } },
                           { "#cave", new FuncPtrInfo { IsEnable          = true, Func = this.Cave } },
                           { "#mcver", new FuncPtrInfo { IsEnable         = true, Func = this.McVersion } },
                           { "#exceptionTest", new FuncPtrInfo { IsEnable = true, Func = this.ExceptionTest } },
                           { "#chat", new FuncPtrInfo { IsEnable          = true, Func = this.Llm } }
                       };
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

    public bool SetFuncState( string funcName, bool enable )
    {
        if ( FuncPtrInfos.TryGetValue( funcName, out FuncPtrInfo _ ) )
        {
            var tempDic = FuncPtrInfos [funcName];
            tempDic.IsEnable        = enable;
            FuncPtrInfos [funcName] = tempDic;
        }
        else
        {
            throw new Exception( "未找到该功能!" );
        }

        return true;
    }

    public bool GetFuncState( string funcName )
    {
        if ( FuncPtrInfos.TryGetValue( funcName, out FuncPtrInfo info ) )
        {
            return info.IsEnable;
        }
        else
        {
            throw new Exception( "未找到该功能!" );
        }
    }
}
