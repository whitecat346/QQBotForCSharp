using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQBotForCSharp.Bot
{
    public class FunctionEnableInfo
    {
        public bool IsEnbale { get; set; }
    }

    public class GroupManagerData
    {
        public string GroupId { get; set; }

        public List<FunctionEnableInfo> Functions { get; set; }
    }
}
