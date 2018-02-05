using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBS.NaturalLanguage;
using JBS.ChatBot.Models;
using JBS.ChatBot.Cntk;
using JBS.ChatBot.Reader;

namespace JBS.ChatBot.Service
{
    /// <summary>
    /// <see cref="IDocumentAnalysisService"/>の標準実装。
    /// </summary>
    public class DocumentAnalysisService
    {
        private MeCabTextSegmentator segmantator;

        public DocumentAnalysisService()
        {
            this.segmantator = new MeCabTextSegmentator();
        }

        /// <summary>
        /// CNTK分類器の結果を返す
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ClassEstimation Analyze(string query)
        {
            // Mecabでセンテンスを分かち書き
            List<string> words = this.ToWordSequence(query);
            //分類
            ClassifyResult topicResult = this.DoClassify(words);
            var result = topicResult.TopScore();
            
            return result;
        }

        /// <summary>
        /// 文章を分類器にかけるための単語シーケンスに変換します。
        /// </summary>
        /// <param name="query"></param>
        /// <param name="recipe"></param>
        /// <returns></returns>
        private List<string> ToWordSequence(string query)
        {
            List<string> wordSequence = segmantator.Split(query, true);

            return wordSequence;
        }

        /// <summary>
        /// レシピを元に分類処理を実行します。
        /// </summary>
        /// <param name="wordSequence"></param>
        /// <param name="recipe"></param>
        /// <returns></returns>
        private ClassifyResult DoClassify(List<string> wordSequence)
        {
            var indexStore = new FileWordIndexStoreStack(FileResourceReader.FromDirectory(""));
            var configuration = new CntkClassifierConfiguration()
            {
                ModelPath = @"Resource\lstm_model.dnn",
                LabelIndexStore = indexStore.LabelIndexes(),
                WordVectorSpace = OneHotWordVectorSpace.Load(indexStore.WordIndexes())
            };
            CntkClassifier classifier=new CntkClassifier(configuration);    
            return classifier.Classify(wordSequence);
        }
    }
}
