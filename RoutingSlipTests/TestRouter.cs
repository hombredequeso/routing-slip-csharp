using System;
using System.Threading.Tasks;
using Hdq.Routingslip.Core;
using Optional;
using Optional.Collections;

namespace RoutingSlipTests
{
    public class TestRouter : IRouter<TestCommand, TestMetadata, string>
    {
        public Tuple<ITransportCommand<TestCommand, TestMetadata, string>, string> Forwarded { get; private set; }
        
        public Task ForwardCommand(
            ITransportCommand<TestCommand, TestMetadata, string> transportCommand)
        {

            Option<string> nextRoute = transportCommand.Metadata.RoutingSlip.RemainingRoutes().FirstOrNone();
            foreach (var route in nextRoute)
            {
                Forwarded = Tuple.Create(transportCommand, route);
            }
            return Task.CompletedTask;
        }
    }
}