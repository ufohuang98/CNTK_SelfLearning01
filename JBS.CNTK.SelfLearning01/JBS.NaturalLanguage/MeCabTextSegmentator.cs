using NMeCab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBS.NaturalLanguage
{
    public class MeCabTextSegmentator 
    {
        public List<string> Split(string text, bool containIsNotPhraseStart = true)
        {
            //Console.WriteLine($"分かち書き前 {text}");
            var outputWordList = SplitDocumentsWithMeCab(text, containIsNotPhraseStart);         
            return outputWordList.ToList();
        }

        /// <summary>
        /// 分かち書き
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> SplitDocumentsWithMeCab(string rawDocument, bool containIsNotPhrageStart)
        {
            var words = new List<string>();
            
            MeCabParam param = new MeCabParam();
            param.DicDir = @"lib\MeCab\dic\ipadic";
            MeCabTagger t = MeCabTagger.Create(param);

            //形態素解析を行い結果を記録
            string result = t.Parse(rawDocument).Replace("\t", ",");
            var results = result.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (var feature in results)
            {
                // MeCabの結果を要素ごとに分割
                var featureElements = feature.Split(',');
                // 品詞を解析
                // BOS/EOS(開始、終端)を除去する
                if ("EOS" == featureElements[0]
                    || String.IsNullOrWhiteSpace(featureElements[0])
                    || containIsNotPhrageStart && "助詞" == featureElements[1]
                    || containIsNotPhrageStart && "助動詞" == featureElements[1]
                    || containIsNotPhrageStart && "記号" == featureElements[1])
                {
                    continue;
                }
                // 文節を結果のリストに格納
                yield return featureElements[0];
            }
        }
    }
}
