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
}