using System.Threading.Tasks;
using Hdq.Routingslip.Core;

namespace RoutingSlipTests
{
    public class TestCommandHandler : ICommandHandler<TestCommand, TestMetadata, string, TestResult>
    {
        public Task<TestResult> Handle(ITransportCommand<TestCommand, TestMetadata, string> cmd)
        {
            return Task.FromResult(
                new TestResult(cmd.Metadata.CorrelationId));
        }
    }
}