using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQBotForCSharp.BotException
{
    public class ArgvExceptions
    {
        public enum ExceptionType
        {
            TooManyArgvs,
            NeedMoreArgvs
        }

        public static string MakeExceptionInfo( string [ ] argv, ExceptionType type, int begin, int end = 0, int ptrPos = 0 )
        {

        }
    }

    public class ArgvCheck
    {
        public static void CheckArgv( string [ ] args )
        {

        }
    }
}
