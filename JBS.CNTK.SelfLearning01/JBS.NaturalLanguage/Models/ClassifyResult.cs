using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBS.ChatBot.Models
{
    /// <summary>
    /// 分類結果。
    /// </summary>
    public class ClassifyResult
    {
        private List<ClassEstimation> classes;

        /// <summary>
        /// 最もスコアの高い分類を返します。
        /// </summary>
        /// <returns></returns>
        public ClassEstimation TopScore()
        {
            return this.classes.FirstOrDefault();
        }
        /// <summary>
        /// スコアの高い順に結果を取得します。
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public IEnumerable<ClassEstimation> TakeHighScore(int num)
        {
            return this.classes.Take(3);
        }

        public ClassifyResult(IEnumerable<ClassEstimation> source)
        {
            this.classes = source.OrderByDescending(x => x.Score).ToList();
        }
    }
}
