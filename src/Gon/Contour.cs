using System;

namespace Gon
{
    public readonly struct Contour<Scalar>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public readonly Point<Scalar>[] Vertices;

        public Contour(Point<Scalar>[] vertices)
        {
            Vertices = vertices;
        }
    }
}
