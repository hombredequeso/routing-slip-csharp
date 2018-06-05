using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Hdq.Routingslip.Core;
using Optional;
using Optional.Unsafe;
using Xunit;

namespace RoutingSlipTests
{
    public static class TestFactory
    {
        public static List<TransportCommand<TestCommand, TestMetadata, string>> GetTestCommands(int count)
        {
            var routingSlip = new List<String> {"route1", "route2"};
            return Enumerable.Range(0, count)
                .Select(i => new TransportCommand<TestCommand, TestMetadata, string>(
                    new TestCommand(Guid.NewGuid()),
                    new TestMetadata(Guid.NewGuid(), routingSlip)))
                .ToList();
        }
        
    }
    public class TestDataSourceTests
    {
        [Fact]
        public async void WhenNoMessagesAvailable_GetNextMessage_ReturnsNothing()
        {
            var testDataSource = new TestDataSource(new List<TransportCommand<TestCommand, TestMetadata, string>>());
            var nextMessage = await testDataSource.GetNextTransportCommand();

            nextMessage.Should().Be(Option.None<TransportCommand<TestCommand, TestMetadata, string>>());
        }

        
        [Fact]
        public async void WhenMessagesAvailable_GetNextMessage_ReturnsNextMessage()
        {
            var cmds = TestFactory.GetTestCommands(2);
            var testDataSource = new TestDataSource(cmds);

            var firstCommand = await testDataSource.GetNextTransportCommand();
            var messageId = firstCommand.ValueOrFailure().DomainCommand.Id;
            var expectedId = cmds.First().DomainCommand.Id;
            messageId.Should().Be(expectedId);
        }

    }
}