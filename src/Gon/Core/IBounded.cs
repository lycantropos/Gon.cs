using System;

namespace Gon
{
    internal interface IBounded<Scalar>
        where Scalar : IEquatable<Scalar>
    {
        public Box<Scalar> BoundingBox { get; }
    }
}
