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

        public int EndId
        {
            get { return Opposite.StartId; }
        }

        public abstract bool FromFirstOperand { get; }

        public abstract bool FromResult { get; }

        public abstract int Id { get; set; }

        public abstract bool IsLeft { get; }

        public abstract Event<Scalar> Opposite { get; set; }

        public abstract int StartId { get; set; }

        protected Event(Point<Scalar> start)
        {
            Start = start;
        }
    }
}
