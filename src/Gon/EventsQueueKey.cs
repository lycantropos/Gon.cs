using System;

namespace Gon
{
    public readonly struct EventsQueueKey<Scalar> : IComparable<EventsQueueKey<Scalar>>
        where Scalar : IComparable<Scalar>,
            IComparable<Int32>,
            IEquatable<Scalar>
#if NET7_0
            ,
            System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
            System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
    {
        public EventsQueueKey(Event<Scalar> value)
        {
            _value = value;
        }

        public int CompareTo(EventsQueueKey<Scalar> other)
        {
            var (start, otherStart) = (_value.Start, other._value.Start);
            if (start.X.CompareTo(otherStart.X) != 0)
            {
                return start.X.CompareTo(otherStart.X);
            }
            else if (start.Y.CompareTo(otherStart.Y) != 0)
            {
                return start.Y.CompareTo(otherStart.Y);
            }
            else if (_value.IsLeft != other._value.IsLeft)
            {
                return _value.IsLeft ? 1 : -1;
            }
            else
            {
                Orientation otherEndOrientation = Orienteer<Scalar>.Orient(
                    start,
                    _value.End,
                    other._value.End
                );
                if (otherEndOrientation == Orientation.Collinear)
                {
                    return other._value.IsFromFirstOperand ? -1 : 1;
                }
                else
                {
                    return
                        otherEndOrientation
                        == (_value.IsLeft ? Orientation.Counterclockwise : Orientation.Clockwise)
                        ? -1
                        : 1;
                }
            }
        }

        private readonly Event<Scalar> _value;
    }
}
