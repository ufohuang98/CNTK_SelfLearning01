using JBS.NaturalLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBS.ChatBot.Reader
{
    public class TextIndexesLoader
    {
        private FileResourceReader reader;

        public TextIndexesLoader(FileResourceReader reader)
        {
            this.reader = reader;
        }

        public TextIndexes Load(string dictionaryFileName)
        {
            var dic = this.reader.Read(dictionaryFileName, false)
                                 .ToDictionary(columns => int.Parse(columns[1]), columns => columns[0]);
            return new TextIndexes(dic);
        }
    }
}
