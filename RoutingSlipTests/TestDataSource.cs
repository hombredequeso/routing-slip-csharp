using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Optional;
using Optional.Collections;

namespace RoutingSlipTests
{
    public class TestDataSource : ICommandSource<TestCommand, TestMetadata, string>
    {
        public readonly IDictionary<Guid, Tuple<TestTransportCommand, Boolean>> _commands;
        public TestDataSource(IEnumerable<TestTransportCommand> cmds)
        {
            _commands = cmds
                .Select(c => Tuple.Create<TestTransportCommand, bool>(c, false))
                .ToDictionary(x => x.Item1.Metadata.CorrelationId);
        }
        
        public Task<Option<ITransportCommand<TestCommand, TestMetadata, string>>> GetNextTransportCommand()
        {
            var nextCmd = 
                _commands
                    .Where(c => !c.Value.Item2)
                    .Select(c => c.Value.Item1)
                    .Cast<ITransportCommand<TestCommand, TestMetadata, string>>()
                    .FirstOrNone();
            return Task.FromResult(nextCmd);
        }

        public Task<bool> AckTransportCommand(
            ITransportCommand<TestCommand, TestMetadata, string> cmd)
        {
            if (!_commands.ContainsKey(cmd.Metadata.CorrelationId)) return Task.FromResult(false);
            var x = _commands[cmd.Metadata.CorrelationId];
            _commands[cmd.Metadata.CorrelationId] = Tuple.Create(x.Item1, true);
            return Task.FromResult(true);
        }
    }
}