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
            string currentRoute = "route1";
            List<TransportCommand<TestCommand, TestMetadata, string>> testTransportCommands = 
                TestFactory.GetTestCommands(10);
            TestDataSource commandSource = new TestDataSource(testTransportCommands);
            TestCommandHandler commandHandler = new TestCommandHandler();
            TestResultProcessor resultProcessor = new TestResultProcessor();
            TestOutCommandRouter router = new TestOutCommandRouter(currentRoute);

            Processor<TestCommand, TestOutCommand, TestMetadata, string, TestResult> processor =
                new Processor<TestCommand, TestOutCommand, TestMetadata, string, TestResult>(
                    commandSource,
                    commandHandler,
                    Option.Some<IResultProcessor<TestResult>>(resultProcessor),
                    router);
                   
            // Act
            Option<Tuple<TransportCommand<TestCommand, TestMetadata, string>, TestResult>> result = 
                await processor.Run();
            
            // Assert
            var processedTransportCommand = testTransportCommands.First();
            
            // Check processor.Run has returned the expected results.
            var expectedResult = new TestResult(processedTransportCommand.Metadata.CorrelationId);
            result.ValueOrFailure().Item2.Should().BeEquivalentTo(expectedResult);
            result.ValueOrFailure().Item1.Should().BeEquivalentTo(processedTransportCommand);
            
            // Check the resultProcessor processed the expected result
            resultProcessor.TestResult.Should().BeEquivalentTo(expectedResult);

            // Check the router 
            var forwarded = router.Forwarded;
            // - forwarded to the expected route.
            string forwardedroute = forwarded.Item2;
            forwardedroute.Should().BeEquivalentTo("route2");

            // - Forwarded the expected command.
            var forwardedMessage = forwarded.Item1;
            TestOutCommand expectedDomainCmd = new TestOutCommand(processedTransportCommand.DomainCommand.Id);
            forwardedMessage.DomainCommand.Should().BeEquivalentTo(expectedDomainCmd);
            forwardedMessage.Should().BeOfType<TransportCommand<TestOutCommand, TestMetadata, string>>();
            // Need to test metadata here.
        }
        
        
        [Fact]
        public async void WithoutResult()
        {
            string currentRoute = "route1";
            List<TransportCommand<TestCommand, TestMetadata, string>> testTransportCommands = 
                TestFactory.GetTestCommands(10);
            TestDataSource commandSource = new TestDataSource(testTransportCommands);
            TestCommandHandler commandHandler = new TestCommandHandler();
            TestOutCommandRouter router = new TestOutCommandRouter(currentRoute);

            Processor<TestCommand, TestOutCommand, TestMetadata, string, TestResult> processor =
                new Processor<TestCommand, TestOutCommand, TestMetadata, string, TestResult>(
                    commandSource,
                    commandHandler,
                    Option.None<IResultProcessor<TestResult>>(),
                    router);
                   
            // Act
            Option<Tuple<TransportCommand<TestCommand, TestMetadata, string>, TestResult>> result = 
                await processor.Run();
            
            // Assert
            var firstCorrelationId = testTransportCommands.First().Metadata.CorrelationId;
            result.ValueOrFailure().Item2.Should().BeEquivalentTo(
                new TestResult(firstCorrelationId));
            result.ValueOrFailure().Item1.Should().BeEquivalentTo(
                testTransportCommands.First());

            var forwarded = router.Forwarded;
            string forwardedroute = forwarded.Item2;
            forwardedroute.Should().BeEquivalentTo("route2");

            var forwardedMessage = forwarded.Item1;
            TestOutCommand expectedDomainCmd = new TestOutCommand(testTransportCommands.First().DomainCommand.Id);
            forwardedMessage.DomainCommand.Should().BeEquivalentTo(expectedDomainCmd);
            forwardedMessage.Should().BeOfType<TransportCommand<TestOutCommand, TestMetadata, string>>();
            // Need to test metadata here.
        }
        
        
        [Fact]
        public async void SameCommandInAndOut()
        {
            string currentRoute = "route1";
            List<TransportCommand<TestCommand, TestMetadata, string>> testTransportCommands = 
                TestFactory.GetTestCommands(10);
            TestDataSource commandSource = new TestDataSource(testTransportCommands);
            TestSameCommandInAndOurHandler commandHandler = new TestSameCommandInAndOurHandler();
            TestCommandRouter router = new TestCommandRouter(currentRoute);

            Processor<TestCommand, TestCommand, TestMetadata, string, TestResult> processor =
                new Processor<TestCommand, TestCommand, TestMetadata, string, TestResult>(
                    commandSource,
                    commandHandler,
                    Option.None<IResultProcessor<TestResult>>(),
                    router);
                   
            // Act
            Option<Tuple<TransportCommand<TestCommand, TestMetadata, string>, TestResult>> result = 
                await processor.Run();
            
            // Assert
            var firstCorrelationId = testTransportCommands.First().Metadata.CorrelationId;
            result.ValueOrFailure().Item2.Should().BeEquivalentTo(
                new TestResult(firstCorrelationId));
            result.ValueOrFailure().Item1.Should().BeEquivalentTo(
                testTransportCommands.First());

            var forwarded = router.Forwarded;
            string forwardedroute = forwarded.Item2;
            forwardedroute.Should().BeEquivalentTo("route2");

            var forwardedMessage = forwarded.Item1;
            TestCommand expectedDomainCmd = new TestCommand(testTransportCommands.First().DomainCommand.Id);
            forwardedMessage.DomainCommand.Should().BeEquivalentTo(expectedDomainCmd);
            forwardedMessage.Should().BeOfType<TransportCommand<TestCommand, TestMetadata, string>>();
            // Need to test metadata here.
        }
    }
}