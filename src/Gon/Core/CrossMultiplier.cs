using System;

namespace Gon
{
    internal static class CrossMultiplier<Scalar>
        where Scalar : IComparable<Scalar>,
            IEquatable<Scalar>
#if NET7_0_OR_GREATER
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
#if NET7_0_OR_GREATER
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
            return (castFirstEnd.X - castFirstStart.X) * (castSecondEnd.Y - castSecondStart.Y)
                - (castFirstEnd.Y - castFirstStart.Y) * (castSecondEnd.X - castSecondStart.X);
        }
    }
}
