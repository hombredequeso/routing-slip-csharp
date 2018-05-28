using System.Threading.Tasks;
using Hdq.Routingslip.Core;

namespace RoutingSlipTests
{
    public class TestResultProcessor : IResultProcessor<TestResult>
    {
        public TestResult TestResult { get; private set; }
        public Task<bool> Process(TestResult result)
        {
            TestResult = result;
            return Task.FromResult(true);
        }
    }
}