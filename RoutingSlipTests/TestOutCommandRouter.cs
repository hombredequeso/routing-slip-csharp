using System;
using System.Threading.Tasks;
using Hdq.Routingslip.Core;
using Optional;

namespace RoutingSlipTests
{
    
    public class TestOutCommandRouter : TestRouter<TestOutCommand>
    {
        public TestOutCommandRouter(string currentRoute) 
            : base(currentRoute, ToNext(currentRoute))
        {
        }
        
        private static 
            Func<TransportCommand<TestOutCommand, TestMetadata, string>, TransportCommand<TestOutCommand, TestMetadata, string>> 
                ToNext(
                    string currentRoute)
        {
            return transportCommandIn =>
                  new TransportCommand<TestOutCommand, TestMetadata, string>(
                    transportCommandIn.DomainCommand, 
                    TestMetadataFactory.NextMetadata(currentRoute, transportCommandIn.Metadata));
        }
    }
    
    
    public class TestCommandRouter : TestRouter<TestCommand>
    {
        public TestCommandRouter(string currentRoute) 
            : base(currentRoute, ToNext(currentRoute))
        {
        }
        
        private static 
            Func<TransportCommand<TestCommand, TestMetadata, string>, TransportCommand<TestCommand, TestMetadata, string>> 
                ToNext(
                    string currentRoute)
        {
            return transportCommandIn =>
                  new TransportCommand<TestCommand, TestMetadata, string>(
                    transportCommandIn.DomainCommand, 
                    TestMetadataFactory.NextMetadata(currentRoute, transportCommandIn.Metadata));
        }
    }
    
    
    public class TestRouter<TCmd> : IRouter<TCmd, TestMetadata, string>
    {
        protected readonly string _currentRoute;

        public TestRouter(
            string currentRoute,
            Func<TransportCommand<TCmd, TestMetadata, string>, TransportCommand<TCmd, TestMetadata, string>> next)
        {
            _currentRoute = currentRoute;
            GetNext = next;
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