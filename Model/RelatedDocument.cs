using System.Reflection;
using log4net;

namespace Rationally.Visio.Model
{
    public class RelatedDocument
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public string Path { get; set; }
        public string Name { get; set; }

        public bool IsFile { get;} //else url

        public RelatedDocument(string path, string name, bool isFile)
        {
            Path = path;
            Name = name;
            IsFile = isFile;
        }
    }
}
