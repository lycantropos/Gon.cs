using System;
using System.Collections.Generic;

namespace Gon
{
    internal static partial class Core
    {
        public static Segment<Scalar>[] ContourVerticesToSegments<Scalar>(Point<Scalar>[] vertices)
            where Scalar : IComparable<Scalar>, IEquatable<Scalar>
#if NET7_0_OR_GREATER
                ,
                System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
                System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
        {
            Segment<Scalar>[] result = ToEmptyArray<Segment<Scalar>>(vertices.Length);
            for (int index = 0; index < vertices.Length - 1; ++index)
            {
                result[index] = new Segment<Scalar>(vertices[index], vertices[index + 1]);
            }
            result[vertices.Length - 1] = new Segment<Scalar>(
                vertices[vertices.Length - 1],
                vertices[0]
            );
            return result;
        }

        public static Location LocatePointInRegion<Scalar>(
            Contour<Scalar> border,
            Point<Scalar> point
        )
            where Scalar : IComparable<Scalar>, IEquatable<Scalar>
#if NET7_0_OR_GREATER
                ,
                System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
                System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
        {
            var isInteriorPoint = false;
            var pointY = point.Y;
            foreach (var edge in border.Segments)
            {
                if (edge.Contains(point))
                {
                    return Location.Boundary;
                }
                var (end, start) = (edge.End, edge.Start);
                if (
                    (start.Y.CompareTo(pointY) > 0) != (end.Y.CompareTo(pointY) > 0)
                    && (
                        (end.Y.CompareTo(start.Y) > 0)
                        == (Orient(start, end, point) == Orientation.Counterclockwise)
                    )
                )
                {
                    isInteriorPoint = !isInteriorPoint;
                }
            }
            return isInteriorPoint ? Location.Interior : Location.Exterior;
        }

        public static Orientation Orient<Scalar>(
            Point<Scalar> vertex,
            Point<Scalar> firstRayPoint,
            Point<Scalar> secondRayPoint
        )
            where Scalar : IComparable<Scalar>, IEquatable<Scalar>
#if NET7_0_OR_GREATER
                ,
                System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
                System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
        {
            var comparisonResult = (
                (dynamic)CrossMultiply(vertex, firstRayPoint, vertex, secondRayPoint)
            ).CompareTo(0);
            return comparisonResult == 0
                ? Orientation.Collinear
                : (comparisonResult > 0 ? Orientation.Counterclockwise : Orientation.Clockwise);
        }

        public static string[] ToStrings<T>(T[] array)
        {
            var result = ToEmptyArray<string>(array.Length);
            for (int index = 0; index < array.Length; ++index)
            {
                result[index] = array[index].ToString();
            }
            return result;
        }

        private static Segment<Scalar>[] ContourVerticesToReversedSegments<Scalar>(
            Point<Scalar>[] vertices
        )
            where Scalar : IComparable<Scalar>, IEquatable<Scalar>
#if NET7_0_OR_GREATER
                ,
                System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
                System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
        {
            var result = ToEmptyArray<Segment<Scalar>>(vertices.Length);
            for (int index = 0; index < vertices.Length - 1; ++index)
            {
                result[index] = new Segment<Scalar>(vertices[index + 1], vertices[index]);
            }
            result[vertices.Length - 1] = new Segment<Scalar>(
                vertices[0],
                vertices[vertices.Length - 1]
            );
            return result;
        }

        private static Scalar CrossMultiply<Scalar>(
            Point<Scalar> firstStart,
            Point<Scalar> firstEnd,
            Point<Scalar> secondStart,
            Point<Scalar> secondEnd
        )
            where Scalar : IComparable<Scalar>, IEquatable<Scalar>
#if NET7_0_OR_GREATER
                ,
                System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
                System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
        {
#if NET7_0_OR_GREATER
            Point<Scalar> castFirstStart = firstStart;
            Point<Scalar> castFirstEnd = firstEnd;
            Point<Scalar> castSecondStart = secondStart;
            Point<Scalar> castSecondEnd = secondEnd;
#else
            dynamic castFirstStart = firstStart;
            dynamic castFirstEnd = firstEnd;
            dynamic castSecondStart = secondStart;
            dynamic castSecondEnd = secondEnd;
#endif
            return (castFirstEnd.X - castFirstStart.X) * (castSecondEnd.Y - castSecondStart.Y)
                - (castFirstEnd.Y - castFirstStart.Y) * (castSecondEnd.X - castSecondStart.X);
        }

        private static Segment<Scalar>[] PolygonToCorrectlyOrientedSegments<Scalar>(
            Polygon<Scalar> polygon
        )
            where Scalar : IComparable<Scalar>, IEquatable<Scalar>
#if NET7_0_OR_GREATER
                ,
                System.Numerics.IAdditionOperators<Scalar, Scalar, Scalar>,
                System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
                System.Numerics.IDivisionOperators<Scalar, Scalar, Scalar>,
                System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
        {
            int segmentsCount = polygon.Border.SegmentsCount;
            foreach (var hole in polygon.Holes)
            {
                segmentsCount += hole.SegmentsCount;
            }
            Segment<Scalar>[] result = ToEmptyArray<Segment<Scalar>>(segmentsCount);
            (
                polygon.Border.Orientation == Orientation.Counterclockwise
                    ? polygon.Border.Segments
                    : ContourVerticesToReversedSegments(polygon.Border.Vertices)
            ).CopyTo(result, 0);
            var offset = polygon.Border.SegmentsCount;
            foreach (var hole in polygon.Holes)
            {
                (
                    hole.Orientation == Orientation.Clockwise
                        ? hole.Segments
                        : ContourVerticesToReversedSegments(hole.Vertices)
                ).CopyTo(result, offset);
                offset += hole.SegmentsCount;
            }
            return result;
        }

        private static Segment<Scalar>[] PolygonsToCorrectlyOrientedSegments<Scalar>(
            Polygon<Scalar>[] polygons
        )
            where Scalar : IComparable<Scalar>, IEquatable<Scalar>
#if NET7_0_OR_GREATER
                ,
                System.Numerics.IAdditionOperators<Scalar, Scalar, Scalar>,
                System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
                System.Numerics.IDivisionOperators<Scalar, Scalar, Scalar>,
                System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
        {
            int segmentsCount = 0;
            foreach (var polygon in polygons)
            {
                segmentsCount += ToPolygonSegmentsCount(polygon);
            }
            var result = new List<Segment<Scalar>>(segmentsCount);
            foreach (var polygon in polygons)
            {
                result.AddRange(PolygonToCorrectlyOrientedSegments(polygon));
            }
            return result.ToArray();
        }

        private static int ToPolygonSegmentsCount<Scalar>(Polygon<Scalar> value)
            where Scalar : IComparable<Scalar>, IEquatable<Scalar>
#if NET7_0_OR_GREATER
                ,
                System.Numerics.IAdditionOperators<Scalar, Scalar, Scalar>,
                System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
                System.Numerics.IDivisionOperators<Scalar, Scalar, Scalar>,
                System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
        {
            int result = value.Border.SegmentsCount;
            foreach (var hole in value.Holes)
            {
                result += hole.SegmentsCount;
            }
            return result;
        }

        private static bool IsNotNull<T>(T value) =>
#if NETCOREAPP3_0_OR_GREATER
            value is not null;
#else
            value is object;
#endif

        private static T[] ToEmptyArray<T>(int capacity)
        {
#if NET5_0_OR_GREATER
            return GC.AllocateUninitializedArray<T>(capacity);
#else
            return new T[capacity];
#endif
        }

        private static void FillArray<T>(T[] array, T value)
        {
#if NET5_0_OR_GREATER
            Array.Fill(array, value);
#else
            for (int index = 0; index < array.Length; ++index)
            {
                array[index] = value;
            }
#endif
        }
    }
}
