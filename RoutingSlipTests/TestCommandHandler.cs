using System;
using System.Threading.Tasks;
using Hdq.Routingslip.Core;

namespace RoutingSlipTests
{
    public class TestCommandHandler : ICommandHandler<TestCommand, TestCommand, TestMetadata, string, TestResult>
    {
        public Task<Tuple<TestResult, ITransportCommand<TestCommand, TestMetadata, string>>> Handle(
            ITransportCommand<TestCommand, TestMetadata, string> cmd)
        {
            var testResult = new TestResult(cmd.Metadata.CorrelationId);
            var nextCmd = cmd;
            return Task.FromResult(Tuple.Create(testResult, nextCmd));
        }
    }
}