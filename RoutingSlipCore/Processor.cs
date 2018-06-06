using System;
using System.Threading.Tasks;
using Optional;
using Optional.Linq;

namespace Hdq.Routingslip.Core
{
    public class Processor<TCmd, TNextCmd, TMetadata, TRoute, TResult> 
        where TMetadata : IMetadata<TRoute>
    {
        // A source of TransportCommands (consisting of TCmd and TMetadata, with a TRoute type)
        private readonly ICommandSource<TCmd, TMetadata, TRoute> _commandSource;
        // Handler for a TCmd. It produces a TNextCmd (which could be the same type as a TCmd)
        // to be passed on to the next route.
        // It also produces a TResult, which could be an event to be raised.
        private readonly ICommandHandler<TCmd, TNextCmd, TMetadata, TRoute, TResult> _commandHandler;
        // Does something with the TResult
        private readonly Option<IResultProcessor<TResult>> _resultProcessor;
        // Router takes a TNextCmd and routes it on to the next handler.
        // It is also responsible for popping the current route off the TMetadata.
        private readonly IRouter<TNextCmd, TMetadata, TRoute> _router;

        public Processor(
            ICommandSource<TCmd, TMetadata, TRoute> commandSource, 
            ICommandHandler<TCmd, TNextCmd, TMetadata, TRoute, TResult> commandHandler, 
            Option<IResultProcessor<TResult>> resultProcessor, 
            IRouter<TNextCmd, TMetadata, TRoute> router)
        {
            _commandSource = commandSource;
            _commandHandler = commandHandler;
            _resultProcessor = resultProcessor;
            _router = router;
        }

        // Get the next TransportCommand, process it, and pass it on the next route.
        // If a command was processed, it returns the TransportCommand that was processed, and its TResult
        public async Task<Option<Tuple<TransportCommand<TCmd, TMetadata, TRoute>, TResult>>> Run()
        {
            Option<TransportCommand<TCmd, TMetadata, TRoute>> sqsCommand = await _commandSource.GetNextTransportCommand();
            Option<TResult> result = await sqsCommand.MapAsync(ProcessCommand);
            await sqsCommand.MapAsync(_commandSource.AckTransportCommand);

            return from c in sqsCommand
                from r in result
                select Tuple.Create(c, r);
        }

        private async Task<TResult> ProcessCommand(TransportCommand<TCmd, TMetadata, TRoute> cmd)
        {
            var handlerResult = await _commandHandler.Handle(cmd);
            var result = handlerResult.Item1;
            var nextTransportCommand = handlerResult.Item2;
            foreach (var processor in _resultProcessor)
                await processor.Process(result);
            await _router.ForwardCommand(nextTransportCommand);
            return result;
        }
    }
}