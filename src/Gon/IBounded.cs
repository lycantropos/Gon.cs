namespace Gon
{
    interface IBounded<Scalar>
    {
        public Box<Scalar> BoundingBox { get; }
    }
}
