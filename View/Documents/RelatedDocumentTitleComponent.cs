using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.View.Documents
{
    internal sealed class RelatedDocumentTitleComponent : TextLabel
    {
        private static readonly Regex RelatedRegex = new Regex($@"{ShapeNames.RelatedDocumentTitle}(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public RelatedDocumentTitleComponent(Page page, Shape shape) : base(page, shape)
        {
            Shape = shape;
            InitStyle();
        }

        public RelatedDocumentTitleComponent(Page page, int index, string text) : base(page, text)
        {
            RationallyType = ShapeNames.TypeRelatedDocumentTitle;
            Name = ShapeNames.RelatedDocumentTitle;
            Index = index;

            AddAction("addRelatedFile", string.Format(VisioFormulas.Formula_QUEUMARKEREVENT, "addRelatedFile"), Messages.Menu_AddFile, false);
            AddAction("addRelatedUrl", string.Format(VisioFormulas.Formula_QUEUMARKEREVENT, "addRelatedUrl"), Messages.Menu_AddUrl, false);
            AddAction("deleteRelatedDocument", string.Format(VisioFormulas.Formula_QUEUMARKEREVENT, "delete"), Messages.Menu_DeleteDocument, false);
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
