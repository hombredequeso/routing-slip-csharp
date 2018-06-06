using System;
using System.Threading.Tasks;
using Hdq.Routingslip.Core;
using Optional;

namespace RoutingSlipTests
{
    public class TestRouter : IRouter<TestOutCommand, TestMetadata, string>
    {
        private readonly string _currentRoute;

        public TestRouter(string currentRoute)
        {
            _currentRoute = currentRoute;
        }
        public Tuple<TransportCommand<TestOutCommand, TestMetadata, string>, string> Forwarded { get; private set; }
        
        public Task ForwardCommand(
            TransportCommand<TestOutCommand, TestMetadata, string> transportCommand)
        {
            var nextCmd = new TransportCommand<TestOutCommand, TestMetadata, string>(
                transportCommand.DomainCommand, 
                TestMetadataFactory.NextMetadata(_currentRoute, transportCommand.Metadata));
            Option<string> nextRoute = nextCmd.Metadata.RoutingSlip.Head();
            foreach (var route in nextRoute)
            {
                Forwarded = Tuple.Create(transportCommand, route);
            }
            return Task.CompletedTask;
        }
    }
    
    public class TestRouter2 : IRouter<TestCommand, TestMetadata, string>
    {
        private readonly string _currentRoute;

        public TestRouter2(string currentRoute)
        {
            _currentRoute = currentRoute;
        }
        public Tuple<TransportCommand<TestCommand, TestMetadata, string>, string> Forwarded { get; private set; }
        
        public Task ForwardCommand(
            TransportCommand<TestCommand, TestMetadata, string> transportCommand)
        {
            var nextCmd = new TransportCommand<TestCommand, TestMetadata, string>(
                transportCommand.DomainCommand, 
                TestMetadataFactory.NextMetadata(_currentRoute, transportCommand.Metadata));
            Option<string> nextRoute = nextCmd.Metadata.RoutingSlip.Head();
            foreach (var route in nextRoute)
            {
                Forwarded = Tuple.Create(transportCommand, route);
            }
            return Task.CompletedTask;
        }
    }
    
    
    public class TestRouterG<TCmd> : IRouter<TCmd, TestMetadata, string>
    {
        private readonly string _currentRoute;

        public TestRouterG(string currentRoute)
        {
            _currentRoute = currentRoute;
        }
        
        public Tuple<TransportCommand<TCmd, TestMetadata, string>, string> Forwarded 
            { get; private set; }

        private readonly Func<TransportCommand<TCmd, TestMetadata, string>, TransportCommand<TCmd, TestMetadata, string>>
            GetNext;
        
        public Task ForwardCommand(
            TransportCommand<TCmd, TestMetadata, string> transportCommand)
        {
            TransportCommand<TCmd, TestMetadata, string> nextCmd = GetNext(transportCommand);
            Option<string> nextRoute = nextCmd.Metadata.RoutingSlip.Head();
            foreach (var route in nextRoute)
            {
                Forwarded = Tuple.Create(transportCommand, route);
            }
            return Task.CompletedTask;
        }
    }
}