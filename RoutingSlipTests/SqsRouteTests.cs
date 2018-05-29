using System.Text.RegularExpressions;
using FluentAssertions;
using Hdq.Routingslip.Core.Aws;
using Xunit;

namespace RoutingSlipTests
{
    public class SqsRouteTests
    {
        [Theory]
        [InlineData("abc", true)]
        [InlineData("abc-123", true)]
        [InlineData("aa bb", false)]
        public void OnlyLowercaseCharDigitsAndDashesAllowed(string s, bool result)
        {
            var regex = new Regex(SqsRoute.lowercaseDigitsAndDashRegex);
            regex.IsMatch(s).Should().Be(result);
        }
        
    }
}