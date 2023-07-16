using System;

namespace Gon
{
    public readonly struct Segment<Scalar> : IBounded<Scalar>, IEquatable<Segment<Scalar>>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public Segment(Point<Scalar> start, Point<Scalar> end)
        {
            Start = start;
            End = end;
        }

        public readonly Point<Scalar> Start;
        public readonly Point<Scalar> End;

        public Box<Scalar> BoundingBox
        {
            get
            {
                var (minX, maxX) =
                    Start.X.CompareTo(End.X) < 0 ? (Start.X, End.X) : (End.X, Start.X);
                var (minY, maxY) =
                    Start.Y.CompareTo(End.Y) < 0 ? (Start.Y, End.Y) : (End.Y, Start.Y);
                return new Box<Scalar>(minX, maxX, minY, maxY);
            }
        }

        public static bool operator ==(Segment<Scalar> self, Segment<Scalar> other) =>
            self.Equals(other);

        public static bool operator !=(Segment<Scalar> self, Segment<Scalar> other) =>
            !self.Equals(other);

        public bool Equals(Segment<Scalar> other) =>
            Start.Equals(other.Start) && End.Equals(other.End)
            || Start.Equals(other.End) && End.Equals(other.Start);

        public override bool Equals(object other) =>
            other is Segment<Scalar> otherSegment && Equals(otherSegment);

        public override int GetHashCode() =>
            Start < End ? Core.HashValues(Start, End) : Core.HashValues(End, Start);

        public override string ToString() => $"Segment({Start}, {End})";
    }
}
