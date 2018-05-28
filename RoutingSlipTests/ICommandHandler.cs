using System.Threading.Tasks;

namespace RoutingSlipTests
{
    public interface ICommandHandler<TCmd, TMetadata, TRoute, TResult> where TMetadata : IMetadata<TRoute>
    {
        Task<TResult> Handle(ITransportCommand<TCmd, TMetadata, TRoute> cmd);
    }
}