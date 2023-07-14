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

        public Box<Scalar> BoundingBox => Border.BoundingBox;

        public static Polygon<Scalar>[] operator &(Polygon<Scalar> self, Polygon<Scalar> other)
        {
            return (new Operation<Scalar>(self, other)).Intersect();
        }

        public static Polygon<Scalar>[] operator &(Polygon<Scalar> self, Multipolygon<Scalar> other)
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

        public override bool Equals(object? other)
        {
            if (other is Polygon<Scalar> otherPolygon)
            {
                return Equals(otherPolygon);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return (Border, Hashing.HashUnorderedUniqueIterable(Holes)).GetHashCode();
        }
    }
}
