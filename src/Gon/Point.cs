using System;

namespace Gon
{
    public readonly struct Point<Scalar> : IComparable<Point<Scalar>>, IEquatable<Point<Scalar>>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public readonly Scalar x;
        public readonly Scalar y;

        public Point(Scalar x, Scalar y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Point<Scalar> self, Point<Scalar> other) =>
            self.Equals(other);

        public static bool operator !=(Point<Scalar> self, Point<Scalar> other) =>
            !self.Equals(other);

        public static bool operator >=(Point<Scalar> self, Point<Scalar> other) =>
            self.CompareTo(other) >= 0;

        public static bool operator >(Point<Scalar> self, Point<Scalar> other) =>
            self.CompareTo(other) > 0;

        public static bool operator <=(Point<Scalar> self, Point<Scalar> other) =>
            self.CompareTo(other) <= 0;

        public static bool operator <(Point<Scalar> self, Point<Scalar> other) =>
            self.CompareTo(other) < 0;

        public int CompareTo(Point<Scalar> other)
        {
            var result = x.CompareTo(other.x);
            return result == 0 ? y.CompareTo(other.y) : result;
        }

        public bool Equals(Point<Scalar> other) => x.Equals(other.x) && y.Equals(other.y);

        public override bool Equals(object? other) =>
            (other is Point<Scalar>) && Equals((Point<Scalar>)other);

        public override int GetHashCode() => (x, y).GetHashCode();
    }
}
