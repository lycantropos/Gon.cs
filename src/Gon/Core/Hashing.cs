using System.Collections;

namespace Gon
{
    internal static partial class Core
    {
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
    }
}
