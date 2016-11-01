namespace Rationally.Visio.Model
{
    public class RelatedDocument
    {
        public string Path { get; set; }
        public string Name { get; set; }

        public bool IsFile { get; set; } //else url

        public RelatedDocument(string path, string name, bool isFile)
        {
            Path = path;
            Name = name;
            IsFile = isFile;
        }
    }
}
