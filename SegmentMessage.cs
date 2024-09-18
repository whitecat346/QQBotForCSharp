using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace QQBotForCSharp
{
    public class SegmentMessage
    {
        public static string GetJObject( string input )
        {
            input = input.Trim( '[', ']' );

            var parts = input.Split( ',' );
            JObject tempJObject = new JObject
            {
                ["type"] = parts [0].Trim().Split( ':' ) [1]
            };

            JObject dataJObject = new();
            foreach (string part in parts)
            {
                if (part.Contains("="))
                {
                    var keyValue = part.Split( '=' );
                    dataJObject [ keyValue [0].Trim() ] = keyValue [1].Trim();
                }
            }

            tempJObject ["data"] = dataJObject;
            return tempJObject.ToString();
        }

        public static bool IsCqCode( string input )
        {
            // [CQ:at,qq=2710458198]
            const string CqCodePattern = @"\[CQ:([a-zA-Z0-9_]+[^\s](,[a-zA-Z0-9_]+=[^\s]+)*)\]";

            return Regex.IsMatch(input, CqCodePattern);
        }

    }
}

namespace AtCode
{
    public class Data
    {
        public required string QQ { get; set; }
    }

    public class CodeInfo
    {
        public required string Type { get; set; }
        public required Data Data { get; set; }
    }

}
