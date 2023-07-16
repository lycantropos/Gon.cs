using System;
using System.Collections.Generic;

namespace Gon
{
    public readonly struct Polygon<Scalar>
        : IBounded<Scalar>,
            IEquatable<Polygon<Scalar>>,
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
        public Polygon(Contour<Scalar> border, Contour<Scalar>[] holes)
        {
            Border = border;
            Holes = holes;
        }

        public Polygon(Contour<Scalar> border)
        {
            Border = border;
            Holes = new Contour<Scalar>[0];
        }

        public readonly Contour<Scalar> Border;
        public readonly Contour<Scalar>[] Holes;

        public Box<Scalar> BoundingBox => Border.BoundingBox;

        public static Polygon<Scalar>[] operator &(Polygon<Scalar> self, Polygon<Scalar> other) =>
            Core.Operation<Scalar>.Intersect(self, other);

        public static Polygon<Scalar>[] operator &(
            Polygon<Scalar> self,
            Multipolygon<Scalar> other
        ) => Core.Operation<Scalar>.Intersect(self, other);

        public static Polygon<Scalar>[] operator |(Polygon<Scalar> self, Polygon<Scalar> other) =>
            Core.Operation<Scalar>.Unite(self, other);

        public static Polygon<Scalar>[] operator |(
            Polygon<Scalar> self,
            Multipolygon<Scalar> other
        ) => Core.Operation<Scalar>.Unite(self, other);

        public static Polygon<Scalar>[] operator ^(Polygon<Scalar> self, Polygon<Scalar> other) =>
            Core.Operation<Scalar>.SymmetricSubtract(self, other);

        public static Polygon<Scalar>[] operator ^(
            Polygon<Scalar> self,
            Multipolygon<Scalar> other
        ) => Core.Operation<Scalar>.SymmetricSubtract(self, other);

        public static Polygon<Scalar>[] operator -(Polygon<Scalar> self, Polygon<Scalar> other) =>
            Core.Operation<Scalar>.Subtract(self, other);

        public static Polygon<Scalar>[] operator -(
            Polygon<Scalar> self,
            Multipolygon<Scalar> other
        ) => Core.Operation<Scalar>.Subtract(self, other);

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

        public override bool Equals(object other) =>
            other is Polygon<Scalar> otherPolygon && Equals(otherPolygon);

        public override int GetHashCode() =>
            Core.HashValues(Border, Core.Hashing.HashUnorderedUniqueIterable(Holes));

        public override string ToString() =>
            $"Polygon({Border}"
            + (
                Holes.Length > 0
                    ? (
                        ", {"
                        + string.Join(
                            ", ",
                            new List<Contour<Scalar>>(Holes).ConvertAll(hole => hole.ToString())
                        )
                        + "}"
                    )
                    : ""
            )
            + ")";

        Polygon<Scalar>[] Core.IShaped<Scalar>.ToPolygons() => new Polygon<Scalar>[1] { this };
    }
}
