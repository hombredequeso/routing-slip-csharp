namespace Hdq.Routingslip.Core
{
    public interface ICommandFactory<TCmd, TMetadata, TRoute> where TMetadata : IMetadata<TRoute>
    {
        ITransportCommand<TCmd, TMetadata, TRoute> CloneWithoutThisRoute(
            TRoute route,
            ITransportCommand<TCmd, TMetadata, TRoute> cmd);
        
    }
}