using System.Collections.Generic;
using System.Linq;

namespace Papper.Internal
{
    internal static class Extensions
    {
        internal static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source) => source == null || !source.Any();
    }
}
