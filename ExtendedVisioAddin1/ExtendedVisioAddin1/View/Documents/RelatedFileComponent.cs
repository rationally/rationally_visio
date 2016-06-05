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
        }

        public RelatedFileComponent(Page page, string filePath) : base(page)
        {
            RShape = page.InsertFromFile(filePath, (short)VisInsertObjArgs.visInsertLink | (short)VisInsertObjArgs.visInsertIcon);
            RShape.Name = "RelatedFile";
            AddUserRow("rationallyType");
            AddAction("editAction","QUEUEMARKEREVENT(\"edit\")","\"Choose other file\"", false);
            RationallyType = "relatedFile";
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
