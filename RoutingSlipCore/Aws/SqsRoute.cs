using System;

namespace Hdq.Routingslip.Core.Aws
{
    public class SqsRoute
    {
        private readonly string _queueName;

        private readonly int minLength = 5;
        private readonly int maxLength = 300;
        public static readonly string lowercaseDigitsAndDashRegex = "^[a-z\\d-]+$";

        public SqsRoute(string queueName)
        {
            if (queueName == null) throw new ArgumentNullException(nameof(queueName));
            if (queueName.Length < minLength || queueName.Length > maxLength)
                throw new ArgumentException("Invalid length", nameof(queueName));
            // check for valid characters.
            
            _queueName = queueName;
        }

        public static implicit operator string(SqsRoute r)
        {
            return r._queueName;
        }
    }
}