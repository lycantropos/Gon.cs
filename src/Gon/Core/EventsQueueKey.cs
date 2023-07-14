using System;

namespace Gon
{
    internal readonly struct EventsQueueKey<Scalar> : IComparable<EventsQueueKey<Scalar>>
        where Scalar : IComparable<Scalar>,
            IComparable<Int32>,
            IEquatable<Scalar>
#if NET7_0_OR_GREATER
            ,
            System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
            System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
    {
        public EventsQueueKey(Event<Scalar> event_)
        {
            Event = event_;
        }

        public int CompareTo(EventsQueueKey<Scalar> other)
        {
            var (start, otherStart) = (Event.Start, other.Event.Start);
            if (start.X.CompareTo(otherStart.X) != 0)
            {
                return start.X.CompareTo(otherStart.X);
            }
            else if (start.Y.CompareTo(otherStart.Y) != 0)
            {
                return start.Y.CompareTo(otherStart.Y);
            }
            else if (Event.IsLeft != other.Event.IsLeft)
            {
                return !Event.IsLeft ? -1 : 1;
            }
            else
            {
                Orientation otherEndOrientation = Orienteer<Scalar>.Orient(
                    start,
                    Event.End,
                    other.Event.End
                );
                if (otherEndOrientation == Orientation.Collinear)
                {
                    return other.Event.FromFirstOperand ? -1 : 1;
                }
                else
                {
                    return (
                        otherEndOrientation
                        == (Event.IsLeft ? Orientation.Counterclockwise : Orientation.Clockwise)
                    )
                        ? -1
                        : 1;
                }
            }
        }

        private readonly Event<Scalar> Event;
    }
}
