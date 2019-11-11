using EQS.Classes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EQS.Generators
{
    public class Generator_Spiral : QueryGenerator
    {
        public Int32 MinR = 0;
        public Int32 MaxR = 600;
        public Int32 Distance = 10;

        public Generator_Spiral(IQueryContext context) : base(context)
        {
        }

        internal override void DoItemGeneration()
        {
            var contextLocations = (this as IPrepareContext).PrepareContext_Location(Context, Querier);

            var phi = (1 + Math.Sqrt(5)) / 2;
            var da = 2 * Math.PI * (phi - 1) / phi;
            var n = (Int32)((this.MaxR - this.MinR) / this.Distance);
            var a = 0.0;
            var r = MinR;

            foreach (var contextLocation in contextLocations)
            {
                var l = contextLocation;
                for (var i = 0; i < n; ++i)
                {
                    var v = new Vector3()
                        { 
                            X = l.X + r * (Single)Math.Cos(a),
                            Y = l.Y + r * (Single)Math.Sin(a),
                            Z = l.Z
                        };

                    a = Rot((Single)(a + da), (Single)(2 * Math.PI));
                    r += Distance;

                    AddGeneratedItem(new QueryItem(v, v.GetType()));
                }
            }

            static Single Rot(Single a, Single b) => a - ((Single)Math.Floor(a / b) * b);
        }
    }
}
