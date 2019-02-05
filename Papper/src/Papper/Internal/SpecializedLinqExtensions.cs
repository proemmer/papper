using System.Collections.Generic;

namespace Papper.Internal
{
    internal static class SpecializedLinqExtensions
    {
        public static bool Any<TSource>(this IList<TSource> source)
        {
            if (source == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(source));
            return source.Count > 0;
        }

        public static bool Any<TSourceKey, TSourceValue>(this IDictionary<TSourceKey, TSourceValue> source)
        {
            if (source == null) ExceptionThrowHelper.ThrowArgumentNullException(nameof(source));
            return source.Count > 0;
        }
    }
}
