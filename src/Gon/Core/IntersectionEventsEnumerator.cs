using System;

namespace Gon
{
    internal class IntersectionEventsNumerator<Scalar> : EventsEnumerator<Scalar>
        where Scalar : IComparable<Scalar>,
            IComparable<int>,
            IEquatable<Scalar>
#if NET7_0_OR_GREATER
            ,
            System.Numerics.IAdditionOperators<Scalar, Scalar, Scalar>,
            System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
            System.Numerics.IDivisionOperators<Scalar, Scalar, Scalar>,
            System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
    {
        public IntersectionEventsNumerator(
            Segment<Scalar>[] firstSegments,
            Segment<Scalar>[] secondSegments
        )
            : base(firstSegments, secondSegments) { }

        protected override bool FromResult(LeftEvent<Scalar> event_)
        {
            return event_.Inside || (!event_.FromFirstOperand && event_.IsCommonRegionBoundary);
        }
    }
}
