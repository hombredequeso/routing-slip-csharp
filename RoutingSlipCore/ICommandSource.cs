using System.Threading.Tasks;
using Optional;

namespace Hdq.Routingslip.Core
{
    public interface ICommandSource<TCmd, TMetadata, TRoute> where TMetadata : IMetadata<TRoute>
    {
        Task<Option<TransportCommand<TCmd, TMetadata, TRoute>>> GetNextTransportCommand();
        Task<bool> AckTransportCommand(TransportCommand<TCmd, TMetadata, TRoute> cmd);
        
    }
}