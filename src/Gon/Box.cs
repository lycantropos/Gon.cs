using System;

namespace Gon
{
    public readonly struct Box<Scalar> : IEquatable<Box<Scalar>>
        where Scalar : IEquatable<Scalar>
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

        public bool Equals(Box<Scalar> other)
        {
            return MinX.Equals(other.MinX)
                && MaxX.Equals(other.MaxX)
                && MinY.Equals(other.MinY)
                && MaxY.Equals(other.MaxY);
        }

        public override bool Equals(object? other) =>
            other is Box<Scalar> otherBox && Equals(otherBox);

        public override int GetHashCode()
        {
            return (MinX, MaxX, MinY, MaxY).GetHashCode();
        }
    }
}
