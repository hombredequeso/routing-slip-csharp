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
    
    
    public class Test2Router : IRouter<TestOutCommand, TestMetadata, string>
    {
        public Tuple<ITransportCommand<TestOutCommand, TestMetadata, string>, string> Forwarded { get; private set; }
        
        public Task ForwardCommand(
            ITransportCommand<TestOutCommand, TestMetadata, string> transportCommand)
        {
            foreach (var route in transportCommand.Metadata.RoutingSlip.Head())
            {
                Forwarded = Tuple.Create(transportCommand, route);
            }
            return Task.CompletedTask;
        }
    }
}