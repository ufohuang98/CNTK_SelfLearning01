using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBS.ChatBot.Batch.Models;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using JBS.ChatBot.Batch.Helper;

namespace JBS.ChatBot.Batch.Services
{
    /// <summary>
    /// CSVからDataSetを読み込むサービスです。
    /// </summary>
    public class ImportDataSetService
    {
        public ImportDataSetService() { }

        public DataSet Import(string filePath)
        {
            var rows = this.ReadRows(filePath).Select(tsv =>
            {
                string label = tsv[0];
                string question = tsv[1];
                return new DataRow(label, question);
            });

            var filtered = this.Filter(rows);

            return new DataSet(filtered.OrderBy(x => x.Label));
        }

        private IEnumerable<DataRow> Filter(IEnumerable<DataRow> rows)
        {
            // 同一ラベル/同一文はフィルタ。異なるラベル/同一文は例外にする。
            var filtered = new List<DataRow>();
            var errorSentences = new HashSet<string>();
            foreach (var g in rows.GroupBy(x => x.Sentence))
            {
                if (g.Skip(1).Any())
                {
                    var dup = g.GroupBy(x => x.Label).Skip(1).FirstOrDefault();
                    if (dup != null)
                    {
                        // エラーを見つけてもいったん全部なめて、まとめてエラー通知する。
                        errorSentences.Add(dup.First().Sentence);
                        continue;
                    }
                }
                filtered.Add(g.First());
            }

            if (errorSentences.Any())
            {
                throw new ApplicationException($"同一の質問文に対して複数のラベルが割り当てられています。" +
                    $"question={errorSentences.AggregateString(",", x => x)}");
            }
            return filtered;
        }

        private IEnumerable<string[]> ReadRows(string dataSetPath)
        {
            using (var stream = new StreamReader(dataSetPath))
            using (var reader = new CsvReader(stream, new CsvConfiguration() { HasHeaderRecord = false, Delimiter = "\t", }))
            {
                while (reader.Read())
                {
                    yield return reader.CurrentRecord;
                }
            }
        }
    }
}
