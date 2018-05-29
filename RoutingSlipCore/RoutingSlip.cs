using System;
using System.Collections.Generic;
using System.Linq;
using Optional;

namespace Hdq.Routingslip.Core
{
    public static class ListExtensions
    {
        public static Tuple<T, List<T>> HeadAndTail<T>(this List<T> l)
        {
            return Tuple.Create(l.First(), l.Skip(1).ToList());
        }

        public static Option<Tuple<T, List<T>>> HeadAndTailOption<T>(this List<T> l)
        {
            return l.Any() ? 
                Option.Some(HeadAndTail(l)) 
                : Option.None<Tuple<T, List<T>>>();
        }

        public static Option<T> Head<T>(this List<T> l)
        {
            return l.Any()
                ? Option.Some(l.First())
                : Option.None<T>();
        }
    }
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