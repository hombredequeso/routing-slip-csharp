using System;
using System.Threading.Tasks;
using Hdq.Routingslip.Core;
using Optional;

namespace RoutingSlipTests
{
    public class TestRouter : IRouter<TestOutCommand, TestMetadata, string>
    {
        public Tuple<TransportCommand<TestOutCommand, TestMetadata, string>, string> Forwarded { get; private set; }
        
        public Task ForwardCommand(
            TransportCommand<TestOutCommand, TestMetadata, string> transportCommand)
        {
            Option<string> nextRoute = transportCommand.Metadata.RoutingSlip.Head();
            foreach (var route in nextRoute)
            {
                Forwarded = Tuple.Create(transportCommand, route);
            }
            return Task.CompletedTask;
        }
    }
}