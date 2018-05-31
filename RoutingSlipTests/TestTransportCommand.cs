using System;
using Hdq.Routingslip.Core;

namespace RoutingSlipTests
{
    public class TestTransportCommand : 
        ITransportCommand<TestCommand, TestMetadata, string>
    {
        public TestTransportCommand(
            TestCommand domainCommand, 
            TestMetadata metadata)
        {
            DomainCommand = domainCommand;
            Metadata = metadata;
        }

        public TestCommand DomainCommand { get; }
        public TestMetadata Metadata { get; }
    }

    public class TransportCommand<TCmd, TMetadata, TRoute> 
        : ITransportCommand<TCmd, TMetadata, TRoute> where TMetadata : IMetadata<TRoute>
    {
        public TransportCommand(TCmd domainCommand, TMetadata metadata)
        {
            DomainCommand = domainCommand;
            Metadata = metadata;
        }

        public TCmd DomainCommand { get; }
        public TMetadata Metadata { get; }
    }
}