using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBS.NaturalLanguage;

namespace JBS.NaturalLanguage.Models
{
    /// <summary>
    /// インデックスされた単語。
    /// </summary>
    public class TextIndex
    {
        /// <summary>
        /// 単語。
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// この単語に割り当てられているインデックス。
        /// </summary>
        public int Index { get; private set; }

        public TextIndex(int index, string text)
        {
            this.Index = index;
            this.Text = text;
        }
    }
}
