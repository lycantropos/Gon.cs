using System;

namespace Gon
{
    internal static partial class Core
    {
        public abstract class Event<Scalar>
            where Scalar : IComparable<Scalar>, IEquatable<Scalar>
        {
            public abstract Point<Scalar> Start { get; }

            public Point<Scalar> End => Opposite.Start;

            public int EndId => Opposite.StartId;

            public abstract bool FromFirstOperand { get; }

            public abstract bool FromResult { get; }

            public abstract int Id { get; set; }

            public abstract bool IsLeft { get; }

            public abstract Event<Scalar> Opposite { get; set; }

            public abstract int StartId { get; set; }
        }
    }
}
