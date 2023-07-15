using System;
using System.Collections;

namespace Gon
{
    internal static partial class Core
    {
        public abstract class EventsEnumerator<Scalar> : IEnumerator
            where Scalar : IComparable<Scalar>,
                IComparable<int>,
                IEquatable<Scalar>
#if NET7_0_OR_GREATER
                ,
                System.Numerics.IAdditionOperators<Scalar, Scalar, Scalar>,
                System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
                System.Numerics.IDivisionOperators<Scalar, Scalar, Scalar>,
                System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
        {
            public EventsEnumerator(
                Segment<Scalar>[] firstSegments,
                Segment<Scalar>[] secondSegments
            )
            {
                _firstSegments = firstSegments;
                _secondSegments = secondSegments;
                _eventsQueue = new EventsQueue<Scalar>(_firstSegments, _secondSegments);
                _currentEndpoint = _eventsQueue.LeftmostPoint;
                _currentEndpointId = 0;
                _sweepLine = SweepLine<Scalar>.Create();
                _current = null;
            }

            object IEnumerator.Current => Current;

            public Event<Scalar> Current => _current!;

            public bool MoveNext()
            {
                while (!_eventsQueue.IsEmpty)
                {
                    var event_ = _eventsQueue.Pop();
                    if (event_.Start != _currentEndpoint)
                    {
                        _currentEndpoint = event_.Start;
                        _currentEndpointId += 1;
                    }
                    event_.StartId = _currentEndpointId;
                    if (event_.IsLeft)
                    {
                        LeftEvent<Scalar> leftEvent = (LeftEvent<Scalar>)(event_);
                        if (!_sweepLine.Contains(leftEvent))
                        {
                            _sweepLine.Add(leftEvent);
                            var (aboveEvent, belowEvent) = (
                                _sweepLine.Above(leftEvent),
                                _sweepLine.Below(leftEvent)
                            );
                            ComputeFields(leftEvent, belowEvent);
                            if (
                                aboveEvent is not null
                                && _eventsQueue.DetectIntersection(leftEvent, aboveEvent)
                            )
                            {
                                ComputeFields(leftEvent, belowEvent);
                                ComputeFields(aboveEvent, leftEvent);
                            }
                            if (
                                belowEvent is not null
                                && _eventsQueue.DetectIntersection(belowEvent, leftEvent)
                            )
                            {
                                var belowBelowEvent = _sweepLine.Below(belowEvent);
                                ComputeFields(belowEvent, belowBelowEvent);
                                ComputeFields(leftEvent, belowEvent);
                            }
                            _current = event_;
                            return true;
                        }
                    }
                    else
                    {
                        var oppositeEvent = (LeftEvent<Scalar>)(event_.Opposite);
                        if (_sweepLine.Contains(oppositeEvent))
                        {
                            var (aboveEvent, belowEvent) = (
                                _sweepLine.Above(oppositeEvent),
                                _sweepLine.Below(oppositeEvent)
                            );
                            _sweepLine.Remove(oppositeEvent);
                            if (aboveEvent is not null && belowEvent is not null)
                            {
                                _ = _eventsQueue.DetectIntersection(belowEvent, aboveEvent);
                            }
                        }
                        _current = event_;
                        return true;
                    }
                }
                return false;
            }

            public void Reset()
            {
                _current = null;
                _eventsQueue = new EventsQueue<Scalar>(_firstSegments, _secondSegments);
                _currentEndpoint = _eventsQueue.LeftmostPoint;
                _currentEndpointId = 0;
                _sweepLine = SweepLine<Scalar>.Create();
            }

            protected abstract bool FromResult(LeftEvent<Scalar> event_);

            private void ComputeFields(LeftEvent<Scalar> event_, LeftEvent<Scalar>? belowEvent)
            {
                if (belowEvent is not null)
                {
                    event_.OtherInteriorToLeft = (
                        event_.FromFirstOperand == belowEvent.FromFirstOperand
                            ? belowEvent.OtherInteriorToLeft
                            : belowEvent.InteriorToLeft
                    );
                    event_.BelowEventFromResult = (
                        (!FromResult(belowEvent) || belowEvent.IsVertical)
                            ? belowEvent.BelowEventFromResult
                            : belowEvent
                    );
                }
                event_.SetFromResult(FromResult(event_));
            }

            private Event<Scalar>? _current;
            private Point<Scalar> _currentEndpoint;
            private int _currentEndpointId;
            private EventsQueue<Scalar> _eventsQueue;
            private readonly Segment<Scalar>[] _firstSegments;
            private readonly Segment<Scalar>[] _secondSegments;
            private SweepLine<Scalar> _sweepLine;
        }
    }
}
