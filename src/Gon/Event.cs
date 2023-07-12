using System;

namespace Gon
{
    public abstract class Event<Scalar>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public readonly Point<Scalar> start;

        public Point<Scalar> end
        {
            get { return opposite.start; }
        }

        public abstract bool isLeft { get; }

        public abstract Event<Scalar> opposite { get; }

        protected Event(Point<Scalar> start)
        {
            this.start = start;
        }
    }
}
