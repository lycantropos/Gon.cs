using System;

namespace Gon
{
    class RightEvent<Scalar> : Event<Scalar>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public LeftEvent<Scalar> left;
        public UInt64 id = UInt64.MaxValue;
        public UInt64 startId = UInt64.MaxValue;

        public override Event<Scalar> opposite
        {
            get { return left; }
        }

        public override bool isLeft { get; } = false;

        public RightEvent(Point<Scalar> start, LeftEvent<Scalar> left)
            : base(start)
        {
            this.left = left;
        }
    }
}
