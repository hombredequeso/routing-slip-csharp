namespace Hdq.Routingslip.Core
{
    public interface ITransportCommand<TCmd, TMetadata, TRoute> where TMetadata: IMetadata<TRoute>
    {
        TCmd DomainCommand { get; }
        TMetadata Metadata { get; }
    }
}