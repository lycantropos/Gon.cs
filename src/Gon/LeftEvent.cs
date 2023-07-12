using System;

namespace Gon
{
    public class LeftEvent<Scalar> : Event<Scalar>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public LeftEvent(
            bool isFromFirstOperand,
            Point<Scalar> start,
            RightEvent<Scalar>? right,
            bool interiorToLeft
        )
            : base(start)
        {
            _right = right;
            InteriorToLeft = interiorToLeft;
            _isFromFirstOperand = isFromFirstOperand;
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

        public override bool FromFirstOperand
        {
            get { return _isFromFirstOperand; }
        }

        public override bool FromResult
        {
            get { return _fromResult; }
        }

        public override int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public override bool IsLeft { get; } = true;

        public override Event<Scalar> Opposite
        {
            get { return _right!; }
            set { _right = (RightEvent<Scalar>)value; }
        }

        public override int StartId
        {
            get { return _startId; }
            set { _startId = value; }
        }

        public bool IsCommonRegionBoundary
        {
            get { return OverlapKind == OverlapKind.SameOrientation; }
        }

        public bool Inside
        {
            get { return OtherInteriorToLeft && OverlapKind == OverlapKind.None; }
        }

        public bool IsVertical
        {
            get { return Start == End; }
        }

        public void SetFromResult(bool value)
        {
            _fromResult = value;
        }

        private bool _fromResult = false;
        private int _id = Constants.UndefinedIndex;
        private readonly bool _isFromFirstOperand;
        private RightEvent<Scalar>? _right;
        private int _startId = Constants.UndefinedIndex;
    }
}
