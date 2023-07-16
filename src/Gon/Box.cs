using System;

namespace Gon
{
    public readonly struct Box<Scalar> : IEquatable<Box<Scalar>>
        where Scalar : IEquatable<Scalar>, IComparable<Scalar>
    {
        public Box(Scalar minX, Scalar maxX, Scalar minY, Scalar maxY)
        {
            MaxX = maxX;
            MaxY = maxY;
            MinX = minX;
            MinY = minY;
        }

        public readonly Scalar MaxX;
        public readonly Scalar MaxY;
        public readonly Scalar MinX;
        public readonly Scalar MinY;

        public static bool operator ==(Box<Scalar> self, Box<Scalar> other) => self.Equals(other);

        public static bool operator !=(Box<Scalar> self, Box<Scalar> other) => !self.Equals(other);

        public bool DisjointWith(Box<Scalar> other)
        {
            return (
                other.MaxX.CompareTo(MinX) < 0
                || MaxX.CompareTo(other.MinX) < 0
                || other.MaxY.CompareTo(MinY) < 0
                || MaxY.CompareTo(other.MinY) < 0
            );
        }

        public bool Equals(Box<Scalar> other)
        {
            return MinX.Equals(other.MinX)
                && MaxX.Equals(other.MaxX)
                && MinY.Equals(other.MinY)
                && MaxY.Equals(other.MaxY);
        }

        public bool Touches(Box<Scalar> other)
        {
            return (
                (
                    (MinX.Equals(other.MaxX) || MaxX.Equals(other.MinX))
                    && (MinY.CompareTo(other.MaxY) <= 0 && other.MinY.CompareTo(MaxY) <= 0)
                )
                || (
                    (MinX.CompareTo(other.MaxX) <= 0 && other.MinX.CompareTo(MaxX) <= 0)
                    && (MinY.Equals(other.MaxY) || other.MinY.Equals(MaxY))
                )
            );
        }

        public override bool Equals(object other) =>
            other is Box<Scalar> otherBox && Equals(otherBox);

        public override int GetHashCode() => Core.HashValues(MinX, MaxX, MinY, MaxY);
        
        public override string ToString() => $"Box({MinX}, {MaxX}, {MinY}, {MaxY})";
    }
}
