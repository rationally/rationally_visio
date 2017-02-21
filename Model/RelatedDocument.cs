using System.Reflection;
using log4net;
using Newtonsoft.Json;

namespace Rationally.Visio.Model
{
    public class RelatedDocument
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static int highestId = -1;
        public string Path { get; set; }
        public string Name { get; set; }

        public int Id { get; set; } //Id that exists independent of the order of the elements. Allows for the identifying of the document
        public bool IsFile { get; set; } //else url

        public RelatedDocument(string path, string name, bool isFile)
        {
            Path = path;
            Name = name;
            IsFile = isFile;
            Id = ++highestId;
        }

        public RelatedDocument(string path, string name, bool isFile, int id)
        {
            Path = path;
            Name = name;
            IsFile = isFile;
            Id = id;
            if (id > highestId)
            {
                highestId = id;
            }
        }

        [JsonConstructor]
        private RelatedDocument()
        {
        }
    }
}
