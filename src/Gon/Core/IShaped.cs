using System;

namespace Gon
{
    internal static partial class Core
    {
        public interface IShaped<Scalar>
            where Scalar : IComparable<Scalar>,
                IEquatable<Scalar>
#if NET7_0_OR_GREATER
                ,
                System.Numerics.IAdditionOperators<Scalar, Scalar, Scalar>,
                System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
                System.Numerics.IDivisionOperators<Scalar, Scalar, Scalar>,
                System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
        {
            Polygon<Scalar>[] ToPolygons();
        }
    }
}
