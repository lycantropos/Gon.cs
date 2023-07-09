using System;

namespace Gon
{
    public class Point<Scalar> : IComparable<Point<Scalar>>, IEquatable<Point<Scalar>>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public Scalar x;
        public Scalar y;

        public Point(Scalar x, Scalar y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Point<Scalar> self, Point<Scalar> other)
        {
            if (ReferenceEquals(self, null))
                return false;
            return self.Equals(other);
        }

        public static bool operator <=(Point<Scalar> self, Point<Scalar> other)
        {
            if (ReferenceEquals(self, null))
                return false;
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(self, other))
                return true;
            return self.x.CompareTo(other.x) < 0
                || self.x.Equals(other.x) && self.y.CompareTo(other.y) <= 0;
        }

        public static bool operator >=(Point<Scalar> self, Point<Scalar> other)
        {
            if (ReferenceEquals(self, null))
                return false;
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(self, other))
                return true;
            return self.x.CompareTo(other.x) > 0
                || self.x.Equals(other.x) && self.y.CompareTo(other.y) >= 0;
        }

        public static bool operator !=(Point<Scalar> self, Point<Scalar> other) => !(self == other);

        public int CompareTo(Point<Scalar>? other)
        {
            if (ReferenceEquals(other, null))
                return 1;
            var result = x.CompareTo(other.x);
            return result == 0 ? y.CompareTo(other.y) : result;
        }

        public bool Equals(Point<Scalar>? other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return x.Equals(other.x) && y.Equals(other.y);
        }

        public override bool Equals(object? other) => Equals(other as Point<Scalar>);

        public override int GetHashCode() => (x, y).GetHashCode();
    }
}
