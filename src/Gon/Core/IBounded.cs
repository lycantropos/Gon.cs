namespace Gon
{
    internal interface IBounded<Scalar>
    {
        public Box<Scalar> BoundingBox { get; }
    }
}
