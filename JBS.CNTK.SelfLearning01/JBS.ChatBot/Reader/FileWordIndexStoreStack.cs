using JBS.NaturalLanguage;
using System.Collections.Concurrent;

namespace JBS.ChatBot.Reader
{
    /// <summary>
    /// ResourceファイルからWordIndexをロードする
    /// </summary>
    public class FileWordIndexStoreStack
    {
        private ConcurrentDictionary<string, TextIndexes> indexStoreCache = new ConcurrentDictionary<string, TextIndexes>();
        private TextIndexesLoader loader;

        public FileWordIndexStoreStack(FileResourceReader reader)
        {
            this.loader = new TextIndexesLoader(reader);
        }

        private TextIndexes Find(string recipeName)
        {
            return this.indexStoreCache.GetOrAdd(recipeName, key =>
            {
                return this.loader.Load(key);
            });
        }

        public TextIndexes LabelIndexes()
        {
            string fileName = $"labels_index.tsv";
            return this.Find(fileName);
        }

        public TextIndexes WordIndexes()
        {
            string fileName = $"words_index.tsv";
            return this.Find(fileName);
        }
    }
}
