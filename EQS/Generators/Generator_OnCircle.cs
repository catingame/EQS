using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using EQS.Classes;

namespace EQS.Generators
{
    public class Generator_OnCircle : QueryGenerator
    {
        public Single GridSize = 600;
        public Int32 NumberOfRings = 30;
        public Int32 PointsPerRing = 5;

        public Generator_OnCircle(IQueryContext context) : base(context)
        {
        }

        internal override List<QueryItem> DoItemGeneration()
        {
            var contextLocations = (this as IPrepareContext).PrepareContext_Location(Context, Querier);

            var ceta = 1 / PointsPerRing;

            var items = new List<QueryItem>();
            foreach (var location in contextLocations)
            {
                for (var i = 0; i < PointsPerRing; ++i)
                {
                    var dx = (ceta * i) * 2 - 1;
                    var rad = (Single)(Math.Exp(dx) / Math.E) * GridSize;

                    var dy = (i + 1) / PointsPerRing;
                    var length = NumberOfRings * dy;
                    for (var j = 0; j < length; ++j)
                    {
                        Single rot = 360 * (j / length);
                        var v = new Vector3()
                        {
                            X = location.X + rad * (Single)Math.Cos(rot),
                            Y = location.Y + rad * (Single)Math.Sin(rot),
                            Z = location.Z
                        };

                        items.Add(new QueryItem(v, v.GetType()));
                    }
                }
            }
            return items;
        }
    }
}
