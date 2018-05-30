using System;
using System.Threading.Tasks;
using Hdq.Routingslip.Core;

namespace RoutingSlipTests
{
    public class TestRouter : IRouter<TestCommand, TestMetadata, string>
    {
        public Tuple<ITransportCommand<TestCommand, TestMetadata, string>, string> Forwarded { get; private set; }
        
        public Task ForwardCommand(
            ITransportCommand<TestCommand, TestMetadata, string> transportCommand)
        {
            foreach (var route in transportCommand.Metadata.RoutingSlip.Head())
            {
                Forwarded = Tuple.Create(transportCommand, route);
            }
            return Task.CompletedTask;
        }

    }
}