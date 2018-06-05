using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Hdq.Routingslip.Core;
using Optional;
using Optional.Unsafe;
using Xunit;
using TestRoute = System.String;

namespace RoutingSlipTests
{
    public class ProcessorTests
    {
        [Fact]
        public async void End_to_end_processor_test_VaryingCommands()
        {
            var testTransportCommands = TestFactory.GetTestCommands(10);
            var commandSource = new TestDataSource(testTransportCommands);
            var commandHandler = new Test2CommandHandler();
            var resultProcessor = new TestResultProcessor();
            var router = new Test2Router();
            var commandFactory = new Test2CommandFactory();
            
            Processor<TestCommand, TestOutCommand, TestMetadata, TestRoute, TestResult> processor =
                new Processor<TestCommand, TestOutCommand, TestMetadata, TestRoute, TestResult>(
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
            var expectedMd = TestCommandFactory.GetNext("route1", testTransportCommands.First());
            forwardedMessage.Should().BeEquivalentTo(expectedMd);
            forwardedMessage.Should().BeOfType<TransportCommand<TestOutCommand, TestMetadata, string>>();
        }
        
        
        [Fact]
        public async void End_to_end_processor_test()
        {
            var testTransportCommands = TestFactory.GetTestCommands(10);
            ICommandSource<TestCommand, TestMetadata, string> commandSource = 
                new TestDataSource(testTransportCommands);
            ICommandHandler<TestCommand, TestCommand, TestMetadata, string, TestResult> commandHandler = 
                new TestCommandHandler();
            var resultProcessor
                = new TestResultProcessor();
            var router = new TestRouter();
            var commandFactory = new TestCommandFactory();
            
            Processor<TestCommand, TestCommand, TestMetadata, TestRoute, TestResult> processor =
                new Processor<TestCommand, TestCommand, TestMetadata, TestRoute, TestResult>(
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
            var expectedMd = TestCommandFactory.GetNext("route1", testTransportCommands.First());
            forwardedMessage.Should().BeEquivalentTo(expectedMd);
        }
    }
}