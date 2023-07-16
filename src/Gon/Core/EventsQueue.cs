using System;
#if NET6_0_OR_GREATER
using System.Collections.Generic;
#else
using Priority_Queue;
#endif
using System.Diagnostics;

namespace Gon
{
    internal static partial class Core
    {
        public readonly struct EventsQueue<Scalar>
            where Scalar : IComparable<Scalar>,
                IEquatable<Scalar>
#if NET7_0_OR_GREATER
                ,
                System.Numerics.IAdditionOperators<Scalar, Scalar, Scalar>,
                System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
                System.Numerics.IDivisionOperators<Scalar, Scalar, Scalar>,
                System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
        {
            public int Count => _values.Count;

            public bool IsEmpty => _values.Count == 0;

            public Point<Scalar> LeftmostPoint => _values.
#if NET6_0_OR_GREATER
                Peek()
#else
                First
#endif
                .Start;

            public EventsQueue(Segment<Scalar>[] firstSegments, Segment<Scalar>[] secondSegments)
            {
                _values = new
#if NET6_0_OR_GREATER
                    PriorityQueue
#else
                    SimplePriorityQueue
#endif
                    <Event<Scalar>, EventsQueueKey<Scalar>>();
                Extend(firstSegments, true);
                Extend(secondSegments, false);
            }

            public bool DetectIntersection(LeftEvent<Scalar> belowEvent, LeftEvent<Scalar> event_)
            {
                var eventStart = event_.Start;
                var eventEnd = event_.End;
                var belowEventStart = belowEvent.Start;
                var belowEventEnd = belowEvent.End;
                var eventStartOrientation = Orient(belowEventStart, belowEventEnd, eventStart);
                var eventEndOrientation = Orient(belowEventStart, belowEventEnd, eventEnd);
                if (
                    eventStartOrientation != Orientation.Collinear
                    && eventEndOrientation != Orientation.Collinear
                )
                {
                    if (eventStartOrientation != eventEndOrientation)
                    {
                        var belowEventStartOrientation = Orient(
                            eventStart,
                            eventEnd,
                            belowEventStart
                        );
                        var belowEventEndOrientation = Orient(eventStart, eventEnd, belowEventEnd);
                        if (
                            belowEventStartOrientation != Orientation.Collinear
                            && belowEventEndOrientation != Orientation.Collinear
                        )
                        {
                            if (belowEventStartOrientation != belowEventEndOrientation)
                            {
                                var point = ToSegmentsIntersectionPoint(
                                    eventStart,
                                    eventEnd,
                                    belowEventStart,
                                    belowEventEnd
                                );
                                Debug.Assert(eventStart < point && point < eventEnd);
                                Debug.Assert(belowEventStart < point && point < belowEventEnd);
                                DivideEventByMidpoint(belowEvent, point);
                                DivideEventByMidpoint(event_, point);
                            }
                        }
                        else if (belowEventStartOrientation != Orientation.Collinear)
                        {
                            if (eventStart < belowEventEnd && belowEventEnd < eventEnd)
                            {
                                var point = belowEventEnd;
                                DivideEventByMidpoint(event_, point);
                            }
                        }
                        else if (eventStart < belowEventStart && belowEventStart < eventEnd)
                        {
                            var point = belowEventStart;
                            DivideEventByMidpoint(event_, point);
                        }
                    }
                }
                else if (eventEndOrientation != Orientation.Collinear)
                {
                    if (belowEventStart < eventStart && eventStart < belowEventEnd)
                    {
                        var point = eventStart;
                        DivideEventByMidpoint(belowEvent, point);
                    }
                }
                else if (eventStartOrientation != Orientation.Collinear)
                {
                    if (belowEventStart < eventEnd && eventEnd < belowEventEnd)
                    {
                        var point = eventEnd;
                        DivideEventByMidpoint(belowEvent, point);
                    }
                }
                else
                {
                    Debug.Assert(belowEvent.FromFirstOperand != event_.FromFirstOperand);
                    if (eventStart == belowEventStart)
                    {
                        if (eventEnd != belowEventEnd)
                        {
                            var (maxEndEvent, minEndEvent) = (
                                eventEnd < belowEventEnd
                                    ? (belowEvent, event_)
                                    : (event_, belowEvent)
                            );
                            var (minEndStartEvent, minEndMaxEndEvent) = DivideEvent(
                                maxEndEvent,
                                minEndEvent.End
                            );
                            Push(minEndStartEvent);
                            Push(minEndMaxEndEvent);
                        }
                        belowEvent.OverlapKind = event_.OverlapKind = (
                            event_.InteriorToLeft == belowEvent.InteriorToLeft
                                ? OverlapKind.SameOrientation
                                : OverlapKind.DifferentOrientation
                        );
                        return true;
                    }
                    else if (eventEnd == belowEventEnd)
                    {
                        var (maxStartEvent, minStartEvent) = (
                            eventStart < belowEventStart
                                ? (belowEvent, event_)
                                : (event_, belowEvent)
                        );
                        var (maxStartToMinStartEvent, maxStartToEndEvent) = DivideEvent(
                            minStartEvent,
                            maxStartEvent.Start
                        );
                        Push(maxStartToMinStartEvent);
                        Push(maxStartToEndEvent);
                    }
                    else if (belowEventStart < eventStart && eventStart < belowEventEnd)
                    {
                        if (eventEnd < belowEventEnd)
                        {
                            DivideEventByMidSegmentEventEndpoints(belowEvent, eventStart, eventEnd);
                        }
                        else
                        {
                            var (maxStart, minEnd) = (eventStart, belowEventEnd);
                            DivideOverlappingEvents(belowEvent, event_, maxStart, minEnd);
                        }
                    }
                    else if (eventStart < belowEventStart && belowEventStart < eventEnd)
                    {
                        if (belowEventEnd < eventEnd)
                        {
                            DivideEventByMidSegmentEventEndpoints(
                                event_,
                                belowEventStart,
                                belowEventEnd
                            );
                        }
                        else
                        {
                            var (maxStart, minEnd) = (belowEventStart, eventEnd);
                            DivideOverlappingEvents(event_, belowEvent, maxStart, minEnd);
                        }
                        ;
                    }
                }
                return false;
            }

            public Event<Scalar> Pop()
            {
                return _values.Dequeue();
            }

            private static (RightEvent<Scalar>, LeftEvent<Scalar>) DivideEvent(
                LeftEvent<Scalar> event_,
                Point<Scalar> midPoint
            )
            {
                var midPointToEventEndEvent = new LeftEvent<Scalar>(
                    event_.FromFirstOperand,
                    midPoint,
                    (RightEvent<Scalar>)event_.Opposite,
                    event_.InteriorToLeft
                );
                event_.Opposite.Opposite = midPointToEventEndEvent;
                var midPointToEventStartEvent = new RightEvent<Scalar>(midPoint, event_);
                event_.Opposite = midPointToEventStartEvent;
                return (midPointToEventStartEvent, midPointToEventEndEvent);
            }

            private void DivideEventByMidSegmentEventEndpoints(
                LeftEvent<Scalar> event_,
                Point<Scalar> midSegmentEventStart,
                Point<Scalar> midSegmentEventEnd
            )
            {
                DivideEventByMidpoint(event_, midSegmentEventEnd);
                DivideEventByMidpoint(event_, midSegmentEventStart);
            }

            private void DivideEventByMidpoint(LeftEvent<Scalar> event_, Point<Scalar> point)
            {
                var (pointToEventStartEvent, pointToEventEndEvent) = DivideEvent(event_, point);
                Push(pointToEventStartEvent);
                Push(pointToEventEndEvent);
            }

            private void DivideOverlappingEvents(
                LeftEvent<Scalar> minStartEvent,
                LeftEvent<Scalar> maxStartEvent,
                Point<Scalar> maxStart,
                Point<Scalar> minEnd
            )
            {
                DivideEventByMidpoint(maxStartEvent, minEnd);
                DivideEventByMidpoint(minStartEvent, maxStart);
            }

            private static Point<Scalar> ToSegmentsIntersectionPoint(
                Point<Scalar> firstStart,
                Point<Scalar> firstEnd,
                Point<Scalar> secondStart,
                Point<Scalar> secondEnd
            )
            {
                var scale = ToSegmentsIntersectionScale(
                    firstStart,
                    firstEnd,
                    secondStart,
                    secondEnd
                );
#if NET7_0_OR_GREATER
                Scalar firstStartX = firstStart.X;
                Scalar firstEndX = firstEnd.X;
                Scalar firstStartY = firstStart.Y;
                Scalar firstEndY = firstEnd.Y;
#else
                dynamic firstStartX = firstStart.X;
                dynamic firstEndX = firstEnd.X;
                dynamic firstStartY = firstStart.Y;
                dynamic firstEndY = firstEnd.Y;
#endif
                return new Point<Scalar>(
                    firstStartX + (firstEndX - firstStartX) * scale,
                    firstStartY + (firstEndY - firstStartY) * scale
                );
            }

            private static Scalar ToSegmentsIntersectionScale(
                Point<Scalar> firstStart,
                Point<Scalar> firstEnd,
                Point<Scalar> secondStart,
                Point<Scalar> secondEnd
            )
            {
#if NET7_0_OR_GREATER
                Scalar
#else
                dynamic
#endif
                dividend = CrossMultiply(firstStart, secondStart, secondStart, secondEnd);
#if NET7_0_OR_GREATER
                Scalar
#else
                dynamic
#endif
                divisor = CrossMultiply(firstStart, firstEnd, secondStart, secondEnd);
                return dividend / divisor;
            }

            private readonly
#if NET6_0_OR_GREATER
            PriorityQueue
#else
            SimplePriorityQueue
#endif
            <Event<Scalar>, EventsQueueKey<Scalar>> _values;

            private void Extend(Segment<Scalar>[] segments, bool isFromFirstOperand)
            {
                foreach (Segment<Scalar> segment in segments)
                {
                    var leftEvent = LeftEvent<Scalar>.FromEndpoints(
                        isFromFirstOperand,
                        segment.Start,
                        segment.End
                    );
                    Push(leftEvent);
                    Push(leftEvent.Opposite);
                }
            }

            private void Push(Event<Scalar> event_)
            {
                _values.Enqueue(event_, new EventsQueueKey<Scalar>(event_));
            }
        }
    }
}
