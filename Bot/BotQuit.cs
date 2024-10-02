using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QQBotForCSharp.Bot;
using QQBotForCSharp.Functions;

namespace QQBotForCSharp;

public class BotQuit
{
    private struct GroupFuncInfo
    {
        public JObject GroupInfo { get; set; }
        public JObject FuncState { get; set; }
    }

    public static void SaveBotState()
    {
        if ( BotInit.GroupBotFunctions.Count == 0 ) return;

        JArray              groupManager = new();
        List<GroupFuncInfo> groupInfo    = new();

        // Now Problem:
        // file info will wron
        // have empty info

        foreach ( var groupBotFunction in BotInit.GroupBotFunctions )
        {
            JObject groupInfoJo = new();
            JObject funcStateJo = new();

            groupInfoJo.Add( "groupId", groupBotFunction.Key );
            foreach ( string funcName in BotFunctionsInfo.AllFunctionNameList )
            {
                funcStateJo.Add( funcName, groupBotFunction.Value.GetFuncState( funcName ) );
            }

            groupInfoJo.Add( "funcInfo", funcStateJo );

            groupInfo.Add( new GroupFuncInfo { GroupInfo = groupInfoJo, FuncState = funcStateJo } );
        }

        foreach ( GroupFuncInfo groupFuncInfo in groupInfo )
        {
            groupManager.Add( groupFuncInfo.GroupInfo );
        }

        File.WriteAllText( "groupManager.json", groupManager.ToString() + "\n" );

        Console.WriteLine( "Save Bot State" );
    }
}
