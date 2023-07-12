using System;

namespace Gon
{
    public readonly struct Segment<Scalar>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public readonly Point<Scalar> start;
        public readonly Point<Scalar> end;

        public Segment(Point<Scalar> start, Point<Scalar> end)
        {
            this.start = start;
            this.end = end;
        }

        public static bool operator ==(Segment<Scalar> self, Segment<Scalar> other) =>
            self.Equals(other);

        public static bool operator !=(Segment<Scalar> self, Segment<Scalar> other) =>
            !self.Equals(other);

        public bool Equals(Segment<Scalar> other) =>
            start.Equals(other.start) && end.Equals(other.end)
            || start.Equals(other.end) && end.Equals(other.start);

        public override bool Equals(object? other) =>
            (other is Segment<Scalar>) && Equals((Segment<Scalar>)other);

        public override int GetHashCode() => (start, end).GetHashCode();
    }
}
