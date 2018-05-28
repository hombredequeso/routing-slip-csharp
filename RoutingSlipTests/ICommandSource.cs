using System.Threading.Tasks;
using Optional;

namespace RoutingSlipTests
{
    public interface ICommandSource<TCmd, TMetadata, TRoute> where TMetadata : IMetadata<TRoute>
    {
        Task<Option<ITransportCommand<TCmd, TMetadata, TRoute>>> GetNextTransportCommand();
        Task<bool> AckTransportCommand(ITransportCommand<TCmd, TMetadata, TRoute> cmd);
    }
}