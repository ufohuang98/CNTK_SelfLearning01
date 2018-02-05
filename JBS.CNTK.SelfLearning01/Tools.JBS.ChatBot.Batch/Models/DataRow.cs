using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBS.ChatBot.Batch.Models
{
    /// <summary>
    /// トレーニングデータおよび評価データの元となるオリジナルデータのデータ行。
    /// </summary>
    public class DataRow
    {
        public string Label { get; private set; }
        public string Sentence { get; private set; }

        public DataRow(string label, string sentence)
        {
            this.Label = label;
            this.Sentence = sentence;
        }
    }
}
