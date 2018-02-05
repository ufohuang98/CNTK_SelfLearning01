using System;
using System.Collections.Generic;
using System.Linq;
using JBS.NaturalLanguage;
using JBS.ChatBot.Models;

namespace JBS.ChatBot.Cntk
{
    public class CntkClassifier 
    {
        private CntkProxy proxy;
        private CntkClassifierConfiguration configuration;

        public CntkClassifier(CntkClassifierConfiguration configuration)
        {
            this.proxy =new CntkProxy();
            this.configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wordSequence">分かち書き結果</param>
        /// <returns></returns>
        public ClassifyResult Classify(List<string> wordSequence)
        {
            try
            {
                List<float> classifyResult = this.Evaluate(wordSequence) ?? new List<float>();

                var estimations = Enumerable.Range(0, classifyResult.Count)
                                  .OrderByDescending(idx => classifyResult[idx])
                                  .Select(idx =>
                                  {
                                      var label = this.configuration.LabelIndexStore.ToText(idx) ?? "";
                                      var score = classifyResult[idx];
                                      return new ClassEstimation(label, score);
                                  });

                return new ClassifyResult(estimations);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private List<float> Evaluate(List<string> wordSequence)
        {
            string modelPath = this.configuration.ModelPath;
            var space = this.configuration.WordVectorSpace;
            // ベクトル化
            var oneHotVecs = wordSequence.Select(w =>
                {
                    var vec = space.ToVector(w);
                    if (vec.HasValue)
                    {
                        return ((OneHotWordVector)vec).HotDimension;
                    }
                    return -1;
                }).Where(v => v >= 0);
            if (!oneHotVecs.Any()) return null;

            // 評価
            return this.proxy.EvaluateOneHotVectorSequence(modelPath, oneHotVecs);

        }
    }
}
