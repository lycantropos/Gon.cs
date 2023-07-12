using System;

namespace Gon
{
    public readonly struct Polygon<Scalar> : IBounded<Scalar>
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
        public Polygon(Contour<Scalar> border, Contour<Scalar>[] holes)
        {
            Border = border;
            Holes = holes;
        }

        public readonly Contour<Scalar> Border;
        public readonly Contour<Scalar>[] Holes;

        public Box<Scalar> BoundingBox
        {
            get { return Border.BoundingBox; }
        }

        public Segment<Scalar>[] Segments
        {
            get
            {
                int segmentsCount = Border.SegmentsCount;
                foreach (var hole in Holes)
                {
                    segmentsCount += hole.SegmentsCount;
                }
                Segment<Scalar>[] result = GC.AllocateUninitializedArray<Segment<Scalar>>(
                    segmentsCount
                );
                Border.Segments.CopyTo(result, 0);
                var offset = Border.SegmentsCount;
                foreach (var hole in Holes)
                {
                    hole.Segments.CopyTo(result, offset);
                    offset += hole.SegmentsCount;
                }
                return result;
            }
        }

        public static Polygon<Scalar>[] operator &(Polygon<Scalar> self, Polygon<Scalar> other)
        {
            return (new Operation<Scalar>(self, other)).Intersect();
        }
    }
}
