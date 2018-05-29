using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace RoutingSlipTests
{
    public class TestMetataTests
    {
        [Fact]
        public void MetadataIsImmutable()
        {
            var metadata = CreateTestMetaData(Guid.NewGuid(), 10);
            var metadataCopy = CreateTestMetaData(metadata.CorrelationId, 10);

            var route = metadata.RoutingSlip;
            route.Clear();
            
            metadata.Should().BeEquivalentTo(
                metadataCopy, 
                "The clearing metadata.RoutingSlip has altered the metadata");
        }

        private static TestMetadata CreateTestMetaData(Guid id, int range)
        {
            var metadata = new TestMetadata(
                id,
                Enumerable.Range(0, range).Select(i => i.ToString()).ToList());
            return metadata;
        }
    }
}