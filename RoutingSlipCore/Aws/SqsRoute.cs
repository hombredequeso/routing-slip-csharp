using System;
using System.Text.RegularExpressions;

namespace Hdq.Routingslip.Core.Aws
{
    public class SqsRoute
    {
        private readonly string _queueName;

        private const int minLength = 5;
        private const int maxLength = 100;
        
        public static readonly string lowercaseDigitsAndDashRegex = 
            // start-of-string [ lowercase digits - ] {allow between minLength and maxLength} end-of-string
            String.Format("^[a-z\\d-]{{{0},{1}}}$", minLength, maxLength);
        private static readonly string regexErrorMessage =
            "Invalid queueName. Only 5-100 lowercase, numeric, and - characters allowed";

        public SqsRoute(string queueName)
        {
            if (queueName == null) throw new ArgumentNullException(nameof(queueName));
            if (!new Regex(lowercaseDigitsAndDashRegex).IsMatch(queueName))
                throw new ArgumentException( regexErrorMessage, nameof(queueName));
            
            _queueName = queueName;
        }

        public static implicit operator string(SqsRoute r)
        {
            return r._queueName;
        }
    }
}