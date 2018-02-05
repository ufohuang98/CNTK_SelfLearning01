using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace JBS.ChatBot.Cntk
{
    /// <summary>
    /// CntkライブラリのAPIに対するProxy.
    /// </summary>
    public class CntkProxy
    {

        
        private DeviceDescriptor descriptor = DeviceDescriptor.CPUDevice;
        /// <summary>
        /// one-hot vectorをパラメータとして指定したモデルで評価を行い、出力結果を返します。
        /// </summary>
        /// <param name="modelFilePath"></param>
        /// <param name="inputVector"></param>
        /// <returns></returns>
        public List<float> EvaluateOneHotVectorSequence(string modelFilePath, IEnumerable<int> inputVector)
        {
         
                using (var modelFunc = Function.Load(modelFilePath, descriptor))
                {
                    // 入力変数オブジェクト取得
                    var cntkInputVar = modelFunc.Arguments.First();
                    var vocabSize =  cntkInputVar.Shape; // サイズを取得（テンソルをフラットにした時の形）

                // 入力ベクトルのサイズ、シーケンス、シーケンス先頭フラグ、デバイス
                //var inputSequence = Value.CreateSequence<float>(vocabSize, inputVector.Select(x => x, true, descriptor);
                     var inputSequence = Value.CreateSequence<int>(vocabSize, inputVector.Select(x => x).ToList(), true, descriptor);

                    return this.Evaluate(modelFunc, cntkInputVar, inputSequence, descriptor);

            }
        }

        private List<float> Evaluate(Function model, Variable variable, Value value, DeviceDescriptor descriptor)
        {
            // Build input data map.
            var inputDataMap = new Dictionary<Variable, Value>();
            inputDataMap.Add(variable, value); // 入力オブジェクトとデータをマップする

            // 出力変数の準備
            Variable outputVar = ExtractResultOutPut(model.Outputs);
            // 出力オブジェクトとデータ(null)をマップする
            var outputDataMap = new Dictionary<Variable, Value>();
            outputDataMap.Add(outputVar, null);

            model.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice); // 評価実行

            // 出力オブジェクトとデータのマップから出力データを取り出す
            Value evalResult = outputDataMap[outputVar];

            // 出力データをリストにコピー（謎のシグネチャ）
            var evalResultList = new List<List<float>>();
            evalResult.CopyVariableValueTo(outputVar, evalResultList);
            //evalResult.GetDenseData()

            return evalResultList[0];
        }

        /// <summary>
        /// v1のモデルは戻り値が３つある（そういう風に作っちゃった）ので、そのうちの判定結果のみ抽出する必要がある
        /// </summary>
        /// <param name="outputs"></param>
        /// <returns></returns>
        private Variable ExtractResultOutPut(IList<Variable> outputs)
        {
            if (outputs.Count == 3)
            {
                return outputs[2];
            }
            return outputs[0];
        }
    }
}
