using JBS.NaturalLanguage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBS.NaturalLanguage
{
    /// <summary>
    /// 単語をone-hotベクトルで表現する、単語ベクトル空間。
    /// </summary>
    public class OneHotWordVectorSpace : IWordVectorSpace
    {
        /// <summary>
        /// このone-hotベクトル空間に適用されている単語インデックスストア。
        /// </summary>
        public ITextIndexes WordIndexes { get; private set; }

        public int DimensionSize { get; private set; }

        public IWordVector Zero { get; private set; }

        private OneHotWordVectorSpace(ITextIndexes wordIndexes)
        {
            this.WordIndexes = wordIndexes;
            this.DimensionSize = this.WordIndexes.Count;
            this.Zero = new ZeroWordVector(this.DimensionSize);
        }

        /// <summary>
        /// 単語インデックス辞書を用いて<see cref="OneHotWordVectorSpace"/>を構成します。
        /// </summary>
        /// <param name="wordIndexes"></param>
        public static OneHotWordVectorSpace Load(ITextIndexes wordIndexes)
        {
            return new OneHotWordVectorSpace(wordIndexes);
        }

        /// <summary>
        /// インデックス付けされていない単語リストを用いて<see cref="OneHotWordVectorSpace"/>を構成します。
        /// </summary>
        /// <param name="indexedWords"></param>
        public static OneHotWordVectorSpace Build(ISet<string> unindexedWords)
        {
            var index = new TextIndexes();
            index.Entry(unindexedWords);
            return new OneHotWordVectorSpace(index);
        }

        public IWordVector ToVector(string word)
        {
            if (!this.WordIndexes.IsDefined(word)) return this.Zero;
            int dim = this.WordIndexes.ToIndex(word);
            return new OneHotWordVector(this.DimensionSize, dim);
        }
    }
}
