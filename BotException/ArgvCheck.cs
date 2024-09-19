using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using QQBotForCSharp.BotException.Argv;

namespace QQBotForCSharp.BotException
{
    public class ArgvExceptions
    {
        public enum ExceptionType
        {
            TooManyArgvs,
            NeedMoreArgvs
        }

        public static string MakeQuate( ref string [ ] argv, int begin, int end = 0, int ptrPos = 0 )
        {
            var spaceCount = 0;
            for (int i = 0; i < begin; i++)
                spaceCount += argv[i].Length + 1;

            string spaceStr = new(' ', spaceCount);
            if (end == 0)
            {
                var sumLength = argv.Sum( s => s.Length + 1 );
                spaceStr += new string( '~', sumLength - spaceCount - 1 ).Insert( ptrPos, "^" );
            }
            else
            {
                var sumLength = 0;
                for (int i = begin; i < end; i++)
                    sumLength += argv[i].Length + 1;
                sumLength--;

                spaceStr += new string('~', sumLength).Insert(ptrPos, "^");
            }

            return spaceStr;
        }

        public static string MakeExceptionInfo( ref string [ ] argv, ExceptionType type, int begin, int end = 0, int ptrPos = 0 )
        {
            return MakeQuate( ref argv, begin, end, ptrPos );
        }
    }

    public class ArgvCheck
    {
        public static string CheckArgv( string [ ] args )
        {
            ArgvInfo.FunctionArgvInfosDictionary.TryGetValue( args [0], out var value );
            if (value.Count > args.Length - 1)
            {
                return ArgvExceptions.MakeExceptionInfo( ref args, ArgvExceptions.ExceptionType.TooManyArgvs, 1 );
            }
            else if (value.Count < args.Length - 1)
            {
                return ArgvExceptions.MakeExceptionInfo( ref args, ArgvExceptions.ExceptionType.NeedMoreArgvs, 1 );
            }
            else return string.Empty;
        }
    }
}
