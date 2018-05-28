using System.Threading.Tasks;
using Optional;

namespace RoutingSlipTests
{
    public class Processor<TCmd, TMetadata, TRoute, TResult> where TMetadata : IMetadata<TRoute>
    {
        private readonly ICommandSource<TCmd, TMetadata, TRoute> _commandSource;
        private readonly ICommandHandler<TCmd, TMetadata, TRoute, TResult> _commandHandler;
        private readonly IResultProcessor<TResult> _resultProcessor;
        private readonly IRouter<TCmd, TMetadata, TRoute> _router;
        private readonly TRoute _thisRoute;

        public Processor(
            ICommandSource<TCmd, TMetadata, TRoute> commandSource, 
            ICommandHandler<TCmd, TMetadata, TRoute, TResult> commandHandler, 
            IResultProcessor<TResult> resultProcessor, 
            IRouter<TCmd, TMetadata, TRoute> router, 
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
            TResult result = await _commandHandler.Handle(cmd);
            bool processResult = await _resultProcessor.Process(result);
            bool forwardResult = await _router.ForwardCommand(cmd);
            bool ackResult = await _commandSource.AckTransportCommand(cmd);
            return ackResult;
        }
    }
}