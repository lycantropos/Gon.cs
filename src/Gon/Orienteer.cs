using System;

namespace Gon
{
    public static class Orienteer<Scalar>
        where Scalar : IComparable<Scalar>,
            IComparable<Int32>,
            IEquatable<Scalar>
#if NET7_0_OR_GREATER
            ,
            System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
            System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
    {
        public static Orientation Orient(
            Point<Scalar> vertex,
            Point<Scalar> firstRayPoint,
            Point<Scalar> secondRayPoint
        )
        {
            var comparisonResult = CrossMultiplier<Scalar>
                .CrossMultiply(vertex, firstRayPoint, vertex, secondRayPoint)
                .CompareTo(0);
            return comparisonResult == 0
                ? Orientation.Collinear
                : (comparisonResult > 0 ? Orientation.Counterclockwise : Orientation.Clockwise);
        }
    }
}
