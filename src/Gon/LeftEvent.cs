using System;

namespace Gon
{
    class LeftEvent<Scalar> : Event<Scalar>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public readonly bool InteriorToLeft;
        public LeftEvent<Scalar>? BelowEventFromShapedResult = null;
        public UInt64 ContourId = UInt64.MaxValue;
        public bool FromInToOut = false;
        public bool FromResult = false;
        public UInt64 Id = UInt64.MaxValue;
        public bool OtherInteriorToLeft = false;
        public OverlapKind OverlapKind = OverlapKind.None;
        public UInt64 StartId = UInt64.MaxValue;

        public override Event<Scalar> Opposite
        {
            get { return _right!; }
        }

        public override bool IsFromFirstOperand
        {
            get { return _isFromFirstOperand; }
        }

        public override bool IsLeft { get; } = true;

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
            result._right = new RightEvent<Scalar>(end, result);
            return result;
        }

        public LeftEvent(
            Point<Scalar> start,
            RightEvent<Scalar>? right,
            bool interiorToLeft,
            bool isFromFirstOperand
        )
            : base(start)
        {
            _right = right;
            InteriorToLeft = interiorToLeft;
            _isFromFirstOperand = isFromFirstOperand;
        }

        private readonly bool _isFromFirstOperand;
        private RightEvent<Scalar>? _right;
    }
}
