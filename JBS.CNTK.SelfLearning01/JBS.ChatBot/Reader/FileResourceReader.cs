using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace JBS.ChatBot.Reader
{
    /// <summary>
    /// ファイルからリソースを読み込むリーダー。Dllリソースまたはローカルディスクから読み込みます。
    /// </summary>
    public class FileResourceReader 
    {
        private readonly string rootDirectory;

        private FileResourceReader(string rootDirectory)
        {
            this.rootDirectory = rootDirectory;
        }

        /// <summary>
        /// Dataアセンブリのリソースファイルから読み込むReaderを作成します。
        /// </summary>
        /// <returns></returns>
        public static FileResourceReader FromResouce()
        {
            return new FileResourceReader(null);
        }

        /// <summary>
        /// ローカルディスク上のファイルから読み込むReaderを作成します。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileResourceReader FromDirectory(string path)
        {
            return new FileResourceReader(path);
        }

        //string rootPath = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data"), "");
        //return new FileResourceReader(rootPath);
        public IEnumerable<string[]> Read(string fileName, bool hasHeader = true, bool ignoreQuotes = true)
        {
            string extension = Path.GetExtension(fileName);
            var config = this.CreateConfiguration(extension, hasHeader, ignoreQuotes);
            return this.ReadFile(fileName, config);
        }

        public IEnumerable<string[]> ReadFile(string fileName, CsvConfiguration configuration)
        {
            if (fileName == null) yield break;

            string directory = @"Resource\";
            string filePath = Path.Combine(directory, fileName);

            using (var stream = new StreamReader(filePath))
            using (var reader = new CsvReader(stream, configuration))
            {
                while (reader.Read())
                {
                    yield return reader.CurrentRecord;
                }
            }
        }

        private CsvConfiguration CreateConfiguration(string fileExtension, bool hasHeader, bool ignoreQuotes = true)
        {
            if (fileExtension == ".csv")
            {
                return new CsvConfiguration() { HasHeaderRecord = hasHeader, };
            }
            else if (fileExtension == ".tsv")
            {
                return new CsvConfiguration()
                {
                    HasHeaderRecord = hasHeader,
                    Delimiter = "\t",
                    IgnoreQuotes = ignoreQuotes
                };
            }
            else
            {
                return this.CreateConfiguration(".csv", hasHeader, ignoreQuotes);
            }
        }
    }
}
