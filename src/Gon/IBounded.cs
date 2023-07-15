using System;

namespace Gon
{
    internal interface IBounded<Scalar>
        where Scalar : IEquatable<Scalar>
    {
        Box<Scalar> BoundingBox { get; }
    }
}
