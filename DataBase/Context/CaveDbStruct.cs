using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace QQBotForCSharp.DataBase.Context
{
    public class CaveDbStruct
    {
        public long ID { get; set; }

        public string Sender { get; set; }

        public string Context { get; set; }
    }
}
