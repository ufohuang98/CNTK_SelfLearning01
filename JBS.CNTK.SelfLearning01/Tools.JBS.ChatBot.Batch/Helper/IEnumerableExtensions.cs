using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBS.ChatBot.Batch.Helper
{
    public static class IEnumerableExtensions
    {
        public static string AggregateString<T>(this IEnumerable<T> source, string separator, Func<T, string> selector)
        {
            if (source == null) return null;
            return source.Aggregate(new StringBuilder(), (StringBuilder x, T y) =>
            {
                if (x.Length > 0) x.Append(separator);
                return x.Append(selector(y));
            }).ToString();
        }
    }
}
