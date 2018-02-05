using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBS.ChatBot.Batch.Models;
using System.IO;
using JBS.ChatBot.Batch.Helper;

namespace JBS.ChatBot.Batch.Services
{
    public class ExportInfo
    {
        public int LabelDimension {get; set; }
        public int WordDimension { get; set; }
    }
    public class ExportTrainSummaryInfoService
    {
        public void Export(CntkTrainDataSet trainData)
        {
            Console.WriteLine("creating export_info.json in folder : Data ");
            var info = this.CreateExportInfo(trainData);

            string filePath = @"Data\export_info.json";
            File.WriteAllText(filePath, JsonHelper.ToJson(info));
        }

        private ExportInfo CreateExportInfo(CntkTrainDataSet trainData)
        {
            var info = new ExportInfo()
            {
                LabelDimension = trainData.LabelIndexes.Count,
                WordDimension = trainData.WordSpace.DimensionSize,
            };

            return info;
        }
    }
}
