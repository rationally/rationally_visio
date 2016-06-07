using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.Model
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
