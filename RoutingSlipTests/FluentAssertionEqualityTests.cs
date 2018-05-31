using System;
using FluentAssertions;
using Xunit;

namespace RoutingSlipTests
{
    public class Widget
    {
        public Widget(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
    
    
    public class FluentAssertionEqualityTests
    {
        [Fact]
        public void EqualityTesting()
        {
            var widget1 = new Widget(Guid.NewGuid());
            var widget1b = new Widget(widget1.Id);
            var widget2 = new Widget(Guid.NewGuid());

            widget1.Should().BeEquivalentTo(widget1b);
            widget1.Should().Be(widget1);
            // widget1.Should().Be(widget1b); fails
            widget1.Should().Equals(widget1b);
            widget1.Should().Equals(widget2);
        }
        
    }
}