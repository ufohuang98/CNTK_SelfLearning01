using JBS.NaturalLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBS.ChatBot.Cntk
{
    /// <summary>
    /// CNTK分類器の設定情報。ConfigurationはBuilderによってRecipeから生成されます。
    /// </summary>
    public class CntkClassifierConfiguration
    {
        /// <summary>
        /// 適用するトレーニングモデルのファイルパス。
        /// </summary>
        public string ModelPath { get; set; }

        /// <summary>
        /// 分類結果ラベルのインデックスストア。
        /// </summary>
        public TextIndexes LabelIndexStore { get; set; }

        /// <summary>
        /// 単語をベクトル化するためのベクトル空間。
        /// </summary>
        public OneHotWordVectorSpace WordVectorSpace { get; set; }
    }
}
