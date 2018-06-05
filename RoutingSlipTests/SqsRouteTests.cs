using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using Hdq.Routingslip.Core.Aws;
using Xunit;

namespace RoutingSlipTests
{
    public class SqsRouteTests
    {
        
        [Theory]
        [InlineData("abccc", true, "at least 5 valid lowercase letters")]
        [InlineData("123-456", true, "at least 5 valid numbers and dash")]
        [InlineData("aa bb", false, "spaces not allowed")]
        [InlineData("abc-A", false, "Uppercase characters not allowed")]
        [InlineData("abc-*", false, "Only special characters allowed are dash")]
        [InlineData("1234", false, "must be at least 5 characters long")]
        [InlineData("12345", true, "5 characters is a valid length")]
        public void SqsRegexTests(string s, bool result, string becauseMessage)
        {
            new Regex(SqsRoute.lowercaseDigitsAndDashRegex).IsMatch(s)
                .Should().Be(result, becauseMessage);
        }

        [Fact]
        public void SqsRegexMaxLengthTest()
        {
            string tooLong = new String('a', 101);
            new Regex(SqsRoute.lowercaseDigitsAndDashRegex).IsMatch(tooLong)
                .Should().Be(false);
            
        }
    }
}