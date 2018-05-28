using System.Threading.Tasks;

namespace RoutingSlipTests
{
    public interface IRouter<TCmd, TMetadata, TRoute> where TMetadata : IMetadata<TRoute>
    {
        Task<bool> ForwardCommand(ITransportCommand<TCmd, TMetadata, TRoute> transportCommand);
    }
}