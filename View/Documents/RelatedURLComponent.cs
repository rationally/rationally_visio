using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.RationallyConstants;

namespace Rationally.Visio.View.Documents
{
    internal sealed class RelatedUrlComponent : VisioShape
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex RelatedRegex = new Regex($@"{ShapeNames.RelatedUrl}(\.\d+)?$");
        public RelatedUrlComponent(Page page, Shape urlShape) : base(page)
        {
            Shape = urlShape;
            InitStyle();
        }

        public RelatedUrlComponent(Page page, int index, string url) : base(page)
        {
            string docPath = Constants.MyShapesFolder + "\\RationallyHidden.vssx";
            Document rationallyDocument = Globals.RationallyAddIn.Application.Documents.OpenEx(docPath, (short)VisOpenSaveArgs.visAddHidden);
            Master rectMaster = rationallyDocument.Masters["LinkIcon"]; 
            Shape = page.Drop(rectMaster, 0, 0);

            Hyperlink link = Shape.AddHyperlink();
            link.Address = url;
            EventDblClick = VisioFormulas.Formula_OpenHyperlink;

            InitStyle();

            Name = ShapeNames.RelatedUrl;
            RationallyType = ShapeNames.TypeRelatedUrl;
            Index = index;

            AddAction("addRelatedFile", string.Format(VisioFormulas.Formula_QUEUMARKEREVENT, "addRelatedFile"), Messages.Menu_AddFile, false);
            AddAction("addRelatedUrl", string.Format(VisioFormulas.Formula_QUEUMARKEREVENT, "addRelatedUrl"), Messages.Menu_AddUrl, false);
            AddAction("deleteRelatedDocument", string.Format(VisioFormulas.Formula_QUEUMARKEREVENT, "delete"), Messages.Menu_DeleteDocument, false);

            rationallyDocument.Close();
        }

        private void InitStyle()
        {
            Width = 0.6;
            Height = 0.6;
            SetMargin(0.1);
        }

        internal static bool IsRelatedUrlComponent(string name) => RelatedRegex.IsMatch(name);
        

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)//Visio does this for us
            {
                UpdateReorderFunctions(Globals.RationallyAddIn.Model.Documents.Count - 1);
            }
            base.Repaint();
        }
    }
}
