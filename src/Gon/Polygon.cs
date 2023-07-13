using System;
using System.Collections.Generic;

namespace Gon
{
    public readonly struct Polygon<Scalar> : IBounded<Scalar>, IEquatable<Polygon<Scalar>>
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

        public static bool operator ==(Polygon<Scalar> self, Polygon<Scalar> other) =>
            self.Equals(other);

        public static bool operator !=(Polygon<Scalar> self, Polygon<Scalar> other) =>
            !self.Equals(other);

        public bool Equals(Polygon<Scalar> other)
        {
            if (Border != other.Border || Holes.Length != other.Holes.Length)
            {
                return false;
            }
            var holesSet = new HashSet<Contour<Scalar>>(Holes);
            foreach (var otherHole in other.Holes)
            {
                if (!holesSet.Contains(otherHole))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object? other) =>
            (other is Polygon<Scalar>) && Equals((Polygon<Scalar>)other);

        public override int GetHashCode()
        {
            var holesHash = (Int64)0;
            foreach (var hole in Holes)
            {
                holesHash ^= shuffleBits(hole.GetHashCode());
            }
            return (Border, holesHash).GetHashCode();
        }

        private static Int64 shuffleBits(int value)
        {
            var casted = (Int64)value;
            return ((casted ^ 89869747) ^ (casted << 16)) * 3644798167;
        }
    }
}
