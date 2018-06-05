using System;
using System.Threading.Tasks;

namespace Hdq.Routingslip.Core
{
    public interface ICommandHandler<TCmd, TNextCmd, TMetadata, TRoute, TResult> where TMetadata : IMetadata<TRoute>
    {
        Task<Tuple<TResult, TransportCommand<TNextCmd, TMetadata, TRoute>>> Handle(
            TransportCommand<TCmd, TMetadata, TRoute> cmd);
    }
}