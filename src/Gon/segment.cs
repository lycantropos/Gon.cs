using System;

namespace Gon
{
    public class Segment<Scalar>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public Point<Scalar> start;
        public Point<Scalar> end;

        public Segment(Point<Scalar> start, Point<Scalar> end)
        {
            this.start = start;
            this.end = end;
        }

        public static bool operator ==(Segment<Scalar> self, Segment<Scalar> other)
        {
            if (ReferenceEquals(self, null))
                return false;
            return self.Equals(other);
        }

        public static bool operator !=(Segment<Scalar> self, Segment<Scalar> other) =>
            !(self == other);

        public bool Equals(Segment<Scalar> other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return start.Equals(other.start) && end.Equals(other.end)
                || start.Equals(other.end) && end.Equals(other.start);
        }

        public override bool Equals(object other) => Equals(other as Segment<Scalar>);

        public override int GetHashCode() => (start, end).GetHashCode();
    }
}
