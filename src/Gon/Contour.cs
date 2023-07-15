using System;
using System.Diagnostics;

namespace Gon
{
    public readonly struct Contour<Scalar> : IBounded<Scalar>, IEquatable<Contour<Scalar>>
        where Scalar : IComparable<Scalar>,
            IComparable<int>,
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
                _ = vertices.MoveNext();
                var firstVertex = (Point<Scalar>)vertices.Current;
                _ = vertices.MoveNext();
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
                var minVertexIndex = MinVertexIndex;
                return Core.Orient(
                    Vertices[minVertexIndex == 0 ? Vertices.Length - 1 : minVertexIndex - 1],
                    Vertices[minVertexIndex],
                    Vertices[(minVertexIndex + 1) % Vertices.Length]
                );
            }
        }

        public int SegmentsCount => Vertices.Length;

        public Segment<Scalar>[] Segments => Core.ContourVerticesToSegments(Vertices);

        public int VerticesCount => Vertices.Length;

        public static bool operator ==(Contour<Scalar> self, Contour<Scalar> other) =>
            self.Equals(other);

        public static bool operator !=(Contour<Scalar> self, Contour<Scalar> other) =>
            !self.Equals(other);

        public bool Equals(Contour<Scalar> other) => AreVerticesEqual(Vertices, other.Vertices);

        public override bool Equals(object other) =>
            other is Contour<Scalar> otherContour && Equals(otherContour);

        public override int GetHashCode()
        {
            var minVertexIndex = MinVertexIndex;
            var orientation = Core.Orient(
                Vertices[minVertexIndex == 0 ? Vertices.Length - 1 : minVertexIndex - 1],
                Vertices[minVertexIndex],
                Vertices[(minVertexIndex + 1) % Vertices.Length]
            );
            var multiplier = InitialHashMultiplier;
            var result = 3430008;
            var index = 0;
            if (orientation == Orientation.Clockwise)
            {
                for (int position = 0; position <= minVertexIndex; ++position)
                {
                    ++index;
                    result =
                        (result ^ Vertices[minVertexIndex - position].GetHashCode()) * multiplier;
                    multiplier += (MultiplierIncrement + index + index);
                }
                for (int position = 1; position < Vertices.Length - minVertexIndex; ++position)
                {
                    ++index;
                    result =
                        (result ^ Vertices[Vertices.Length - position].GetHashCode()) * multiplier;
                    multiplier += (MultiplierIncrement + index + index);
                }
            }
            else
            {
                for (int position = minVertexIndex; position < Vertices.Length; ++position)
                {
                    ++index;
                    result = (result ^ Vertices[position].GetHashCode()) * multiplier;
                    multiplier += (MultiplierIncrement + index + index);
                }
                for (int position = 0; position < minVertexIndex; ++position)
                {
                    ++index;
                    result = (result ^ Vertices[position].GetHashCode()) * multiplier;
                    multiplier += (MultiplierIncrement + index + index);
                }
            }
            result += 97531;
            return result;
        }

        private int MinVertexIndex
        {
            get
            {
                int result = 0;
                for (int index = 1; index < Vertices.Length; ++index)
                {
                    if (Vertices[result] < Vertices[index])
                    {
                        result = index;
                    }
                }
                return result;
            }
        }

        private const int InitialHashMultiplier = 1000003;
        private const int MultiplierIncrement = 82520;

        private static bool AreVerticesEqual(Point<Scalar>[] first, Point<Scalar>[] second)
        {
            if (first.Length != second.Length)
            {
                return false;
            }
            var secondOffset = Array.IndexOf(second, first[0]);
            if (secondOffset < 0)
            {
                return false;
            }
            Debug.Assert(first[0] == second[secondOffset]);
            if (first[1] == second[(secondOffset + 1) % second.Length])
            {
                for (int index = 2; index < second.Length - secondOffset; ++index)
                {
                    if (first[index] != second[secondOffset + index])
                    {
                        return false;
                    }
                }
                for (int index = 0; index < secondOffset; ++index)
                {
                    if (first[first.Length - secondOffset + index] != second[index])
                    {
                        return false;
                    }
                }
                return true;
            }
            else if (first[1] == second[(second.Length + (secondOffset - 1)) % second.Length])
            {
                for (int index = 2; index <= secondOffset; ++index)
                {
                    if (first[index] != second[secondOffset - index])
                    {
                        return false;
                    }
                }
                for (int index = secondOffset + 1; index < second.Length; ++index)
                {
                    if (first[first.Length + secondOffset - index] != second[index])
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
