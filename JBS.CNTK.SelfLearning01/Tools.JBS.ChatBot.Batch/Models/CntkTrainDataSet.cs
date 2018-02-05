using JBS.NaturalLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBS.NaturalLanguage.Models;

namespace JBS.ChatBot.Batch.Models
{
    /// <summary>
    /// CNTKトレーニングデータセット.
    /// </summary>
    public class CntkTrainDataSet
    {
        public TextIndexes LabelIndexes { get; private set; }

        public OneHotWordVectorSpace WordSpace { get; private set; }

        public IList<CntkSentence> Sentences { get; private set; }

        public string SegmentatorId { get; private set; }

        public CntkTrainDataSet(
            TextIndexes labelIndexes,
            OneHotWordVectorSpace wordSpace,
            IList<CntkSentence> sentences)
        {
            this.LabelIndexes = labelIndexes;
            this.WordSpace = wordSpace;
            this.Sentences = sentences;
        }
    }
}
