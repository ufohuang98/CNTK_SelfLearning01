using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBS.NaturalLanguage.Models
{
    /// <summary>
    /// 単語ベクトル空間。
    /// </summary>
    public interface IWordVectorSpace
    {
        /// <summary>
        /// 単語をベクトル表現にエンコードします。
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        IWordVector ToVector(string word);

        /// <summary>
        /// 次元の数。
        /// </summary>
        int DimensionSize { get; }

        /// <summary>
        /// このベクトル空間におけるゼロベクトルを返します。
        /// </summary>
        IWordVector Zero { get; }
    }
}
