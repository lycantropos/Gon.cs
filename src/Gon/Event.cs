using System;

namespace Gon
{
    public abstract class Event<Scalar>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public readonly Point<Scalar> Start;

        public Point<Scalar> End
        {
            get { return Opposite.Start; }
        }

        public abstract bool IsFromFirstOperand { get; }

        public abstract bool IsLeft { get; }

        public abstract Event<Scalar> Opposite { get; }

        protected Event(Point<Scalar> start)
        {
            Start = start;
        }
    }
}
