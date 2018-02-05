using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBS.ChatBot.Batch.Models
{
    /// <summary>
    /// CNTKトレーニングデータ(Sentence).
    /// </summary>
    public class CntkSentence
    {
        public int Id { get; set; }

        public string Sentence { get; set; }

        public CntkTopic Topic { get; set; }

        public List<CntkWord> Words { get; set; }
    }
}
