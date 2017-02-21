using System.Reflection;
using log4net;
using Newtonsoft.Json;

namespace Rationally.Visio.Model
{
    public class RelatedDocument
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static int highestUniqueIdentifier = -1;
        public string Path { get; set; }
        public string Name { get; set; }

        public int UniqueIdentifier { get; set; } //Id that exists independent of the order of the elements. Allows for the identifying of the document
        public bool IsFile { get; set; } //else url

        public RelatedDocument(string path, string name, bool isFile)
        {
            Path = path;
            Name = name;
            IsFile = isFile;
            UniqueIdentifier = ++highestUniqueIdentifier;
        }

        public RelatedDocument(string path, string name, bool isFile, int uniqueIdentifier)
        {
            Path = path;
            Name = name;
            IsFile = isFile;
            UniqueIdentifier = uniqueIdentifier;
            if (uniqueIdentifier > highestUniqueIdentifier)
            {
                highestUniqueIdentifier = uniqueIdentifier;
            }
        }

        [JsonConstructor]
        private RelatedDocument()
        {
        }
    }
}
