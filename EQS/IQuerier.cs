using System.Numerics;

namespace EQS
{
    public interface IQuerier
    {
        Vector3 GetLocation();
        Vector3 GetRotation();
    }
}