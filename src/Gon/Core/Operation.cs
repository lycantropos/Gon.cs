using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Gon
{
    internal static partial class Core
    {
        public static class Operation<Scalar>
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
            public static Polygon<Scalar>[] Intersect<First, Second>(First first, Second second)
                where First : IBounded<Scalar>, IShaped<Scalar>
                where Second : IBounded<Scalar>, IShaped<Scalar>
            {
                var firstBoundingBox = first.BoundingBox;
                var secondBoundingBox = second.BoundingBox;
                if (
                    firstBoundingBox.DisjointWith(secondBoundingBox)
                    || firstBoundingBox.Touches(secondBoundingBox)
                )
                {
                    return new Polygon<Scalar>[0];
                }
                var firstSegments = PolygonsToCorrectlyOrientedSegments(first.ToPolygons());
                var secondSegments = PolygonsToCorrectlyOrientedSegments(second.ToPolygons());
                var events = new List<Event<Scalar>>(firstSegments.Length + secondSegments.Length);
                var eventsEnumerator = new IntersectionEventsNumerator<Scalar>(
                    firstSegments,
                    secondSegments
                );
                var firstMaxX = firstBoundingBox.MaxX;
                var secondMaxX = secondBoundingBox.MaxX;
                var minMaxX = firstMaxX.CompareTo(secondMaxX) < 0 ? firstMaxX : secondMaxX;
                while (eventsEnumerator.MoveNext())
                {
                    var event_ = eventsEnumerator.Current;
                    if (event_.Start.X.CompareTo(minMaxX) > 0)
                    {
                        break;
                    }
                    events.Add(event_);
                }
                return ProcessEvents(events);
            }

            private static Point<Scalar>[] ContourEventsToVertices(Event<Scalar>[] events)
            {
                var result = new List<Point<Scalar>>(events.Length) { events[0].Start };
                for (int index = 0; index < events.Length - 1; ++index)
                {
                    result.Add(events[index].End);
                }
                ShrinkCollinearVertices(result);
                return result.ToArray();
            }

            private static void ProcessContourEvents(
                Event<Scalar>[] contourEvents,
                int contourId,
                bool[] areEventsProcessed
            )
            {
                foreach (var event_ in contourEvents)
                {
                    areEventsProcessed[event_.Id] = true;
                    areEventsProcessed[event_.Opposite.Id] = true;
                    if (event_.IsLeft)
                    {
                        var leftEvent = (LeftEvent<Scalar>)event_;
                        leftEvent.FromInToOut = false;
                        leftEvent.ContourId = contourId;
                    }
                    else
                    {
                        var leftEvent = (LeftEvent<Scalar>)event_.Opposite;
                        leftEvent.FromInToOut = true;
                        leftEvent.ContourId = contourId;
                    }
                }
            }

            private static void ShrinkCollinearVertices(List<Point<Scalar>> vertices)
            {
                var index = -vertices.Count + 1;
                while (index < 0)
                {
                    while (
                        Math.Max(2, -index) < vertices.Count
                        && Orient(
                            vertices[(vertices.Count + index + 1) % vertices.Count],
                            vertices[(vertices.Count + index + 2) % vertices.Count],
                            vertices[vertices.Count + index]
                        ) == Orientation.Collinear
                    )
                    {
                        vertices.RemoveAt((vertices.Count + index + 1) % vertices.Count);
                    }
                    index += 1;
                }
                while (index < vertices.Count)
                {
                    while (
                        Math.Max(2, index) < vertices.Count
                        && Orient(
                            vertices[(vertices.Count + index - 1) % vertices.Count],
                            vertices[(vertices.Count + index - 2) % vertices.Count],
                            vertices[index]
                        ) == Orientation.Collinear
                    )
                    {
                        vertices.RemoveAt(index - 1);
                    }
                    index += 1;
                }
            }

            private static Polygon<Scalar>[] ProcessEvents(List<Event<Scalar>> events)
            {
                events = events.FindAll(event_ => event_.FromResult);
                if (events.Count == 0)
                {
                    return new Polygon<Scalar>[0];
                }
                var maxEndpointId = events[events.Count - 1].StartId;
                Debug.Assert(maxEndpointId != UndefinedIndex);
                Debug.Assert(events.TrueForAll(event_ => event_.StartId <= maxEndpointId));
                events.Sort(
                    (event_, otherEvent) =>
                        (new EventsQueueKey<Scalar>(event_)).CompareTo(
                            new EventsQueueKey<Scalar>(otherEvent)
                        )
                );
                var eventId = -1;
                foreach (var event_ in events)
                {
                    event_.Id = ++eventId;
                }
                var areInternal = new List<bool>();
                var depths = new List<int>();
                var holesIds = new List<List<int>>();
                var parents = new List<int>();
                var areEventsProcessed = new bool[events.Count];
                var contours = new List<Contour<Scalar>>();
                var connectivity = EventsToConnectivity(events);
                eventId = -1;
                var visitedEndpointsPositions = ToEmptyArray<int>(maxEndpointId + 1);
                FillArray(visitedEndpointsPositions, UndefinedIndex);
                foreach (var event_ in events)
                {
                    ++eventId;
                    if (areEventsProcessed[eventId])
                    {
                        continue;
                    }
                    var leftEvent = (LeftEvent<Scalar>)event_;
                    var contourId = contours.Count;
                    ComputeRelations(leftEvent, contourId, areInternal, depths, holesIds, parents);
                    var contourEvents = ToContourEvents(
                        leftEvent,
                        events,
                        connectivity,
                        areEventsProcessed,
                        visitedEndpointsPositions
                    );
                    ProcessContourEvents(contourEvents, contourId, areEventsProcessed);
                    var vertices = ContourEventsToVertices(contourEvents);
                    if (depths[contourId] % 2 != 0)
                    {
                        // holesIds will be in clockwise order
                        Array.Reverse(vertices, 1, vertices.Length - 1);
                    }
                    contours.Add(new Contour<Scalar>(vertices));
                }
                return ToPolygons(contours, areInternal, holesIds);
            }

            private static Polygon<Scalar>[] ToPolygons(
                List<Contour<Scalar>> contours,
                List<bool> areInternal,
                List<List<int>> holesIds
            )
            {
                var result = new List<Polygon<Scalar>>();
                var contourId = -1;
                foreach (var contour in contours)
                {
                    ++contourId;
                    if (areInternal[contourId])
                    {
                        foreach (var holeId in holesIds[contourId])
                        {
                            // hole of a hole is an external polygon
                            result.Add(
                                new Polygon<Scalar>(
                                    contours[holeId],
                                    CollectContours(holesIds[holeId], contours)
                                )
                            );
                        }
                    }
                    else
                    {
                        result.Add(
                            new Polygon<Scalar>(
                                contour,
                                CollectContours(holesIds[contourId], contours)
                            )
                        );
                    }
                }
                return result.ToArray();
            }

            private static Contour<Scalar>[] CollectContours(
                List<int> contoursIds,
                List<Contour<Scalar>> contours
            )
            {
                var result = ToEmptyArray<Contour<Scalar>>(contoursIds.Count);
                var contourIndex = -1;
                foreach (var contourId in contoursIds)
                {
                    ++contourIndex;
                    result[contourIndex] = contours[contourId];
                }
                return result;
            }

            private static Event<Scalar>[] ToContourEvents(
                LeftEvent<Scalar> event_,
                List<Event<Scalar>> events,
                int[] connectivity,
                bool[] areEventsProcessed,
                int[] visitedEndpointsPositions
            )
            {
                Debug.Assert(event_.IsLeft);
                var result = new List<Event<Scalar>> { event_ };
                visitedEndpointsPositions[event_.StartId] = 0;
                var oppositeEventId = event_.Opposite.Id;
                var contourStart = event_.Start;
                Event<Scalar> cursor = event_;
                var visitedEndpointsIds = new List<int> { event_.StartId };
                while (cursor.End != contourStart)
                {
                    var previousEndpointPosition = visitedEndpointsPositions[cursor.EndId];
                    if (previousEndpointPosition == UndefinedIndex)
                    {
                        visitedEndpointsPositions[cursor.EndId] = result.Count;
                    }
                    else
                    {
                        // vertices loop found, i.e. contour has self-intersection
                        Debug.Assert(previousEndpointPosition != 0);
                        result.RemoveRange(
                            previousEndpointPosition,
                            result.Count - previousEndpointPosition
                        );
                    }
                    visitedEndpointsIds.Add(cursor.EndId);
                    var eventId = ToNextEventId(oppositeEventId, areEventsProcessed, connectivity);
                    if (eventId == UndefinedIndex)
                    {
                        break;
                    }
                    cursor = events[eventId];
                    oppositeEventId = cursor.Opposite.Id;
                    result.Add(cursor);
                }
                foreach (var eventId in visitedEndpointsIds)
                {
                    visitedEndpointsPositions[eventId] = UndefinedIndex;
                }
                Debug.Assert(
                    (new List<int>(visitedEndpointsPositions)).TrueForAll(
                        position => position == UndefinedIndex
                    )
                );
                return result.ToArray();
            }

            private static int ToNextEventId(
                int eventId,
                bool[] areEventsProcessed,
                int[] connectivity
            )
            {
                var candidate = eventId;
                while (true)
                {
                    candidate = connectivity[candidate];
                    if (!areEventsProcessed[candidate])
                    {
                        return candidate;
                    }
                    else if (candidate == eventId)
                    {
                        return UndefinedIndex;
                    }
                }
            }

            private static void ComputeRelations(
                LeftEvent<Scalar> event_,
                int contourId,
                List<bool> areInternal,
                List<int> depths,
                List<List<int>> holesIds,
                List<int> parents
            )
            {
                var depth = 0;
                int parent = UndefinedIndex;
                var isInternal = false;
                var belowEventFromResult = event_.BelowEventFromResult;
                if (IsNotNull(belowEventFromResult))
                {
                    var belowContourId = belowEventFromResult.ContourId;
                    if (!belowEventFromResult.FromInToOut)
                    {
                        if (!areInternal[belowContourId])
                        {
                            holesIds[belowContourId].Add(contourId);
                            parent = belowContourId;
                            depth = depths[belowContourId] + 1;
                            isInternal = true;
                        }
                    }
                    else if (areInternal[belowContourId])
                    {
                        var belowInResultParentId = parents[belowContourId];
                        holesIds[belowInResultParentId].Add(contourId);
                        parent = belowInResultParentId;
                        depth = depths[belowContourId];
                        isInternal = true;
                    }
                }
                holesIds.Add(new List<int>());
                parents.Add(parent);
                depths.Add(depth);
                areInternal.Add(isInternal);
            }

            private static int[] EventsToConnectivity(List<Event<Scalar>> events)
            {
                var eventsCount = events.Count;
                var result = new int[eventsCount];
                var index = 0;
                while (index < eventsCount)
                {
                    var currentStart = events[index].Start;
                    var rightStartIndex = index;
                    while (
                        index < eventsCount
                        && events[index].Start == currentStart
                        && !events[index].IsLeft
                    )
                    {
                        index += 1;
                    }
                    var leftStartIndex = index;
                    while (index < eventsCount && events[index].Start == currentStart)
                    {
                        index += 1;
                    }
                    var leftStopIndex = index - 1;
                    var hasRightEvents = leftStartIndex >= rightStartIndex + 1;
                    var hasLeftEvents = leftStopIndex >= leftStartIndex;
                    if (hasRightEvents)
                    {
                        for (
                            int subIndex = rightStartIndex;
                            subIndex < leftStartIndex - 1;
                            ++subIndex
                        )
                        {
                            result[subIndex] = subIndex + 1;
                        }
                        result[leftStartIndex - 1] = (
                            hasLeftEvents ? leftStopIndex : rightStartIndex
                        );
                    }
                    if (hasLeftEvents)
                    {
                        result[leftStartIndex] = (hasRightEvents ? rightStartIndex : leftStopIndex);
                        for (
                            int subIndex = leftStartIndex + 1;
                            subIndex <= leftStopIndex;
                            ++subIndex
                        )
                        {
                            result[subIndex] = subIndex - 1;
                        }
                    }
                }
                return result;
            }
        }
    }
}
