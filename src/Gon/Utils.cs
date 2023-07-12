using System;

namespace Gon
{
    static class Utils
    {
        public static Segment<Scalar>[] ContourVerticesToSegments<Scalar>(Point<Scalar>[] vertices)
            where Scalar : IComparable<Scalar>, IEquatable<Scalar>
        {
            Segment<Scalar>[] result = GC.AllocateUninitializedArray<Segment<Scalar>>(
                vertices.Length
            );
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

        public static Segment<Scalar>[] ContourVerticesToReversedSegments<Scalar>(
            Point<Scalar>[] vertices
        )
            where Scalar : IComparable<Scalar>, IEquatable<Scalar>
        {
            Segment<Scalar>[] result = GC.AllocateUninitializedArray<Segment<Scalar>>(
                vertices.Length
            );
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

        public static Segment<Scalar>[] PolygonToCorrectlyOrientedSegments<Scalar>(
            Polygon<Scalar> polygon
        )
            where Scalar : IComparable<Scalar>,
                IComparable<Int32>,
                IEquatable<Scalar>
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
            Segment<Scalar>[] result = GC.AllocateUninitializedArray<Segment<Scalar>>(
                segmentsCount
            );
            (
                polygon.Border.Orientation == Orientation.Counterclockwise
                    ? polygon.Border.Segments
                    : ContourVerticesToReversedSegments<Scalar>(polygon.Border.Vertices)
            ).CopyTo(result, 0);
            var offset = polygon.Border.SegmentsCount;
            foreach (var hole in polygon.Holes)
            {
                (
                    hole.Orientation == Orientation.Clockwise
                        ? hole.Segments
                        : ContourVerticesToReversedSegments<Scalar>(hole.Vertices)
                ).CopyTo(result, offset);
                offset += hole.SegmentsCount;
            }
            return result;
        }
    }
}
