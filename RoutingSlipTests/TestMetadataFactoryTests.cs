using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace RoutingSlipTests
{
    public class TestMetadataFactoryTests
    {
        [Fact]
        public void NextMetadata_RemovesRoutingSlipHead()
        {
            var id = Guid.NewGuid();
            var initialRoutingSlip = Enumerable.Range(0, 10).Select(x => x.ToString()).ToList();
            var expectedRoutingSlip = initialRoutingSlip.Skip(1).ToList();
            
            var initialMetadata = new TestMetadata(id, initialRoutingSlip);
            var expectedMetadata = new TestMetadata(id, expectedRoutingSlip);
            
            var result = TestMetadataFactory.NextMetadata(initialRoutingSlip.First(), initialMetadata);
            result.Should().BeEquivalentTo(expectedMetadata);
        }
    }
}