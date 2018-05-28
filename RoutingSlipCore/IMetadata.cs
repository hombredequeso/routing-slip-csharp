namespace Hdq.Routingslip.Core
{
    public interface IMetadata<TRoute>
    {
        RoutingSlip<TRoute> RoutingSlip { get; }
    }
}