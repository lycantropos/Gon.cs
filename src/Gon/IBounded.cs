using System;

namespace Gon
{
    internal interface IBounded<Scalar>
        where Scalar : IEquatable<Scalar>
    {
#if NETCOREAPP3_0_OR_GREATER
        public
#endif
        Box<Scalar> BoundingBox { get; }
    }
}
