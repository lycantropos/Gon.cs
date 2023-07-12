using System;

namespace Gon
{
    public readonly struct Segment<Scalar>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public readonly Point<Scalar> Start;
        public readonly Point<Scalar> End;

        public Segment(Point<Scalar> start, Point<Scalar> end)
        {
            Start = start;
            End = end;
        }

        public static bool operator ==(Segment<Scalar> self, Segment<Scalar> other) =>
            self.Equals(other);

        public static bool operator !=(Segment<Scalar> self, Segment<Scalar> other) =>
            !self.Equals(other);

        public bool Equals(Segment<Scalar> other) =>
            Start.Equals(other.Start) && End.Equals(other.End)
            || Start.Equals(other.End) && End.Equals(other.Start);

        public override bool Equals(object? other) =>
            (other is Segment<Scalar>) && Equals((Segment<Scalar>)other);

        public override int GetHashCode() =>
            (Start < End ? (Start, End) : (End, Start)).GetHashCode();
    }
}
