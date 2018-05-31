namespace Hdq.Routingslip.Core
{
    public interface ITransportCommand<out TCmd, out TMetadata, TRoute> where TMetadata: IMetadata<TRoute>
    {
        TCmd DomainCommand { get; }
        TMetadata Metadata { get; }
    }
}