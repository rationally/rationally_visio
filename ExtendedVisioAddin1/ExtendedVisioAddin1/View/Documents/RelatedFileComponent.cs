using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    internal class RelatedFileComponent : RComponent
    {
        private static readonly Regex RelatedRegex = new Regex(@"RelatedFile(\.\d+)?$");
        public RelatedFileComponent(Page page, Shape fileShape) : base(page)
        {
            RShape = fileShape;
            InitStyle();
        }

        public RelatedFileComponent(Page page, int index, string filePath) : base(page)
        {
            RShape = page.InsertFromFile(filePath, (short)VisInsertObjArgs.visInsertLink | (short)VisInsertObjArgs.visInsertIcon);
            Name = "RelatedFile";
            AddUserRow("rationallyType");
            AddAction("editAction","QUEUEMARKEREVENT(\"edit\")","\"Choose other file\"", false);
            RationallyType = "relatedFile";
            AddUserRow("documentIndex");
            DocumentIndex = index;
            InitStyle();
        }

        private void InitStyle()
        {
            Width = 0.6;
            Height = 0.6;
            SetMargin(0.1);
        }

        internal static bool IsRelatedFileComponent(string name)
        {
            return RelatedRegex.IsMatch(name);
        }
        
    }
}
