using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBS.NaturalLanguage.Models
{
    /// <summary>
    /// 単語ベクトル.
    /// </summary>
    public interface IWordVector
    {
        /// <summary>
        /// ベクトル量を数値配列表現で返します。
        /// </summary>
        /// <returns></returns>
        float[] ToNumericValue();

        /// <summary>
        /// ベクトル量をSparse表現の文字列として返します。
        /// </summary>
        /// <returns></returns>
        string ToSparseString();

        /// <summary>
        /// ベクトルが値を持っているかどうか返します。コーパスにない未知語の場合はtrueになります。
        /// </summary>
        bool HasValue { get; }
    }
}
