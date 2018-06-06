using System;
using System.Threading.Tasks;
using Optional;
using Optional.Linq;

namespace Hdq.Routingslip.Core
{
    public class Processor<TCmd, TNextCmd, TMetadata, TRoute, TResult> 
        where TMetadata : IMetadata<TRoute>
    {
        private readonly ICommandSource<TCmd, TMetadata, TRoute> _commandSource;
        private readonly ICommandHandler<TCmd, TNextCmd, TMetadata, TRoute, TResult> _commandHandler;
        private readonly IResultProcessor<TResult> _resultProcessor;
        private readonly IRouter<TNextCmd, TMetadata, TRoute> _router;
        private readonly TRoute _thisRoute;
        private readonly ICommandFactory<TNextCmd, TMetadata, TRoute> _commandFactory;

        public Processor(
            ICommandSource<TCmd, TMetadata, TRoute> commandSource, 
            ICommandHandler<TCmd, TNextCmd, TMetadata, TRoute, TResult> commandHandler, 
            IResultProcessor<TResult> resultProcessor, 
            IRouter<TNextCmd, TMetadata, TRoute> router, 
            TRoute thisRoute, 
            ICommandFactory<TNextCmd, TMetadata, TRoute> commandFactory)
        {
            _commandSource = commandSource;
            _commandHandler = commandHandler;
            _resultProcessor = resultProcessor;
            _router = router;
            _thisRoute = thisRoute;
            _commandFactory = commandFactory;
        }

        public async Task<Option<Tuple<TransportCommand<TCmd, TMetadata, TRoute>, TResult>>> Run()
        {
            var sqsCommand = await _commandSource.GetNextTransportCommand();
            var result = await sqsCommand.MapAsync(ProcessCommand);
            await sqsCommand.MapAsync(_commandSource.AckTransportCommand);

            return from c in sqsCommand
                from r in result
                select Tuple.Create(c, r);
        }

        public async Task<TResult> ProcessCommand(TransportCommand<TCmd, TMetadata, TRoute> cmd)
        {
            var handlerResult = await _commandHandler.Handle(cmd);
            TResult result = handlerResult.Item1;
            var nextCmd = handlerResult.Item2;
            await _resultProcessor.Process(result);
            var cloneWithoutThisRoute = _commandFactory.CloneWithoutThisRoute(_thisRoute, nextCmd);
            _router.ForwardCommand(cloneWithoutThisRoute);
            return result;
        }
    }
}