using EQS.Classes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using EQS;

namespace EQS.Tests
{
    public class Test_Dot : QueryTest
    {
        private readonly List<Vector3> _contextRotation = new List<Vector3>();
        private List<Vector3> _lineBFromVector;
        private List<Vector3> _delayRotator;
        
        public IQueryContext Rotation;
        public IQueryContext LineFrom;
        public Single RotationDensity;

        internal override void OnRunTest()
        {
            var lineARotator = (this as IPrepareContext).PrepareContext_Rotation(Rotation, Querier);
            _lineBFromVector = (this as IPrepareContext).PrepareContext_Location(LineFrom, Querier);

            if (lineARotator.Count < 1 || _lineBFromVector.Count < 1) return;

            if (_delayRotator != null)
            {
                for (Int32 i = 0, l = lineARotator.Count; i < l; ++i)
                {
                    lineARotator[i] = System.Numerics.Vector3.Lerp(_delayRotator[i], lineARotator[i], RotationDensity);
                }
            }
            else 
            {
                _delayRotator = lineARotator;
            }

            foreach (var rotator in _delayRotator)
            {
                _contextRotation.Add(new Vector3() { Z = rotator.Z });
            }

            LoopOverItems();
        }

        internal override void OnQuery()
        {
            var lineBToVector = CurrentIterator.Location;

            foreach (var vector in _lineBFromVector)
            {
                var x = lineBToVector.X - vector.X;
                var y = lineBToVector.Y - vector.Y;

                var s = (Single)Math.Sqrt(x * x + y * y);

                x /= s;
                y /= s;

                foreach (var rotator in _contextRotation)
                {
                    var a = rotator;
                    var w = a.X * x + a.Y * y;

                    SetScore(w);
                }
            }
        }
    }
}
