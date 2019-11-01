using EQS.Classes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EQS.Generators
{
    public class GeneratorSpiral : QueryGenerator
    {
        private Int32 MinR = 0;
        private Int32 MaxR = 600;
        private Int32 Distance = 10;

        public GeneratorSpiral(IQueryContext context) : base(context)
        {
        }

        internal override void DoItemGeneration()
        {
            var rawDatas = (this as IPrepareContext).PrepareContext_Location(Context, querier);

            var phi = (1 + Math.Sqrt(5)) / 2;
            var da = 2 * Math.PI * (phi - 1) / phi;
            var n = (Int32)((this.MaxR - this.MinR) / this.Distance);
            var a = 0.0;
            var r = MinR;

            foreach (var rawData in rawDatas)
            {
                var l = rawData;
                for (var i = 0; i < n; ++i)
                {
                    var v = new Vector3()
                        { 
                            X = l.X + r * (Single)Math.Cos(a),
                            Y = l.Y + r * (Single)Math.Sin(a),
                            Z = l.Z
                        };

                    a = fn((Single)(a + da), (Single)(2 * Math.PI));
                    r += Distance;

                    AddGeneratedItem(new QueryItem(v, v.GetType()));
                }
            }
        }

        private Single fn(Single a, Single b) 
        {
            return a - ((Single)Math.Floor(a / b) * b);
        }
    }
}
