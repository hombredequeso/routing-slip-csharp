using System;

namespace RoutingSlipTests
{
    public class TestCommand
    {
        public readonly Guid Id;

        public TestCommand(Guid id)
        {
            Id = id;
        }
    }
}