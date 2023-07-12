using System;

namespace Gon
{
    public static class Orienter<Scalar>
        where Scalar : IComparable<Scalar>,
            IComparable<int>,
            IEquatable<Scalar>
#if NET7_0
            ,
            System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
            System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
    {
        public static Orientation Orient(
            Point<Scalar> vertex,
            Point<Scalar> first_ray_point,
            Point<Scalar> second_ray_point
        )
        {
            var comparisonResult = CrossMultiplier<Scalar>
                .CrossMultiply(vertex, first_ray_point, vertex, second_ray_point)
                .CompareTo(0);
            return comparisonResult == 0
                ? Orientation.Collinear
                : (comparisonResult > 0 ? Orientation.Counterclockwise : Orientation.Clockwise);
        }
    }
}
