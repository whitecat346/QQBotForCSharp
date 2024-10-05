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
            var sendMessage = msg.Skip( 1 ).Aggregate( string.Empty, ( current, s ) => current + ( s + ' ' ) );
            var ollama      = new OllamaApiClient();

            var enumerable = await ollama.Completions.GenerateCompletionAsync(
                                                                              model: "llama3.1",
                                                                              prompt: sendMessage,
                                                                              context: _context
                                                                             );

            _context = enumerable.Context;

            if ( _context is { Count: > 50 } )
            {
                _context.RemoveAt( 0 );
            }

            await eventArgs.ReplyAsync( new TextSegment( enumerable.Response ) );
        }
    }
}
