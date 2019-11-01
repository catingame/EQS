using EQS.Classes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EQS.Tests
{
    public class TestDelayDistance : QueryTest
    {
        private readonly IQueryContext _distanceTo;

        private List<Vector3> contextLocations;

        public TestDelayDistance(IQueryContext DistanceTo)
        {
            _distanceTo = DistanceTo;
        }

        internal override void OnRunTest()
        {
            contextLocations = (this as IPrepareContext).PrepareContext_Location(_distanceTo, Querier);
            LoopOverItems();
        }

        internal override void OnQuery()
        {
            foreach (var location in contextLocations)
            {
                var l = location;
                var a = CurrentIterator.Location;

                var x = a.X - l.X;
                var y = a.Y - l.Y;

                var w = (Single)Math.Sqrt(x * x + y * y);
                
                SetScoreSingle(w);
            }
        }
    }
}
