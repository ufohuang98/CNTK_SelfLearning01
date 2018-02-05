using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBS.NaturalLanguage.Models;

namespace JBS.NaturalLanguage
{
    /// <summary>
    /// 単語・語句に対して割り振られたインデックスを管理するデータストア。
    /// </summary>
    public interface ITextIndexes
    {
        /// <summary>
        /// インデックスストアに登録されている語句の総数。
        /// </summary>
        int Count { get; }

        /// <summary>
        /// インデックス番号が割り振られている単語を返します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        string ToText(int index);

        /// <summary>
        /// 単語に割り振られているインデックス番号を返します。
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        int ToIndex(string word);

        /// <summary>
        /// 単語がインデックスストアに登録されているかどうかを返します。
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        bool IsDefined(string word);

        /// <summary>
        /// インデックスストアに登録されている内容を全て取得します。
        /// </summary>
        /// <returns></returns>
        IEnumerable<TextIndex> AllEntries();
    }
}
