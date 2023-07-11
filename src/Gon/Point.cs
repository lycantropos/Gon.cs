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

        public static bool operator ==(Point<Scalar>? self, Point<Scalar>? other) =>
            ReferenceEquals(self, null) ? ReferenceEquals(other, null) : self.Equals(other);

        public static bool operator !=(Point<Scalar>? self, Point<Scalar>? other) =>
            ReferenceEquals(self, null) ? !ReferenceEquals(other, null) : !self.Equals(other);

        public static bool operator >=(Point<Scalar>? self, Point<Scalar>? other) =>
            !ReferenceEquals(self, null) && self.CompareTo(other) >= 0;

        public static bool operator >(Point<Scalar>? self, Point<Scalar>? other) =>
            !ReferenceEquals(self, null) && self.CompareTo(other) > 0;

        public static bool operator <=(Point<Scalar>? self, Point<Scalar>? other) =>
            !ReferenceEquals(self, null) && self.CompareTo(other) <= 0;

        public static bool operator <(Point<Scalar>? self, Point<Scalar>? other) =>
            !ReferenceEquals(self, null) && self.CompareTo(other) < 0;

        public int CompareTo(Point<Scalar>? other)
        {
            if (ReferenceEquals(other, null))
                return 1;
            if (ReferenceEquals(this, other))
                return 0;
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

    public static class CrossMultiplier<Scalar>
        where Scalar : IComparable<Scalar>,
            IEquatable<Scalar>
#if NET7_0
            ,
            System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
            System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
    {
        public static Scalar CrossMultiply(
            Point<Scalar> first_start,
            Point<Scalar> first_end,
            Point<Scalar> second_start,
            Point<Scalar> second_end
        )
        {
#if NET7_0
            Point<Scalar> cast_first_start = first_start;
            Point<Scalar> cast_first_end = first_end;
            Point<Scalar> cast_second_start = second_start;
            Point<Scalar> cast_second_end = second_end;
#else
            dynamic cast_first_start = first_start;
            dynamic cast_first_end = first_end;
            dynamic cast_second_start = second_start;
            dynamic cast_second_end = second_end;
#endif
            return (Scalar)(
                (cast_first_end.x - cast_first_start.x) * (cast_second_end.y - cast_second_start.y)
                - (cast_first_end.y - cast_first_start.y)
                    * (cast_second_end.x - cast_second_start.x)
            );
        }
    }
}
