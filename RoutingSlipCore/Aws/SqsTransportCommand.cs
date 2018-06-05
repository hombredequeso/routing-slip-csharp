using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Hdq.Routingslip.Core.Aws
{
    
    public class TestSqsCommand
    {}
    
    public class TestSqsMetadata: IMetadata<string>
    {
        public readonly JObject Metadata;
        private readonly List<string> _routingSlip;

        public TestSqsMetadata(JObject metadata)
        {
            _routingSlip = ((JArray) (metadata["routingslip"]))
                .Select(c => (string)c)
                .ToList();
            Metadata = metadata;
        }

        public List<string> RoutingSlip => _routingSlip.ToList();
    }
    
    public static class SqsTransportCommandFactory
    {
        public static List<string> ToList(JArray ja)
        {
            return ToList(ja, x => (string)x);
        }

        public static List<T> ToList<T>(JArray ja, Func<JToken, T> toJToken)
        {
            return ja.Select(toJToken).ToList();
        }
        
        public static JArray ToJArray(List<string> l)
        {
            return new JArray(
                from s in l
                select new JValue(s));
        }
    }
}