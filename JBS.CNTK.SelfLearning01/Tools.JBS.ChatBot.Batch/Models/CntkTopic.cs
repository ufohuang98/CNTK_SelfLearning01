using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBS.ChatBot.Batch.Models
{
    /// <summary>
    /// CNTKトレーニングデータ(Topic).
    /// </summary>
    public class CntkTopic
    {
        public int Id { get; private set; }
        public string Label { get; private set; }

        public CntkTopic(int id, string label)
        {
            this.Id = id;
            this.Label = label;
        }
    }
}
