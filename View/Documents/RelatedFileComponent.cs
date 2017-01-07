using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Documents
{
    internal sealed class RelatedFileComponent : RationallyComponent
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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

            AddUserRow("filePath");
            FilePath = filePath; //store the path of the file this shape is a link to, so it can be read during a tree rebuild

            RationallyType = "relatedFile";
            AddUserRow("documentIndex");
            DocumentIndex = index;

            AddAction("addRelatedFile", "QUEUEMARKEREVENT(\"addRelatedFile\")", "\"Add file\"", false);
            AddAction("addRelatedUrl", "QUEUEMARKEREVENT(\"addRelatedUrl\")", "\"Add url\"", false);
            AddAction("deleteRelatedDocument", "QUEUEMARKEREVENT(\"delete\")", "\"Delete document\"", false);

            InitStyle();
        }

        private void InitStyle()
        {
            Width = 0.6;
            Height = 0.6;
            SetMargin(0.1);
        }

        internal static bool IsRelatedFileComponent(string name) => RelatedRegex.IsMatch(name);

        private void UpdateReorderFunctions()
        {
            AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "\"Move up\"", false);
            AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "\"Move down\"", false);

            if (DocumentIndex == 0)
            {
                DeleteAction("moveUp");
            }

            if (DocumentIndex == Globals.RationallyAddIn.Model.Documents.Count - 1)
            {
                DeleteAction("moveDown");
            }
        }

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)//Visio does this for us
            {
                UpdateReorderFunctions();
            }
            base.Repaint();
        }

    }
}
