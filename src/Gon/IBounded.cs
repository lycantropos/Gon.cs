using System;

namespace Gon
{
    internal interface IBounded<Scalar>
        where Scalar : IComparable<Scalar>, IEquatable<Scalar>
    {
        Box<Scalar> BoundingBox { get; }
    }
}
