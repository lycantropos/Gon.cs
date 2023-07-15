#if NETCOREAPP2_1_OR_GREATER
using System;
#endif
using System.Collections;

namespace Gon
{
    internal static partial class Core
    {
        public static int HashValues<T1, T2>(T1 first, T2 second)
        {
#if NETCOREAPP2_1_OR_GREATER
            return HashCode.Combine((first, second));
#else
            int result = HashSeed;
            result += result * HashMultiplier + first.GetHashCode();
            result += result * HashMultiplier + second.GetHashCode();
            return result;
#endif
        }

        public static int HashValues<T1, T2, T3, T4>(T1 first, T2 second, T3 third, T4 fourth)
        {
#if NETCOREAPP2_1_OR_GREATER
            return HashCode.Combine((first, second, third, fourth));
#else
            int result = HashSeed;
            result += result * HashMultiplier + first.GetHashCode();
            result += result * HashMultiplier + second.GetHashCode();
            result += result * HashMultiplier + third.GetHashCode();
            result += result * HashMultiplier + fourth.GetHashCode();
            return result;
#endif
        }

        public static class Hashing
        {
            public static int HashUnorderedUniqueIterable<Iterable>(Iterable value)
                where Iterable : IEnumerable
            {
                var result = 0L;
                foreach (var element in value)
                {
                    result ^= ShuffleBits(element.GetHashCode());
                }
                return result.GetHashCode();
            }

            private static long ShuffleBits(int value)
            {
                var casted = (long)value;
                return ((casted ^ 89869747) ^ (casted << 16)) * 3644798167;
            }
        }

        private const int HashSeed = 23;
        private const int HashMultiplier = 31;
    }
}
