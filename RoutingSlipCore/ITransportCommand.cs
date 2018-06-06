namespace Hdq.Routingslip.Core
{
    public class TransportCommand<TCmd, TMetadata, TRoute> where TMetadata: IMetadata<TRoute>
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