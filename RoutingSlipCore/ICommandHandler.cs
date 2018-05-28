using System.Threading.Tasks;

namespace Hdq.Routingslip.Core
{
    public interface ICommandHandler<TCmd, TMetadata, TRoute, TResult> where TMetadata : IMetadata<TRoute>
    {
        Task<TResult> Handle(ITransportCommand<TCmd, TMetadata, TRoute> cmd);
    }
}