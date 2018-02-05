using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBS.NaturalLanguage.Models;

namespace JBS.NaturalLanguage
{
    /// <summary>
    /// <see cref="ITextIndexes"/>の標準実装。
    /// </summary>
    public class TextIndexes: ITextIndexes
    {
        // bi-dictionary
        private Dictionary<int, string> index_to_text;
        private Dictionary<string, int> text_to_index;

        public int Count => this.text_to_index.Count;

        /// <summary>
        /// 空の<see cref="TextIndexes"/>を作成します。
        /// </summary>
        public TextIndexes()
        {
            this.index_to_text = new Dictionary<int, string>();
            this.text_to_index = new Dictionary<string, int>();
        }

        /// <summary>
        /// 既にインデックス済みの単語ディクショナリから<see cref="TextIndexes"/>を作成します。
        /// </summary>
        /// <param name="words"></param>
        public TextIndexes(IDictionary<int, string> words)
        {
            this.index_to_text = new Dictionary<int, string>(words);
            this.text_to_index = words.ToDictionary(x => x.Value, x => x.Key);
        }

        /// <summary>
        /// まだインデックスが割り振られていない単語リストを用いて<see cref="TextIndexes"/>を作成します。
        /// </summary>
        /// <param name="words"></param>
        public TextIndexes(ICollection<string> words) : this()
        {
            foreach (string word in words)
            {
                this.Entry(word);
            }
        }

        /// <summary>
        /// 単語がまだ未登録の場合、Indexストアに登録して発番されたIndexを返します。既に登録済みの場合、発番済みIndexを返します。
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public int Entry(string word)
        {
            int entried_index;
            if (!this.text_to_index.TryGetValue(word, out entried_index))
            {
                entried_index = this.Count;
                this.text_to_index[word] = entried_index;
                this.index_to_text[entried_index] = word;
            }
            return entried_index;
        }

        /// <summary>
        /// 単語を一括登録します。
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public void Entry(IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                this.Entry(word);
            }
        }

        public string ToText(int index)
        {
            return GetOrDefault(index_to_text,index);
        }

        public int ToIndex(string word)
        {
            return this.IsDefined(word) ? this.text_to_index[word] : -1;
        }

        public bool IsDefined(string word)
        {
            return this.text_to_index.ContainsKey(word);
        }

        public IEnumerable<TextIndex> AllEntries()
        {
            return this.text_to_index.Select(kv =>
            {
                return new TextIndex(kv.Value, kv.Key);
            }).OrderBy(x => x.Index);
        }

        public static TValue GetOrDefault<TKey, TValue>(IDictionary<TKey, TValue> dic, TKey key)
        {
            TValue value;
            if (dic.TryGetValue(key, out value))
            {
                return value;
            }
            return default(TValue);
        }
    }
}
