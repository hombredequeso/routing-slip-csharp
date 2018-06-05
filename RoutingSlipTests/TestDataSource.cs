using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hdq.Routingslip.Core;
using Optional;
using Optional.Collections;

namespace RoutingSlipTests
{
    public class TestDataSource : ICommandSource<TestCommand, TestMetadata, string>
    {
        public readonly IDictionary<Guid, Tuple<TransportCommand<TestCommand, TestMetadata, string>, Boolean>> _commands;
        public TestDataSource(IEnumerable<TransportCommand<TestCommand, TestMetadata, string>> cmds)
        {
            _commands = cmds
                .Select(c => Tuple.Create(c, false))
                .ToDictionary(x => x.Item1.Metadata.CorrelationId);
        }


        public Task<Option<TransportCommand<TestCommand, TestMetadata, string>>> 
            GetNextTransportCommand()
        {
            Option<TransportCommand<TestCommand, TestMetadata, string>> nextCmd = 
                _commands
                    .Where(c => !c.Value.Item2)
                    .Select(c => c.Value.Item1)
                    .Cast<TransportCommand<TestCommand, TestMetadata, string>>()
                    .FirstOrNone();

            return Task.FromResult(nextCmd);
        }

        public Task<bool> AckTransportCommand(
            TransportCommand<TestCommand, TestMetadata, string> cmd)
        {
            if (!_commands.ContainsKey(cmd.Metadata.CorrelationId)) return Task.FromResult(false);
            var x = _commands[cmd.Metadata.CorrelationId];
            _commands[cmd.Metadata.CorrelationId] = Tuple.Create(x.Item1, true);
            return Task.FromResult(true);
        }

    }
}