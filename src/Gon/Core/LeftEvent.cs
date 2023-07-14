using System;

namespace Gon
{
    internal sealed class LeftEvent<Scalar> : Event<Scalar>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public LeftEvent(
            bool isFromFirstOperand,
            Point<Scalar> start,
            RightEvent<Scalar>? right,
            bool interiorToLeft
        )
        {
            _isFromFirstOperand = isFromFirstOperand;
            _start = start;
            _right = right;
            InteriorToLeft = interiorToLeft;
        }

        public static LeftEvent<Scalar> FromEndpoints(
            bool isFromFirstOperand,
            Point<Scalar> start,
            Point<Scalar> end
        )
        {
            var interiorToLeft = true;
            if (start > end)
            {
                (start, end) = (end, start);
                interiorToLeft = false;
            }
            var result = new LeftEvent<Scalar>(isFromFirstOperand, start, null, interiorToLeft);
            result._right = new RightEvent<Scalar>(end, result);
            return result;
        }

        public readonly bool InteriorToLeft;
        public LeftEvent<Scalar>? BelowEventFromResult = null;
        public int ContourId = Constants.UndefinedIndex;
        public bool FromInToOut = false;
        public bool OtherInteriorToLeft = false;
        public OverlapKind OverlapKind = OverlapKind.None;

        public override bool FromFirstOperand => _isFromFirstOperand;

        public override bool FromResult => _fromResult;

        public override int Id
        {
            get => _id;
            set => _id = value;
        }

        public override bool IsLeft { get; } = true;

        public override Event<Scalar> Opposite
        {
            get => _right!;
            set => _right = (RightEvent<Scalar>)value;
        }

        public override Point<Scalar> Start => _start;

        public override int StartId
        {
            get => _startId;
            set => _startId = value;
        }

        public bool IsCommonRegionBoundary => OverlapKind == OverlapKind.SameOrientation;

        public bool Inside => OtherInteriorToLeft && OverlapKind == OverlapKind.None;

        public bool IsVertical => Start == End;

        public void SetFromResult(bool value)
        {
            _fromResult = value;
        }

        private bool _fromResult = false;
        private int _id = Constants.UndefinedIndex;
        private readonly bool _isFromFirstOperand;
        private RightEvent<Scalar>? _right;
        private Point<Scalar> _start;
        private int _startId = Constants.UndefinedIndex;
    }
}
