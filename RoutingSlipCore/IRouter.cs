using System.Threading.Tasks;

namespace Hdq.Routingslip.Core
{
    public interface IRouter<in TCmd, in TMetadata, TRoute> where TMetadata : IMetadata<TRoute>
    {
        Task ForwardCommand(ITransportCommand<TCmd, TMetadata, TRoute> transportCommand);
    }
}