namespace Hdq.Routingslip.Core
{
    public interface ICommandFactory<TCmd, TMetadata, TRoute> where TMetadata : IMetadata<TRoute>
    {
        TransportCommand<TCmd, TMetadata, TRoute> CloneWithoutThisRoute(
            TRoute route,
            TransportCommand<TCmd, TMetadata, TRoute> cmd);
        
    }
}