using System;
using System.Collections.Generic;
using System.Linq;
using JBS.ChatBot.Batch.Models;
using JBS.NaturalLanguage;
using JBS.ChatBot.Batch.Common;
namespace JBS.ChatBot.Batch
{
    public class TrainingDataSetTransform 
    {

        public CntkTrainDataSet Transform(DataSet dataSet)
        {
            var segmentator = new MeCabTextSegmentator();
            var topicStore = new CntkTopicStore();
            var corpus = new HashSet<string>();
            int sentenceId = 0;

            var n = 0;
            //var count = sentences.Count;
            List<CntkSentence> sentences = dataSet.Select(row =>
            {
                n++;
                if (n % 10 == 0)
                {
                    Console.WriteLine(n+" sentences");
                }
                CntkTopic topic = topicStore.GetOrRegister(row.Label);
                // クレンジングをかけつつ単語収集
                //string cleanedSentence = textTransformBefore.Transform(row.Sentence);
                List<string> words = segmentator.Split(row.Sentence);

                words.ForEach(w => corpus.Add(w));
                return new CntkSentence()
                {
                    Id = sentenceId++,
                    Topic = topic,
                    Sentence = row.Sentence,
                    Words = words.Select(w => new CntkWord() { Text = w }).ToList(),
                };
            }).ToList();

            // 取り込む単語が全て確定しないとVector表現が決まらないので、そこだけ最後
            var space = OneHotWordVectorSpace.Build(corpus);

            foreach (var sentence in sentences)
            {

                foreach (var word in sentence.Words)
                {
                    word.Value = space.ToVector(word.Text);
                }
            }

            var trainData = new CntkTrainDataSet(topicStore.Indexer, space, sentences);

            return trainData;
        }
    }
}
