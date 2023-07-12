using System;

namespace Gon
{
    public readonly struct Contour<Scalar> : IBounded<Scalar>
        where Scalar : IComparable<Scalar>,
            IComparable<Int32>,
            IEquatable<Scalar>
#if NET7_0_OR_GREATER
            ,
            System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
            System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
    {
        public Contour(Point<Scalar>[] vertices)
        {
            Vertices = vertices;
        }

        public readonly Point<Scalar>[] Vertices;

        public Box<Scalar> BoundingBox
        {
            get
            {
                var vertices = Vertices.GetEnumerator();
                vertices.MoveNext();
                var firstVertex = (Point<Scalar>)vertices.Current;
                vertices.MoveNext();
                var secondVertex = (Point<Scalar>)vertices.Current;
                var (minX, maxX) =
                    firstVertex.X.CompareTo(secondVertex.X) < 0
                        ? (firstVertex.X, secondVertex.X)
                        : (secondVertex.X, firstVertex.X);
                var (minY, maxY) =
                    firstVertex.Y.CompareTo(secondVertex.Y) < 0
                        ? (firstVertex.Y, secondVertex.Y)
                        : (secondVertex.Y, firstVertex.Y);
                while (vertices.MoveNext())
                {
                    var vertex = (Point<Scalar>)vertices.Current;
                    if (vertex.X.CompareTo(minX) < 0)
                    {
                        minX = vertex.X;
                    }
                    else if (vertex.X.CompareTo(maxX) > 0)
                    {
                        maxX = vertex.X;
                    }
                    if (vertex.Y.CompareTo(minY) < 0)
                    {
                        minY = vertex.Y;
                    }
                    else if (vertex.Y.CompareTo(maxY) > 0)
                    {
                        maxY = vertex.Y;
                    }
                }
                return new Box<Scalar>(minX, maxX, minY, maxY);
            }
        }

        public Orientation Orientation
        {
            get
            {
                int minVertexIndex = 0;
                for (int index = 1; index < Vertices.Length; ++index)
                {
                    if (Vertices[minVertexIndex] < Vertices[index])
                    {
                        minVertexIndex = index;
                    }
                }
                return Orienteer<Scalar>.Orient(
                    Vertices[minVertexIndex == 0 ? Vertices.Length - 1 : minVertexIndex - 1],
                    Vertices[minVertexIndex],
                    Vertices[(minVertexIndex + 1) % Vertices.Length]
                );
            }
        }

        public int SegmentsCount
        {
            get { return Vertices.Length; }
        }

        public Segment<Scalar>[] Segments
        {
            get { return Utils.ContourVerticesToSegments<Scalar>(Vertices); }
        }

        public int VerticesCount
        {
            get { return Vertices.Length; }
        }
    }
}
