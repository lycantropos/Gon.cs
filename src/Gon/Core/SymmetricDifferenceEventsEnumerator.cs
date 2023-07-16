using System;

namespace Gon
{
    internal static partial class Core
    {
        public sealed class SymmetricDifferenceEventsNumerator<Scalar> : EventsEnumerator<Scalar>
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
            public SymmetricDifferenceEventsNumerator(
                Segment<Scalar>[] firstSegments,
                Segment<Scalar>[] secondSegments
            )
                : base(firstSegments, secondSegments) { }

            protected override bool FromResult(LeftEvent<Scalar> event_)
            {
                return !event_.IsOverlap;
            }
        }
    }
}
