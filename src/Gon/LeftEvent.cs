using System;

namespace Gon
{
    class LeftEvent<Scalar> : Event<Scalar>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public RightEvent<Scalar>? right;
        public readonly bool interiorToLeft;
        public readonly bool fromFirstOperand;
        public LeftEvent<Scalar>? belowEventFromShapedResult = null;
        public UInt64 contourId = UInt64.MaxValue;
        public bool fromInToOut = false;
        public bool fromResult = false;
        public UInt64 id = UInt64.MaxValue;
        public bool otherInteriorToLeft = false;
        public OverlapKind overlap_kind = OverlapKind.None;
        public UInt64 startId = UInt64.MaxValue;

        public override Event<Scalar> opposite
        {
            get { return right!; }
        }

        public override bool isLeft { get; } = true;

        static LeftEvent<Scalar> FromEndpoints(
            Point<Scalar> start,
            Point<Scalar> end,
            bool fromFirstOperand
        )
        {
            var interiorToLeft = true;
            if (start < end)
            {
                (start, end) = (end, start);
                interiorToLeft = false;
            }
            var result = new LeftEvent<Scalar>(start, null, interiorToLeft, fromFirstOperand);
            result.right = new RightEvent<Scalar>(end, result);
            return result;
        }

        public LeftEvent(
            Point<Scalar> start,
            RightEvent<Scalar>? right,
            bool interiorToLeft,
            bool fromFirstOperand
        )
            : base(start)
        {
            this.right = right;
            this.interiorToLeft = interiorToLeft;
            this.fromFirstOperand = fromFirstOperand;
        }
    }
}
