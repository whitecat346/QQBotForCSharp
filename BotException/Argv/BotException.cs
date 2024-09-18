namespace QQBotForCSharp.BotException.Argv
{
    public class FunctionArgyType
    {
        public static Dictionary<string, int> FunctionArgvTotal = new()
        {
            {"#randImg", 0},
            {"#netRandImg", 0},
            {"#earthQuake", 0},
            {"#ban", 3},
            {"#help", 1},
            {"#unban", 1}
        };
    }

    public class ArgvException
    {
        public static int CheckArgv( string [ ] argv, string functionType )
        {
            var functionArgvTotal = FunctionArgyType.FunctionArgvTotal [functionType];
            if (functionArgvTotal == argv.Length)
                return 0;
            else if (functionArgvTotal > argv.Length)
                return 1;
            else return -1;
        }

        public static string TooManyArgv( string [ ] argv, int begin, int end = 0, int ptrPos = 0)
        {


            return "none";
        }
    }
}
