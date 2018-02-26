using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBS.ChatBot.Batch.Services;
using JBS.ChatBot.Batch.Models;

namespace JBS.ChatBot.Batch
{
    /// <summary>
    /// 学習用データセットを元にCNTK用トレーニングデータを生成するタスクを実行します。
    /// </summary>
    public class CreateCntkTrainDataTasks 
    {

        private ImportDataSetService dataSetImport;
        private ExportCntkTrainDataSetService trainDataSetExport;
        private ExportTrainSummaryInfoService summaryInfoExport;
        private TrainingDataSetTransform trainDataSetTransform;

        public CreateCntkTrainDataTasks()
        {
            this.dataSetImport = new ImportDataSetService();
            this.trainDataSetExport = new ExportCntkTrainDataSetService();
            this.summaryInfoExport = new ExportTrainSummaryInfoService();
            this.trainDataSetTransform = new TrainingDataSetTransform();
        }

        public void Run()
        {

            // 学習データの取り込み
            string dataSetFilePath = @"Resource\train_dataset.tsv";//context.BaseContext.TrainDataSetPath();
            DataSet data = this.dataSetImport.Import(dataSetFilePath);

            // CNTKトレーニングデータ生成
            Console.WriteLine("reading data...");
            CntkTrainDataSet trainDataSet = this.trainDataSetTransform.Transform(data);

            // CNTKトレーニングデータ出力
            this.trainDataSetExport.Export(trainDataSet);

            //トレーニングデータサマリー出力
            this.summaryInfoExport.Export( trainDataSet);
        }
    }
}
