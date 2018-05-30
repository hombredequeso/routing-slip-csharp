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
            return new TestTransportCommand(t.DomainCommand, NextMetadata(route, t.Metadata));
        }

        public static TestMetadata NextMetadata(
            string route,
            TestMetadata t)
        {
            Tuple<string, List<string>> ht = t.RoutingSlip.HeadAndTail();
            if (ht.Item1 != route)
                throw new Exception("Attempt to forward command where the first route in the list is not the current route");

            return new TestMetadata(t.CorrelationId, ht.Item2);
        }
    }
}