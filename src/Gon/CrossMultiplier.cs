using System;

namespace Gon
{
    public static class CrossMultiplier<Scalar>
        where Scalar : IComparable<Scalar>,
            IEquatable<Scalar>
#if NET7_0
            ,
            System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
            System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
    {
        public static Scalar CrossMultiply(
            Point<Scalar> first_start,
            Point<Scalar> first_end,
            Point<Scalar> second_start,
            Point<Scalar> second_end
        )
        {
#if NET7_0
            Point<Scalar> cast_first_start = first_start;
            Point<Scalar> cast_first_end = first_end;
            Point<Scalar> cast_second_start = second_start;
            Point<Scalar> cast_second_end = second_end;
#else
            dynamic cast_first_start = first_start;
            dynamic cast_first_end = first_end;
            dynamic cast_second_start = second_start;
            dynamic cast_second_end = second_end;
#endif
            return (Scalar)(
                (cast_first_end.x - cast_first_start.x) * (cast_second_end.y - cast_second_start.y)
                - (cast_first_end.y - cast_first_start.y)
                    * (cast_second_end.x - cast_second_start.x)
            );
        }
    }
}
