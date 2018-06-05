using System;
using System.Threading.Tasks;
using Hdq.Routingslip.Core;

namespace RoutingSlipTests
{
    public class TestRouter : IRouter<TestCommand, TestMetadata, string>
    {
        public Tuple<TransportCommand<TestCommand, TestMetadata, string>, string> Forwarded { get; private set; }
        
        public Task ForwardCommand(
            TransportCommand<TestCommand, TestMetadata, string> transportCommand)
        {
            foreach (var route in transportCommand.Metadata.RoutingSlip.Head())
            {
                Forwarded = Tuple.Create(transportCommand, route);
            }
            return Task.CompletedTask;
        }
    }
    
    
    public class Test2Router : IRouter<TestOutCommand, TestMetadata, string>
    {
        public Tuple<TransportCommand<TestOutCommand, TestMetadata, string>, string> Forwarded { get; private set; }
        
        public Task ForwardCommand(
            TransportCommand<TestOutCommand, TestMetadata, string> transportCommand)
        {
            foreach (var route in transportCommand.Metadata.RoutingSlip.Head())
            {
                Forwarded = Tuple.Create(transportCommand, route);
            }
            return Task.CompletedTask;
        }
    }
}