using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBS.ChatBot.Batch.Models;
using JBS.NaturalLanguage;
using System.IO;

namespace JBS.ChatBot.Batch.Services
{
    public class ExportCntkTrainDataSetService
    {
        public void Export(CntkTrainDataSet trainData)
        {
            // trainning_data.tsv

            this.WriteTranInputData(trainData.Sentences);

            // label
            Console.WriteLine("creating label_index.tsv in folder : Data");
            string labelFile =@"Data\labels_index.tsv";
            this.WriteWordIndexStore(trainData.LabelIndexes, labelFile);

            // words
            Console.WriteLine("creating words_index.tsv in folder: Data");
            ITextIndexes indexes = (trainData.WordSpace).WordIndexes;
            if (indexes != null)
            {
                string wordsFilePath = @"Data\words_index.tsv";
                WriteWordIndexStore(indexes, wordsFilePath);
            }
        }

        /// <summary>
        /// CNTKトレーニングデータを出力します。
        /// </summary>
        /// <param name="sentences"></param>
        private void WriteTranInputData(IList<CntkSentence> sentences)
        {
            var trainSentences = new List<CntkSentence>();
            var evalSentences = new List<CntkSentence>();
            string trainOutputPath = @"Data\train_data.tsv";
            string evalOutputPath = @"Eval\eval_data.tsv";
            var n = sentences.Count / 10;
            Random ra = new Random();
            var list = new List<int>();
            for (int i=0; i < n; i++)
            {               
                var number = ra.Next(0, sentences.Count);
                while (list.Contains(number))
                {
                    number = ra.Next(0, sentences.Count);
                }
                list.Add(number);
            }
            for(int i=0;i< sentences.Count; i++)
            {
                if (list.Contains(i))
                {
                    evalSentences.Add(sentences[i]);
                }
                else
                {
                    trainSentences.Add(sentences[i]);
                }
            }
            Console.WriteLine("creating train_data.tsv in folder : Data ");
            WriteInputData(trainOutputPath, trainSentences);
            Console.WriteLine("creating eval_data.tsv in folder : Eval");
            WriteInputData(evalOutputPath, evalSentences);
        }

        private string ToStringLine(CntkSentence sentence, CntkWord word, bool appendCT)
        {
            string vectorValue =word.Value.ToSparseString();

            string line = $"{sentence.Id}\t|#{this.Escape(word.Text)}\t|CW {vectorValue}\t";
            if (appendCT)
            {
                line += $"|CT {sentence.Topic.Id}:1";
            }
            return line;
        }

        private string Escape(string comment)
        {
            if (comment == null) return "";
            return comment.Replace("|", "{pipe}").Replace("\t", "{tab}");
        }

        /// <summary>
        /// <see cref="ITextIndexes"/>の内容をtsvとして出力します。
        /// </summary>
        /// <param name="indexes"></param>
        /// <param name="outputFilePath"></param>
        private void WriteWordIndexStore(ITextIndexes indexes, string outputFilePath)
        {
            using (var stream = new StreamWriter(outputFilePath, append: false, encoding: Encoding.UTF8))
            {
                foreach (var entry in indexes.AllEntries())
                {
                    stream.Write(entry.Text);
                    stream.Write("\t");
                    stream.Write(entry.Index);
                    stream.WriteLine();
                }
                stream.Flush();
            }
        }

        private void WriteInputData(string path,List<CntkSentence> sentences)
        {
            using (var stream = new StreamWriter(path, append: false, encoding: Encoding.UTF8))
            {
                foreach (var sentence in sentences)
                {
                    int index = 0;
                    foreach (var word in sentence.Words)
                    {
                        bool appendCT = index == 0;
                        stream.WriteLine(this.ToStringLine(sentence, word, appendCT));
                        index++;
                    }
                }
                stream.Flush();
            }
        }
    }
}
