using System.Threading.Tasks;

namespace Hdq.Routingslip.Core
{
    public interface IRouter<TCmd, TMetadata, TRoute> where TMetadata : IMetadata<TRoute>
    {
        Task<bool> ForwardCommand(ITransportCommand<TCmd, TMetadata, TRoute> transportCommand);
    }
}