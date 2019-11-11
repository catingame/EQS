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

        internal override void DoItemGeneration()
        {
            var contextLocations = (this as IPrepareContext).PrepareContext_Location(Context, querier);

            Single i_p = 1 / PointsPerRing;

            foreach (var location in contextLocations)
            {
                for (var i = 0; i < PointsPerRing; ++i)
                {
                    var dx = (i_p * i) * 2 - 1;
                    var rad = (Math.Exp(dx) / Math.E) * GridSize;

                    var dy = (i + 1) / PointsPerRing;
                    var length = NumberOfRings * dy;
                    for (var j = 0; j < length; ++j)
                    {
                        var rot = 360 * (j / length);
                        var vector = new Vector3(0, 0, rot);

                        // TODO:
                        //const rot = 360 * (index / index_length)
                        //    $R.Yaw = rot;
                        //const dest_vector = ($V_unit_X.RotateVector($R, $V)).Multiply_VectorFloat(rad, $V)
                        //const dest_location = l.Add_VectorVector(dest_vector, $V)

                        //this.AddGeneratedVector(dest_location)
                    }
                }
            }
        }
    }
}
