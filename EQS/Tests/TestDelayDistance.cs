using EQS.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EQS.Tests
{
    internal class TestDelayDistance : QueryTest
    {
        private List<Location> contextLocations;

        internal override void OnRunTest()
        {
            contextLocations = (this as IPrepareContext).PrepareContext_Location(ClampContext, Querier);
            LoopOverItems();
        }

        internal override void OnQuery()
        {
            foreach (var location in contextLocations)
            {
                var l = location.To;
                var a = CurrentIterator.Location;

                var x = a.X - l.X;
                var y = a.Y - l.Y;

                var w = (Single)Math.Sqrt(x * x + y * y);
                
                SetScoreSingle(w);
            }
        }
    }
}
