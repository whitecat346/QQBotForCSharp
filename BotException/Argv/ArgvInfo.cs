using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQBotForCSharp.BotException.Argv
{
    public struct FunctionArgvInfo
    {
        public int Count { get; set; }
        public string[] Args { get; set; }
    }

    public class ArgvInfo
    {


        public Dictionary<string, FunctionArgvInfo> FunctionArgvInfos { get; set; }
    }
}
