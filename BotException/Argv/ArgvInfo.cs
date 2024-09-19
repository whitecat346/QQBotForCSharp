using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQBotForCSharp.BotException.Argv
{
    #region FunctionArgvInfo

    public struct FunctionArgvInfo
    {
        public int Count { get; set; }
    }

    #endregion

    public class ArgvInfo
    {
        public static readonly Dictionary<string, FunctionArgvInfo> FunctionArgvInfosDictionary = new()
        {
            { "#echo", new FunctionArgvInfo() { Count = int.MaxValue } },
            { "#randImg", new FunctionArgvInfo() { Count = 0 } },
            { "#netRandImg", new FunctionArgvInfo() { Count = 0 } },
            { "#ban", new FunctionArgvInfo() { Count = 2 } },
            { "#unban", new FunctionArgvInfo() { Count = 1 } },
            { "#help", new FunctionArgvInfo() { Count = 1 } },
            { "#stopBot", new FunctionArgvInfo() { Count = 0 } },
        };

        public Dictionary<string, FunctionArgvInfo> FunctionArgvInfos => FunctionArgvInfosDictionary;
    }
}
