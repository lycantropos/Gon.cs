using System;

namespace Gon
{
    class RightEvent<Scalar> : Event<Scalar>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public UInt64 Id = UInt64.MaxValue;
        public UInt64 StartId = UInt64.MaxValue;

        public override Event<Scalar> Opposite
        {
            get { return _left; }
        }

        public override bool IsFromFirstOperand
        {
            get { return _left.IsFromFirstOperand; }
        }

        public override bool IsLeft { get; } = false;

        public RightEvent(Point<Scalar> start, LeftEvent<Scalar> left)
            : base(start)
        {
            _left = left;
        }

        private LeftEvent<Scalar> _left;
    }
}
