namespace RoutingSlipTests
{
    public interface IMetadata<TRoute>
    {
        RoutingSlip<TRoute> RoutingSlip { get; }
    }
}