using System;

namespace Gon
{
    public readonly struct Contour<Scalar>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public readonly Point<Scalar>[] vertices;

        public Contour(Point<Scalar>[] vertices)
        {
            this.vertices = vertices;
        }
    }
}
