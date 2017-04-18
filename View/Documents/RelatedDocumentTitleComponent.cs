using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.View.Documents
{
    internal sealed class RelatedDocumentTitleComponent : TextLabel
    {
        private static readonly Regex RelatedRegex = new Regex(@"Related Document Title(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public RelatedDocumentTitleComponent(Page page, Shape shape) : base(page, shape)
        {
            Shape = shape;
            InitStyle();
        }

        public RelatedDocumentTitleComponent(Page page, int index, string text) : base(page, text)
        {
            RationallyType = "relatedDocumentTitle";
            Name = "Related Document Title";
            AddUserRow("index");
            Index = index;

            AddAction("addRelatedFile", "QUEUEMARKEREVENT(\"addRelatedFile\")", "Add file", false);
            AddAction("addRelatedUrl", "QUEUEMARKEREVENT(\"addRelatedUrl\")", "Add url", false);
            AddAction("deleteRelatedDocument", "QUEUEMARKEREVENT(\"delete\")", "Delete document", false);
            InitStyle();
        }

        private void InitStyle()
        {
            Width = 3.3;
            Height = 0.6;
            SetMargin(0.1);
            HAlign = 0;
            SetUsedSizingPolicy(SizingPolicy.FixedSize);
        }

        internal static bool IsRelatedDocumentTitleContainer(string name) => RelatedRegex.IsMatch(name);
        

        public override void Repaint()
        {

            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)//Visio does this for us
            {
                if (Globals.RationallyAddIn.Model.Documents.Count > Index)
                {
                    RelatedDocument doc = Globals.RationallyAddIn.Model.Documents[Index];
                    if (Text != doc.Name)
                    {
                        Text = doc.Name;
                    }
                }
                UpdateReorderFunctions(Globals.RationallyAddIn.Model.Documents.Count - 1);
            }
            base.Repaint();
        }
    }
}
