using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Hdq.Routingslip.Core.Aws
{
    
    public class TestSqsCommand
    {}
    
    public class TestSqsMetadata: IMetadata<string>
    {
        private readonly JObject _metadata;
        private readonly List<string> _routingSlip;

        public TestSqsMetadata(JObject metadata)
        {
            _routingSlip = ((JArray) (metadata["routingslip"]))
                .Select(c => (string)c)
                .ToList();
            _metadata = metadata;
        }

        public List<string> RoutingSlip => _routingSlip.ToList();
    }
    
    public class SqsTransportCommand
        : ITransportCommand<TestSqsCommand, TestSqsMetadata, string>
    {
        public TestSqsCommand DomainCommand { get; }
        public TestSqsMetadata Metadata { get; }
    }
}