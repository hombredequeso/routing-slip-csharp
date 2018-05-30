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
        public readonly IDictionary<Guid, Tuple<TestTransportCommand, Boolean>> _commands;
        public TestDataSource(IEnumerable<TestTransportCommand> cmds)
        {
            _commands = cmds
                .Select(c => Tuple.Create(c, false))
                .ToDictionary(x => x.Item1.Metadata.CorrelationId);
        }

        public static ITransportCommand<TestCommand, TestMetadata, string> GetNext(
            ITransportCommand<TestCommand, TestMetadata, string> t)
        {
            return new TestTransportCommand(t.DomainCommand, NextMetadata(t.Metadata));
        }

        public static TestMetadata NextMetadata(
            TestMetadata t)
        {
            var ht = t.RoutingSlip.HeadAndTail();
            var newMetadata = new TestMetadata(t.CorrelationId, ht.Item2);
            return newMetadata;
        }

        public Task<Option<ITransportCommand<TestCommand, TestMetadata, string>>> 
            GetNextTransportCommand()
        {
            Option<ITransportCommand<TestCommand, TestMetadata, string>> nextCmd = 
                _commands
                    .Where(c => !c.Value.Item2)
                    .Select(c => c.Value.Item1)
                    .Cast<ITransportCommand<TestCommand, TestMetadata, string>>()
                    .FirstOrNone();

//            Option<ITransportCommand<TestCommand, TestMetadata, string>> nextCmd2 = 
//                nextCmd.Map(GetNext);
            
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

        public ITransportCommand<TestCommand, TestMetadata, string> CloneWithoutThisRoute(
            string route, 
            ITransportCommand<TestCommand, TestMetadata, string> cmd)
        {
            return GetNext(cmd);
        }
    }
}