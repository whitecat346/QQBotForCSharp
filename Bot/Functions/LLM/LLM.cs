using Makabaka.Models.EventArgs;
using Makabaka.Models.Messages;
using Ollama;

namespace QQBotForCSharp.Functions
{
    public partial class BotFunctions
    {
        private IList<long>? _context = null;

        public async void Llm( string [ ] msg, GroupMessageEventArgs eventArgs )
        {
            if ( msg.Length != 1 )
            {
                switch ( msg [1] )
                {
                    case "context" when msg.Length > 2 :
                        await eventArgs.ReplyAsync( [ new AtSegment( eventArgs.UserId ), new TextSegment( " 多余的参数!" ) ]
                                                  );
                        return;
                    case "context" when _context == null :
                        await eventArgs.ReplyAsync( [
                                                       new AtSegment( eventArgs.UserId ),
                                                       new TextSegment( " 没有任何上下文记录!" )
                                                   ]
                                                  );
                        return;
                    case "context" :
                        await eventArgs.ReplyAsync( [
                                                       new AtSegment( eventArgs.UserId ),
                                                       new TextSegment( " 当前上下文条数:" + _context.Count.ToString() )
                                                   ]
                                                  );
                        return;
                    case "clear" when eventArgs.UserId != 2710458198 :
                        await eventArgs.ReplyAsync( [
                                                       new AtSegment( eventArgs.UserId ),
                                                       new TextSegment( " 非所有者禁止使用！" )
                                                   ]
                                                  );
                        return;
                    case "clear" :
                        _context = null;
                        await eventArgs.ReplyAsync( [
                                                       new AtSegment( eventArgs.UserId ),
                                                       new TextSegment( " Context 已清空！" )
                                                   ]
                                                  );
                        return;
                }
            }

            var sendMessage = msg.Skip( 1 ).Aggregate( string.Empty, ( current, s ) => current + ( s + ' ' ) );
            var ollama      = new OllamaApiClient();

            var enumerable = await ollama.Completions.GenerateCompletionAsync(
                                                                              model: "llama3.1",
                                                                              prompt: sendMessage,
                                                                              context: _context
                                                                             );

            _context = enumerable.Context;

            await eventArgs.ReplyAsync( new TextSegment( enumerable.Response ) );
        }
    }
}
