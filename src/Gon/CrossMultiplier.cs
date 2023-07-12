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
            Point<Scalar> firstStart,
            Point<Scalar> firstEnd,
            Point<Scalar> secondStart,
            Point<Scalar> secondEnd
        )
        {
#if NET7_0
            Point<Scalar> castFirstStart = firstStart;
            Point<Scalar> castFirstEnd = firstEnd;
            Point<Scalar> castSecondStart = secondStart;
            Point<Scalar> castSecondEnd = secondEnd;
#else
            dynamic castFirstStart = firstStart;
            dynamic castFirstEnd = firstEnd;
            dynamic castSecondStart = secondStart;
            dynamic castSecondEnd = secondEnd;
#endif
            return (Scalar)(
                (castFirstEnd.X - castFirstStart.X) * (castSecondEnd.Y - castSecondStart.Y)
                - (castFirstEnd.Y - castFirstStart.Y) * (castSecondEnd.X - castSecondStart.X)
            );
        }
    }
}
