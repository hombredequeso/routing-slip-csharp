using System;

namespace RoutingSlipTests
{
    public class TestOutCommand : IEquatable<TestOutCommand>
    {
        public bool Equals(TestOutCommand other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TestOutCommand) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public readonly Guid Id;


        public TestOutCommand(Guid id)
        {
            Id = id;
        }
    }
}