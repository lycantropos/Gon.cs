using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Gon
{
    internal static partial class Core
    {
        public readonly struct SweepLine<Scalar>
            where Scalar : IComparable<Scalar>,
                IComparable<int>,
                IEquatable<Scalar>
#if NET7_0_OR_GREATER
                ,
                System.Numerics.IMultiplyOperators<Scalar, Scalar, Scalar>,
                System.Numerics.ISubtractionOperators<Scalar, Scalar, Scalar>
#endif
        {
            public static SweepLine<Scalar> Create()
            {
                return new SweepLine<Scalar>(
                    new SortedList<SweepLineKey<Scalar>, LeftEvent<Scalar>>()
                );
            }

            public LeftEvent<Scalar>? Above(LeftEvent<Scalar> event_)
            {
                Debug.Assert(Contains(event_));
                int index = _values.IndexOfValue(event_);
                if (0 <= index && index < _values.Count - 1)
                {
                    var nextKey = _values.Keys[index + 1];
                    Debug.Assert(ToKey(event_).CompareTo(nextKey) < 0);
                    return _values[nextKey];
                }
                else
                {
                    return null;
                }
            }

            public void Add(LeftEvent<Scalar> event_)
            {
                Debug.Assert(!Contains(event_));
                _values.Add(ToKey(event_), event_);
            }

            public LeftEvent<Scalar>? Below(LeftEvent<Scalar> event_)
            {
                Debug.Assert(Contains(event_));
                int index = _values.IndexOfValue(event_);
                if (0 < index && index < _values.Count)
                {
                    var prevKey = _values.Keys[index - 1];
                    Debug.Assert(ToKey(event_).CompareTo(prevKey) > 0);
                    return _values[prevKey];
                }
                else
                {
                    return null;
                }
            }

            public bool Contains(LeftEvent<Scalar> event_)
            {
                return _values.ContainsKey(ToKey(event_));
            }

            public void Remove(LeftEvent<Scalar> event_)
            {
                bool removed = _values.Remove(ToKey(event_));
                Debug.Assert(removed);
            }

            private SweepLine(SortedList<SweepLineKey<Scalar>, LeftEvent<Scalar>> values)
            {
                _values = values;
            }

            private static SweepLineKey<Scalar> ToKey(LeftEvent<Scalar> event_)
            {
                return new SweepLineKey<Scalar>(event_);
            }

            private readonly SortedList<SweepLineKey<Scalar>, LeftEvent<Scalar>> _values;
        }
    }
}
