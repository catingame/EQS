using System;
using System.Collections.Generic;
using System.Text;

namespace EQS
{
    internal class Location
    {
        internal Single X = 0;
        internal Single Y = 0;
        internal Single Z = 0;

        internal System.Numerics.Vector3 To => new System.Numerics.Vector3(X, Y, Z);
    }

    internal class Rotation
    {
        internal Single Yaw = 0;
        internal Single Pitch = 0;
        internal Single Roll = 0;

        internal System.Numerics.Vector3 To => new System.Numerics.Vector3(Yaw, Pitch, Roll);
    }

    internal static class ExtensionMethod
    {
        internal static Rotation ToRotation(this System.Numerics.Vector3 v)
        {
            return new Rotation() { Yaw = v.X, Pitch = v.Y, Roll = v.Z };
        }
    }
}
