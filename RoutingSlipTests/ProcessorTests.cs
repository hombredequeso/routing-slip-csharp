using System;
using System.Linq;
using FluentAssertions;
using Hdq.Routingslip.Core;
using Optional;
using Xunit;
using TestRoute = System.String;

namespace RoutingSlipTests
{
    public class ProcessorTests
    {
        [Fact]
        public async void End_to_end_processor_test()
        {
            var testTransportCommands = TestFactory.GetTestCommands(10);
            ICommandSource<TestCommand, TestMetadata, string> commandSource = 
                new TestDataSource(testTransportCommands);
            ICommandHandler<TestCommand, TestMetadata, string, TestResult> commandHandler = 
                new TestCommandHandler();
            var resultProcessor
                = new TestResultProcessor();
            var router = new TestRouter();
            
            Processor<TestCommand, TestMetadata, TestRoute, TestResult> processor =
                new Processor<TestCommand, TestMetadata, TestRoute, TestResult>(
                    commandSource,
                    commandHandler,
                    resultProcessor,
                    router,
                    "route1");
                   
            // Act
            Option<bool> result = await processor.Run();
            
            // Assert
            result.Should().Be(Option.Some(true));
            var firstCorrelationId = testTransportCommands.First().Metadata.CorrelationId;
            resultProcessor.TestResult.Should().BeEquivalentTo(new TestResult(firstCorrelationId));

            var forwarded = router.Forwarded;
            string forwardedroute = forwarded.Item2;
            forwardedroute.Should().BeEquivalentTo("route2");

            var forwardedMessage = forwarded.Item1;
            var expectedMessage = testTransportCommands.First();
            forwardedMessage.Should().BeEquivalentTo(expectedMessage);
        }
    }
}