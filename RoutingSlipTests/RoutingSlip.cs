using System;
using System.Collections.Generic;
using System.Linq;
using Optional;

namespace RoutingSlipTests
{
    public class RoutingSlip<TRoute>
    {
        private readonly Stack<TRoute> _routes;

        public RoutingSlip(IEnumerable<TRoute> routes)
        {
            if (routes == null) throw new ArgumentNullException(nameof(routes));
            _routes = new Stack<TRoute>(routes.Reverse());
        }

        public Option<TRoute> Next()
        {
            return _routes.Any() ? 
                Option.Some(_routes.Pop()) 
                : Option.None<TRoute>();
        }

        public List<TRoute> RemainingRoutes()
        {
            return _routes.ToList();
        }
    }
}