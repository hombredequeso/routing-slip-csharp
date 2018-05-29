using System;
using System.Threading.Tasks;
using Optional;

namespace Hdq.Routingslip.Core
{
    public class Processor<TCmd, TNextCmd, TMetadata, TRoute, TResult> where TMetadata : IMetadata<TRoute>
    {
        private readonly ICommandSource<TCmd, TMetadata, TRoute> _commandSource;
        private readonly ICommandHandler<TCmd, TNextCmd, TMetadata, TRoute, TResult> _commandHandler;
        private readonly IResultProcessor<TResult> _resultProcessor;
        private readonly IRouter<TNextCmd, TMetadata, TRoute> _router;
        private readonly TRoute _thisRoute;

        public Processor(
            ICommandSource<TCmd, TMetadata, TRoute> commandSource, 
            ICommandHandler<TCmd, TNextCmd, TMetadata, TRoute, TResult> commandHandler, 
            IResultProcessor<TResult> resultProcessor, 
            IRouter<TNextCmd, TMetadata, TRoute> router, 
            TRoute thisRoute)
        {
            _commandSource = commandSource;
            _commandHandler = commandHandler;
            _resultProcessor = resultProcessor;
            _router = router;
            _thisRoute = thisRoute;
        }

        public async Task<Option<bool>> Run()
        {
            Option<ITransportCommand<TCmd, TMetadata, TRoute>> cmd = 
                await _commandSource.GetNextTransportCommand();
            var result = await cmd.MapAsync(ProcessCommand);
            return result;
        }

        public async Task<bool> ProcessCommand(ITransportCommand<TCmd, TMetadata, TRoute> cmd)
        {
            Tuple<TResult, ITransportCommand<TNextCmd, TMetadata, TRoute>> handlerResult = 
                await _commandHandler.Handle(cmd);
            var result = handlerResult.Item1;
            ITransportCommand<TNextCmd, TMetadata, TRoute> nextCmd = handlerResult.Item2;
            await _resultProcessor.Process(result);
            await _router.ForwardCommand(nextCmd);
            bool ackResult = await _commandSource.AckTransportCommand(cmd);
            return ackResult;
        }
    }
}