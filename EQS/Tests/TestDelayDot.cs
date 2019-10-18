using EQS.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using EQS;

namespace EQS.Tests
{
    public class TestDelayDot : QueryTest
    {
        readonly private IQueryContext _rotation;
        readonly private IQueryContext _lineFrom;
        readonly private Single _rotationDensity;

        private List<Location> LineBFrom_Vector;
        private List<Rotation> DelayRotator;
        private List<Rotation> ContextRotation = new List<Rotation>();

        public TestDelayDot(in IQueryContext Rotation, in IQueryContext LineFrom, Single RotationDensity) 
        {
            _rotation = Rotation;
            _lineFrom = LineFrom;
            _rotationDensity = RotationDensity;
        }
        internal override void OnRunTest()
        {
            var LineA_Rotator = (this as IPrepareContext).PrepareContext_Rotation(_rotation, Querier);
            LineBFrom_Vector = (this as IPrepareContext).PrepareContext_Location(_lineFrom, Querier);

            if (LineA_Rotator.Count < 1 || LineBFrom_Vector.Count < 1) return;

            if (DelayRotator != null)
            {
                for (Int32 i = 0, l = LineA_Rotator.Count; i < l; ++i)
                {
                    LineA_Rotator[i] = System.Numerics.Vector3.Lerp(DelayRotator[i].To, LineA_Rotator[i].To, _rotationDensity).ToRotation();
                }
            }
            else 
            {
                DelayRotator = LineA_Rotator;
            }

            foreach (var rotator in DelayRotator)
            {
                ContextRotation.Add(new Rotation() { Yaw = rotator.Yaw });
            }

            LoopOverItems();
        }

        internal override void OnQuery()
        {
            var LineBTo_Vector = CurrentIterator.Location;

            foreach (var vector in LineBFrom_Vector)
            {
                var x = LineBTo_Vector.X - vector.X;
                var y = LineBTo_Vector.Y - vector.Y;

                var s = (Single)Math.Sqrt(x * x + y * y);

                x /= s;
                y /= s;

                foreach (var rotator in ContextRotation)
                {
                    var a = rotator.To;
                    var w = a.X * x + a.Y * y;

                    SetScoreSingle(w);
                }
            }
        }
    }
}
