using System;
using System.Collections.Generic;
using FluentAssertions;
using Hdq.Routingslip.Core.Aws;
using Newtonsoft.Json.Linq;
using Xunit;

namespace RoutingSlipTests
{
    public static class MetadataFactory
    {
        public static JObject Create()
        {
            return new JObject
            {
                ["routingslip"] = new JArray("route1", "route2"),
                ["correlationId"] = Guid.NewGuid().ToString()
            };
        }
    }
    
    public class TestSqsMetadataTests
    {
        [Fact]
        public void RoutingSlipIsCorrect()
        {
            var metadata = new TestSqsMetadata(MetadataFactory.Create());
            List<string> routingSlip = metadata.RoutingSlip;
            var expectedRoutingSlip = new List<string>{"route1", "route2"};

            routingSlip.Should().Equal(expectedRoutingSlip);
        }
    }
}