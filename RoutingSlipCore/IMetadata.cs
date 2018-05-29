using System.Collections.Generic;

namespace Hdq.Routingslip.Core
{
    public interface IMetadata<TRoute>
    {
        List<TRoute> RoutingSlip { get; }
    }
}