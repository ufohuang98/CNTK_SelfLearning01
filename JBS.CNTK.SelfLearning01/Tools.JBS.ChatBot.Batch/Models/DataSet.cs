using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JBS.ChatBot.Batch.Models
{
    /// <summary>
    /// トレーニングデータおよび評価データの元となるオリジナルのデータセット。
    /// </summary>
    public class DataSet : IEnumerable<DataRow>
    {
        public List<DataRow> Rows { get; private set; } = new List<DataRow>();

        public DataSet() { }

        public DataSet(IEnumerable<DataRow> rows)
        {
            this.Rows = rows.ToList();
        }

        public DataSet(DataSet dataset)
        {
            this.Rows = dataset.Rows.ToList();
        }

        public void AddRow(string label, string question)
        {
            this.Rows.Add(new DataRow(label, question));
        }

        public void AddRow(DataRow row)
        {
            this.Rows.Add(row);
        }

        public void AddRows(IEnumerable<DataRow> rows)
        {
            this.Rows.AddRange(rows);
        }

        IEnumerator<DataRow> IEnumerable<DataRow>.GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }
    }
}
