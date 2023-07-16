using System;
using System.Collections.Generic;

namespace Gon
{
    public readonly struct Multipolygon<Scalar>
        : IBounded<Scalar>,
            IEquatable<Multipolygon<Scalar>>,
            Core.IShaped<Scalar>
        where Scalar : IComparable<Scalar>,
            IEquatable<Scalar>
#if NET7_0_OR_GREATER
            ,
            System.Numerics.IAdditionOperators<Scalar, Scalar, Scalar>,
            System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
            System.Numerics.IDivisionOperators<Scalar, Scalar, Scalar>,
            System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
    {
        public Multipolygon(Polygon<Scalar>[] polygons)
        {
            Polygons = polygons;
        }

        public readonly Polygon<Scalar>[] Polygons;

        public Box<Scalar> BoundingBox
        {
            get
            {
                var polygons = Polygons.GetEnumerator();
                _ = polygons.MoveNext();
                var firstPolygonBoundingBox = ((Polygon<Scalar>)polygons.Current).BoundingBox;
                _ = polygons.MoveNext();
                var secondPolygonBoundingBox = ((Polygon<Scalar>)polygons.Current).BoundingBox;
                var minX =
                    firstPolygonBoundingBox.MinX.CompareTo(secondPolygonBoundingBox.MinX) < 0
                        ? firstPolygonBoundingBox.MinX
                        : secondPolygonBoundingBox.MinX;
                var minY =
                    firstPolygonBoundingBox.MinY.CompareTo(secondPolygonBoundingBox.MinY) < 0
                        ? firstPolygonBoundingBox.MinY
                        : secondPolygonBoundingBox.MinY;
                var maxX =
                    firstPolygonBoundingBox.MaxX.CompareTo(secondPolygonBoundingBox.MaxX) < 0
                        ? firstPolygonBoundingBox.MaxX
                        : secondPolygonBoundingBox.MaxX;
                var maxY =
                    firstPolygonBoundingBox.MaxY.CompareTo(secondPolygonBoundingBox.MaxY) < 0
                        ? firstPolygonBoundingBox.MaxY
                        : secondPolygonBoundingBox.MaxY;
                while (polygons.MoveNext())
                {
                    var boundingBox = ((Polygon<Scalar>)polygons.Current).BoundingBox;
                    if (boundingBox.MinX.CompareTo(minX) < 0)
                    {
                        minX = boundingBox.MinX;
                    }
                    if (boundingBox.MaxX.CompareTo(maxX) > 0)
                    {
                        maxX = boundingBox.MaxX;
                    }
                    if (boundingBox.MinY.CompareTo(minY) < 0)
                    {
                        minY = boundingBox.MinY;
                    }
                    if (boundingBox.MaxY.CompareTo(maxY) > 0)
                    {
                        maxY = boundingBox.MaxY;
                    }
                }
                return new Box<Scalar>(minX, maxX, minY, maxY);
            }
        }

        public int PolygonsCount => Polygons.Length;

        public static Polygon<Scalar>[] operator &(
            Multipolygon<Scalar> self,
            Polygon<Scalar> other
        ) => Core.Operation<Scalar>.Intersect(self, other);

        public static Polygon<Scalar>[] operator &(
            Multipolygon<Scalar> self,
            Multipolygon<Scalar> other
        ) => Core.Operation<Scalar>.Intersect(self, other);

        public static Polygon<Scalar>[] operator |(
            Multipolygon<Scalar> self,
            Multipolygon<Scalar> other
        ) => Core.Operation<Scalar>.Unite(self, other);

        public static Polygon<Scalar>[] operator |(
            Multipolygon<Scalar> self,
            Polygon<Scalar> other
        ) => Core.Operation<Scalar>.Unite(self, other);

        public static Polygon<Scalar>[] operator ^(
            Multipolygon<Scalar> self,
            Multipolygon<Scalar> other
        ) => Core.Operation<Scalar>.SymmetricSubtract(self, other);

        public static Polygon<Scalar>[] operator ^(
            Multipolygon<Scalar> self,
            Polygon<Scalar> other
        ) => Core.Operation<Scalar>.SymmetricSubtract(self, other);

        public static Polygon<Scalar>[] operator -(
            Multipolygon<Scalar> self,
            Multipolygon<Scalar> other
        ) => Core.Operation<Scalar>.Subtract(self, other);

        public static Polygon<Scalar>[] operator -(
            Multipolygon<Scalar> self,
            Polygon<Scalar> other
        ) => Core.Operation<Scalar>.Subtract(self, other);

        public static bool operator ==(Multipolygon<Scalar> self, Multipolygon<Scalar> other) =>
            self.Equals(other);

        public static bool operator !=(Multipolygon<Scalar> self, Multipolygon<Scalar> other) =>
            !self.Equals(other);

        public bool Equals(Multipolygon<Scalar> other) =>
            new HashSet<Polygon<Scalar>>(Polygons) == new HashSet<Polygon<Scalar>>(other.Polygons);

        public override bool Equals(object other) =>
            other is Multipolygon<Scalar> otherMultipolygon && Equals(otherMultipolygon);

        public override int GetHashCode() => Core.Hashing.HashUnorderedUniqueIterable(Polygons);

        Polygon<Scalar>[] Core.IShaped<Scalar>.ToPolygons() => Polygons;
    }
}
