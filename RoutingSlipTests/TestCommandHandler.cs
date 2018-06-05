using System;
using System.Threading.Tasks;
using Hdq.Routingslip.Core;

namespace RoutingSlipTests
{
    public class TestCommandHandler : ICommandHandler<TestCommand, TestCommand, TestMetadata, string, TestResult>
    {
        public Task<Tuple<TestResult, TransportCommand<TestCommand, TestMetadata, string>>> Handle(
            TransportCommand<TestCommand, TestMetadata, string> cmd)
        {
            var testResult = new TestResult(cmd.Metadata.CorrelationId);
            var nextCmd = cmd;
            return Task.FromResult(Tuple.Create(testResult, nextCmd));
        }
    }
    
    
    
    public class Test2CommandHandler : ICommandHandler<TestCommand, TestOutCommand, TestMetadata, string, TestResult>
    {
        public Task<Tuple<TestResult, TransportCommand<TestOutCommand, TestMetadata, string>>> Handle(
            TransportCommand<TestCommand, TestMetadata, string> cmd)
        {
            var testResult = new TestResult(cmd.Metadata.CorrelationId);
            TestOutCommand nextDomainCmd = new TestOutCommand(cmd.DomainCommand.Id);
            TransportCommand<TestOutCommand, TestMetadata, string> nextCmd = 
                new TransportCommand<TestOutCommand, TestMetadata, string>(nextDomainCmd, cmd.Metadata);
            return Task.FromResult(Tuple.Create(testResult, nextCmd));
        }
    }
}