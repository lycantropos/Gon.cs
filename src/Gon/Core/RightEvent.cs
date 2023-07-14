using System;

namespace Gon
{
    internal sealed class RightEvent<Scalar> : Event<Scalar>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public RightEvent(Point<Scalar> start, LeftEvent<Scalar> left)
        {
            _start = start;
            _left = left;
        }

        public override bool FromFirstOperand => _left.FromFirstOperand;

        public override bool FromResult => _left.FromResult;

        public override int Id
        {
            get => _id;
            set => _id = value;
        }

        public override bool IsLeft { get; } = false;

        public override Event<Scalar> Opposite
        {
            get => _left;
            set => _left = (LeftEvent<Scalar>)value;
        }

        public override Point<Scalar> Start => _start;

        public override int StartId
        {
            get => _startId;
            set => _startId = value;
        }

        private int _id = Constants.UndefinedIndex;
        private LeftEvent<Scalar> _left;
        private readonly Point<Scalar> _start;
        private int _startId = Constants.UndefinedIndex;
    }
}
