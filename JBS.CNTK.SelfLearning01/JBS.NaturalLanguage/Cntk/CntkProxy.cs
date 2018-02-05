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
                var vocabSize = cntkInputVar.Shape.TotalSize; // サイズを取得（テンソルをフラットにした時の形）
                // 入力ベクトルのサイズ、シーケンス、シーケンス先頭フラグ、デバイス
                var inputSequence = Value.CreateSequence<float>(vocabSize, inputVector.Select(x => x).ToList(), descriptor, true);
                return this.Evaluate(modelFunc, cntkInputVar, inputSequence, descriptor);
            }
        }

        private List<float> Evaluate(Function model, Variable variable, Value value, DeviceDescriptor descriptor)
        {
            // 入力オブジェクトとデータをマップする
            var inputDataMap = new Dictionary<Variable, Value>
            {
                { variable, value }
            };
            // 出力変数の準備
            Variable outputVar = model.Output;

            // 出力オブジェクトとデータ(null)をマップする
            var outputDataMap = new Dictionary<Variable, Value>
            {
                { outputVar, null }
            };

            // 評価実行
            model.Evaluate(inputDataMap, outputDataMap, descriptor); 

            // 出力オブジェクトとデータのマップから出力データを取り出す
            Value evalResult = outputDataMap[outputVar]; 
            var output=evalResult.GetDenseData<float>(outputVar);
            return (List<float>)output[0];
        }
    }
}
