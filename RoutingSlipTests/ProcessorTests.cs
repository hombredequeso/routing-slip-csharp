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
    public class ProcessorTests
    {
        [Fact]
        public async void End_to_end_processor_test()
        {
            List<TransportCommand<TestCommand, TestMetadata, string>> testTransportCommands = 
                TestFactory.GetTestCommands(10);
            TestDataSource commandSource = new TestDataSource(testTransportCommands);
            TestCommandHandler commandHandler = new TestCommandHandler();
            TestResultProcessor resultProcessor = new TestResultProcessor();
            TestRouter router = new TestRouter();
            TestCommandFactory commandFactory = new TestCommandFactory();
            
            Processor<TestCommand, TestOutCommand, TestMetadata, string, TestResult> processor =
                new Processor<TestCommand, TestOutCommand, TestMetadata, string, TestResult>(
                    commandSource,
                    commandHandler,
                    resultProcessor,
                    router,
                    "route1",
                    commandFactory);
                   
            // Act
            Option<Tuple<TransportCommand<TestCommand, TestMetadata, string>, TestResult>> result = 
                await processor.Run();
            
            // Assert
            var firstCorrelationId = testTransportCommands.First().Metadata.CorrelationId;
            result.ValueOrFailure().Item2.Should().BeEquivalentTo(
                new TestResult(firstCorrelationId));
            result.ValueOrFailure().Item1.Should().BeEquivalentTo(
                testTransportCommands.First());
            
            resultProcessor.TestResult.Should().BeEquivalentTo(new TestResult(firstCorrelationId));

            var forwarded = router.Forwarded;
            string forwardedroute = forwarded.Item2;
            forwardedroute.Should().BeEquivalentTo("route2");

            var forwardedMessage = forwarded.Item1;
            TestOutCommand expectedDomainCmd = new TestOutCommand(testTransportCommands.First().DomainCommand.Id);
            forwardedMessage.DomainCommand.Should().BeEquivalentTo(expectedDomainCmd);
            forwardedMessage.Should().BeOfType<TransportCommand<TestOutCommand, TestMetadata, string>>();
            // Need to test metadata here.
        }
    }
}