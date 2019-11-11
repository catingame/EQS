using EQS.Classes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EQS.Tests
{
    public enum TestDistance
    {
        Distance3D,
        Distance2D,
        DistanceZ,
        DistanceAbsoluteZ
    }

    public class Test_Distance : QueryTest
    {
        private readonly IQueryContext _distanceTo;
        private List<Vector3> _contextLocations;

        public TestDistance TestMode = TestDistance.Distance2D;

        public Test_Distance(IQueryContext distanceTo)
        {
            _distanceTo = distanceTo;
        }

        internal override void OnRunTest()
        {
            _contextLocations = (this as IPrepareContext).PrepareContext_Location(_distanceTo, Querier);
            LoopOverItems();
        }

        internal override void OnQuery()
        {
            foreach (var location in _contextLocations)
            {
                var l = location;
                var a = CurrentIterator.Location;

                var w = TestMode switch
                {
                    TestDistance.Distance2D => CalcDistance2D(l, a),
                    TestDistance.Distance3D => CalcDistance3D(l, a),
                    TestDistance.DistanceZ => CalcDistanceZ(l, a),
                    TestDistance.DistanceAbsoluteZ => CalcDistanceAbsoluteZ(l, a),
                    _ => 0f
                };

                SetScore(w);
            }
        }

        private Single CalcDistance3D(Vector3 posA, Vector3 posB)
        {
            return (posB - posA).Length();
        }

        private Single CalcDistance2D(Vector3 posA, Vector3 posB)
        {
            posB.Z = posA.Z = 0;
            return (posB - posA).Length();
        }

        private Single CalcDistanceZ(Vector3 posA, Vector3 posB)
        {
            return posB.Z - posA.Z;
        }

        private Single CalcDistanceAbsoluteZ(Vector3 posA, Vector3 posB)
        {
            return Math.Abs(posB.Z - posA.Z);
        }
    }
}
