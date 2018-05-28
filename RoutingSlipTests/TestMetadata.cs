using System;

namespace RoutingSlipTests
{
    public class TestMetadata : IMetadata<string>
    {

        public TestMetadata(Guid correlationId, RoutingSlip<string> routingSlip)
        {
            CorrelationId = correlationId;
            RoutingSlip = routingSlip;
        }

        public RoutingSlip<string> RoutingSlip { get; }
        public readonly Guid CorrelationId;
    }
}