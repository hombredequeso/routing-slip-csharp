using System.Collections.Generic;
using FluentAssertions;
using Hdq.Routingslip.Core;
using Optional;
using Xunit;

namespace RoutingSlipTests
{
    public class RouterTests
    {
        [Fact]
        public void CanGetNextRoute()
        {
            var routingSlip = new RoutingSlip<string>(new []{"route1", "route2"});
            routingSlip.Next().Should().Be(Option.Some("route1"));
            routingSlip.Next().Should().Be(Option.Some("route2"));
            routingSlip.Next().Should().Be(Option.None<string>());
        }

        [Fact]
        public void NextRemovesRouteFromList()
        {
            var routingSlip = new RoutingSlip<string>(new []{"route1", "route2"});
            routingSlip.Next();

            routingSlip.RemainingRoutes().Should().Equal(new List<string>(){"route2"});
        }
    }
}