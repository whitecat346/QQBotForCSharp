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
            string totalString = string.Empty;

            // 计算实际的结束索引
            int actualEnd = (end != 0) ? end : argv.Length;
            var subString = string.Join( " ", argv.Skip( begin ).Take( actualEnd - begin ) );

            if (subString.Length > 20)
            {
                int outLength = subString.Length - 20;
                totalString = GetTruncatedString( subString, outLength );
            }
            else
            {
                totalString = subString;
            }

            string quateStr = new string( ' ', 4 ) + new string( '~', totalString.Length - 1 ).Insert( ptrPos, "^" );
            return totalString + "\n" + quateStr;
        }

        private static string GetTruncatedString( string input, int outLength )
        {
            int halfLength = (input.Length - outLength) / 2;
            if (outLength % 2 == 0)
            {
                return input.Substring( 0, halfLength ) + " ... " + input.Substring( input.Length - halfLength );
            }
            else
            {
                return input.Substring( 0, halfLength + 1 ) + " ... " + input.Substring( input.Length - halfLength );
            }
        }

        //public static string MakeQuate( ref string [ ] argv, int begin, int end = 0, int ptrPos = 0 )
        //{
        //    #region NotUsed

        //    //var spaceCount = 0;
        //    //for (int i = 0; i < begin; i++)
        //    //    spaceCount += argv[i].Length + 1;

        //    //string spaceStr = new(' ', spaceCount);
        //    //if (end == 0)
        //    //{
        //    //    var sumLength = argv.Sum( s => s.Length + 1 );
        //    //    spaceStr += new string( '~', sumLength - spaceCount - 1 ).Insert( ptrPos, "^" );
        //    //}
        //    //else
        //    //{
        //    //    var sumLength = 0;
        //    //    for (int i = begin; i < end; i++)
        //    //        sumLength += argv[i].Length + 1;
        //    //    sumLength--;

        //    //    spaceStr += new string('~', sumLength).Insert(ptrPos, "^");
        //    //}

        //    //return spaceStr;

        //    #endregion

        //    string? totalString = string.Empty;

        //    if (end != 0)
        //    {
        //        var subString = string.Join( " ", argv.Skip( begin ).Take( end - begin ) );
        //        if (subString.Length > 20)
        //        {
        //            var outLength = subString.Length - 20;
        //            if (outLength % 2 == 0)
        //            {
        //                totalString = new(
        //                    subString.Substring( 0, subString.Length / 2 ).Remove( outLength / 2 )
        //                    + " ... "
        //                    + subString.Substring( subString.Length / 2 ).Remove( 0, outLength / 2 )
        //                );
        //            }
        //            else
        //            {
        //                totalString = new(
        //                    subString.Substring( 0, subString.Length / 2 + 1 ).Remove( outLength / 2 )
        //                    + " ... "
        //                    + subString.Substring( subString.Length / 2 + 1 ) /*.Remove(0, outLength / 2 )*/
        //                );
        //            }
        //        }
        //    }
        //    else
        //    {
        //        var subString = string.Join( " ", argv.Skip( begin ).Take( argv.Length - begin ) );
        //        if (subString.Length > 20)
        //        {
        //            var outLength = subString.Length - 20;
        //            if (outLength % 2 == 0)
        //            {
        //                totalString = new(
        //                    subString.Substring( 0, subString.Length / 2 ).Remove( outLength / 2 )
        //                    + " ... "
        //                    + subString.Substring( subString.Length / 2 ).Remove( 0, outLength / 2 )
        //                );
        //            }
        //            else
        //            {
        //                totalString = new(
        //                    subString.Substring( 0, subString.Length / 2 + 1 ).Remove( outLength / 2 )
        //                    + " ... "
        //                    + subString.Substring( subString.Length / 2 + 1 ) /*.Remove(0, outLength / 2 )*/
        //                );
        //            }
        //        }
        //    }

        //    string quateStr = new(' ', 4);
        //    quateStr += new string( '~', totalString!.Length - 1 ).Insert(ptrPos, "^");

        //    return new string( totalString + "\n" + quateStr );
        //}

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
