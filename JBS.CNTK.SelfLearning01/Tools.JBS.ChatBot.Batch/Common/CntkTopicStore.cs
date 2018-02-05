using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBS.NaturalLanguage;
using JBS.ChatBot.Batch.Models;

namespace JBS.ChatBot.Batch.Common
{
    public class CntkTopicStore
    {
        public TextIndexes Indexer { get; } = new TextIndexes();
        private Dictionary<int, CntkTopic> topicPool = new Dictionary<int, CntkTopic>();

        public CntkTopic GetOrRegister(string word)
        {
            int index = this.Indexer.Entry(word);
            return GetOrAdd(topicPool, index, () =>
            {
                return new CntkTopic(index, word);
            });
        }

        private TValue GetOrAdd<TKey, TValue>( IDictionary<TKey, TValue> dic, TKey key, Func<TValue> factory)
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
