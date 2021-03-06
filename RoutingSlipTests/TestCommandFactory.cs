﻿using System;
using System.Collections.Generic;
using System.Linq;
using Hdq.Routingslip.Core;


namespace RoutingSlipTests
{
    public static class TestMetadataFactory
    {
        public static TestMetadata NextMetadata(
            string route,
            TestMetadata t)
        {
            return new TestMetadata(
                t.CorrelationId,
                NextRoute(route, t.RoutingSlip));
        }

        private static List<TRoute> NextRoute<TRoute>(
            TRoute currentRoute, 
            List<TRoute> route) 
            where TRoute: IComparable<TRoute>
        {
            if (currentRoute == null) 
                throw new ArgumentNullException(nameof(currentRoute));
            if (!route.Any()) 
                throw new ArgumentException(
                    "Routing slip does not have any routes. Needs at least one.", 
                    nameof(route));
            Tuple<TRoute, List<TRoute>> ht = route.HeadAndTail();
            if (!Equals(ht.Item1, currentRoute))
                throw new Exception("Attempt to forward command where the first route in the list is not the current route");
            return ht.Item2;

        }
    }
}