using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBS.ChatBot.Models
{
    /// <summary>
    /// 分類結果。
    /// </summary>
    public class ClassEstimation
    {
        public string Label { get; private set; }

        public float Score { get; private set; }

        public ClassEstimation(string label, float score)
        {
            this.Label = label;
            this.Score = score;
        }
    }
}
