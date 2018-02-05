using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBS.ChatBot.Batch.Helper
{
    public static class IDictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
        {
            TValue value;
            if (dic.TryGetValue(key, out value))
            {
                return value;
            }
            return default(TValue);
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, Func<TValue> factory)
        {
            TValue value;
            if (!dic.TryGetValue(key, out value))
            {
                value = factory();
                dic[key] = value;
            }
            return value;
        }
    }
}
