using System;

namespace RoutingSlipTests
{
    public class TestResult
    {
        public TestResult(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}