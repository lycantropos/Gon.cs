namespace Gon
{
    public readonly struct Box<Scalar>
    {
        public Box(Scalar minX, Scalar maxX, Scalar minY, Scalar maxY)
        {
            MaxX = maxX;
            MaxY = maxY;
            MinX = minX;
            MinY = minY;
        }

        public readonly Scalar MaxX;
        public readonly Scalar MaxY;
        public readonly Scalar MinX;
        public readonly Scalar MinY;
    }
}
