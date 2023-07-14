using System;

namespace Gon
{
    internal readonly struct SweepLineKey<Scalar> : IComparable<SweepLineKey<Scalar>>
        where Scalar : IComparable<Scalar>,
            IComparable<int>,
            IEquatable<Scalar>
#if NET7_0_OR_GREATER
            ,
            System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
            System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
    {
        public readonly LeftEvent<Scalar> Event;

        public SweepLineKey(LeftEvent<Scalar> event_)
        {
            Event = event_;
        }

        public int CompareTo(SweepLineKey<Scalar> other)
        {
            if (ReferenceEquals(Event, other.Event))
            {
                return 0;
            }
            var (start, otherStart) = (Event.Start, other.Event.Start);
            var (end, otherEnd) = (Event.End, other.Event.End);
            var otherStartOrientation = Orienteer<Scalar>.Orient(start, end, otherStart);
            var otherEndOrientation = Orienteer<Scalar>.Orient(start, end, otherEnd);
            if (otherStartOrientation == otherEndOrientation)
            {
                if (otherStartOrientation != Orientation.Collinear)
                {
                    // other segment fully lies on one side
                    return otherStartOrientation == Orientation.Counterclockwise ? -1 : 1;
                }
                // segments are collinear
                else if (Event.FromFirstOperand != other.Event.FromFirstOperand)
                {
                    return Event.FromFirstOperand ? -1 : 1;
                }
                else if (start.X.CompareTo(otherStart.X) == 0)
                {
                    if (start.Y.CompareTo(otherStart.Y) != 0)
                    {
                        // segments are vertical
                        return start.Y.CompareTo(otherStart.Y);
                    }
                    // segments have same start
                    else if (end.Y.CompareTo(otherEnd.Y) != 0)
                    {
                        return end.Y.CompareTo(otherEnd.Y);
                    }
                    else
                    {
                        // segments are horizontal
                        return end.X.CompareTo(otherEnd.X);
                    }
                }
                else if (start.Y.CompareTo(otherStart.Y) != 0)
                {
                    return start.Y.CompareTo(otherStart.Y);
                }
                else
                {
                    // segments are horizontal
                    return start.X.CompareTo(otherStart.X);
                }
            }
            var startOrientation = Orienteer<Scalar>.Orient(otherStart, otherEnd, start);
            var endOrientation = Orienteer<Scalar>.Orient(otherStart, otherEnd, end);
            if (startOrientation == endOrientation)
            {
                return startOrientation == Orientation.Clockwise ? -1 : 1;
            }
            else if (otherStartOrientation == Orientation.Collinear)
            {
                return otherEndOrientation == Orientation.Counterclockwise ? -1 : 1;
            }
            else if (startOrientation == Orientation.Collinear)
            {
                return endOrientation == Orientation.Clockwise ? -1 : 1;
            }
            else if (endOrientation == Orientation.Collinear)
            {
                return startOrientation == Orientation.Clockwise ? -1 : 1;
            }
            else
            {
                return otherStartOrientation == Orientation.Counterclockwise ? -1 : 1;
            }
        }
    }
}
