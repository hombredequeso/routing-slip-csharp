﻿using System;
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
        public static List<TestTransportCommand> GetTestCommands(int count)
        {
            var routingSlip = new List<String> {"route1", "route2"};
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

        [Fact]
        public void NextMetadata_RemovesRoutingSlipHead()
        {
            var id = Guid.NewGuid();
            var initialRoutingSlip = Enumerable.Range(0, 10).Select(x => x.ToString()).ToList();
            var expectedRoutingSlip = initialRoutingSlip.Skip(1).ToList();
            
            var initialMetadata = new TestMetadata(id, initialRoutingSlip);
            var expectedMetadata = new TestMetadata(id, expectedRoutingSlip);
            
            var result = TestDataSource.NextMetadata(initialMetadata);
            result.Should().BeEquivalentTo(expectedMetadata);
        }
    }
}