using System;
using System.Collections.Generic;
using Hdq.Routingslip.Core;

namespace RoutingSlipTests
{
    public class TestCommandFactory : ICommandFactory<TestCommand, TestMetadata, string>
    {
        public ITransportCommand<TestCommand, TestMetadata, string> CloneWithoutThisRoute(string route, ITransportCommand<TestCommand, TestMetadata, string> cmd)
        {
            return GetNext(route, cmd);
        }
        
        public static ITransportCommand<TestCommand, TestMetadata, string> GetNext(
            string route,
            ITransportCommand<TestCommand, TestMetadata, string> t)
        {
            return new TestTransportCommand(
                t.DomainCommand, 
                TestMetadataFactory.NextMetadata(route, t.Metadata));
        }
    }
    
    public class Test2CommandFactory : ICommandFactory<TestOutCommand, TestMetadata, string>
    {
        public ITransportCommand<TestOutCommand, TestMetadata, string> CloneWithoutThisRoute(
            string route, 
            ITransportCommand<TestOutCommand, TestMetadata, string> cmd)
        {
            return GetNext(route, cmd);
        }
        
        public static ITransportCommand<TestOutCommand, TestMetadata, string> GetNext(
            string route,
            ITransportCommand<TestOutCommand, TestMetadata, string> t)
        {
            return new TransportCommand<TestOutCommand, TestMetadata, string>(
                t.DomainCommand, 
                TestMetadataFactory.NextMetadata(route, t.Metadata));
        }

    }

    public static class TestMetadataFactory
    {
        public static TestMetadata NextMetadata(
            string route,
            TestMetadata t)
        {
            if (route == null) throw new ArgumentNullException(nameof(route));
            Tuple<string, List<string>> ht = t.RoutingSlip.HeadAndTail();
            if (ht.Item1 != route)
                throw new Exception("Attempt to forward command where the first route in the list is not the current route");

            return new TestMetadata(t.CorrelationId, ht.Item2);
        }
    }
}