using System;
using System.Threading.Tasks;
using Hdq.Routingslip.Core;
using Optional;

namespace RoutingSlipTests
{
    public class TestRouter : IRouter<TestCommand, TestMetadata, string>
    {
        public Tuple<ITransportCommand<TestCommand, TestMetadata, string>, string> Forwarded { get; private set; }
        
        public Task<bool> ForwardCommand(
            ITransportCommand<TestCommand, TestMetadata, string> transportCommand)
        {
            Option<string> nextRoute = transportCommand.Metadata.RoutingSlip.Next();
            foreach (var route in nextRoute)
            {
                Forwarded = Tuple.Create(transportCommand, route);
            }
            return Task.FromResult(true);
        }
    }
}