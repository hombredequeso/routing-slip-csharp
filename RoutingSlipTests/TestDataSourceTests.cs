using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Optional;
using Optional.Unsafe;
using Xunit;

namespace RoutingSlipTests
{
    public static class TestFactory
    {
        public static List<TestTransportCommand> GetTestCommands(int count)
        {
            var routingSlip = new RoutingSlip<string>(new []{"route1", "route2"});
            return Enumerable.Range(0, count)
                .Select(i => new TestTransportCommand(
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
            var testDataSource = new TestDataSource(new List<TestTransportCommand>());
            var nextMessage = await testDataSource.GetNextTransportCommand();

            nextMessage.Should().Be(Option.None<ITransportCommand<TestCommand, TestMetadata, string>>());
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