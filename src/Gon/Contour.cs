using System;

namespace Gon
{
    public class Contour<Scalar>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        public Point<Scalar>[] vertices;

        public Contour(Point<Scalar>[] vertices)
        {
            this.vertices = vertices;
        }
    }
}