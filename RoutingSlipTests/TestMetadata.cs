using System;
using System.Collections.Generic;
using System.Linq;
using Hdq.Routingslip.Core;

namespace RoutingSlipTests
{
    public class TestMetadata : IMetadata<string>
    {
        public TestMetadata(Guid correlationId, List<string> routingSlip)
        {
            CorrelationId = correlationId;
            _routingSlip = routingSlip;
        }

        public List<string> RoutingSlip => _routingSlip.ToList();

        public readonly Guid CorrelationId;
        private readonly List<string> _routingSlip;
    }
}