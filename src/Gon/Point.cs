using System;

namespace Gon
{
    public readonly struct Point<Scalar> : IComparable<Point<Scalar>>, IEquatable<Point<Scalar>>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public readonly Scalar X;
        public readonly Scalar Y;

        public Point(Scalar x, Scalar y)
        {
            X = x;
            Y = y;
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
            var result = X.CompareTo(other.X);
            return result == 0 ? Y.CompareTo(other.Y) : result;
        }

        public bool Equals(Point<Scalar> other) => X.Equals(other.X) && Y.Equals(other.Y);

        public override bool Equals(object? other) =>
            other is Point<Scalar> otherPoint && Equals(otherPoint);

        public override int GetHashCode() => HashCode.Combine(X, Y);
    }
}
