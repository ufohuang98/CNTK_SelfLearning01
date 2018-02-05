using JBS.NaturalLanguage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBS.ChatBot.Batch.Models
{
    public class CntkWord
    {
        public IWordVector Value { get; set; }

        public string Text { get; set; }
    }
}
