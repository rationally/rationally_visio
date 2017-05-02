using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Documents
{
    internal sealed class RelatedURLURLComponent : TextLabel
    {
        private static readonly Regex UrlUrlRegex = new Regex($@"{ShapeNames.RelatedUrlUrl}(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public RelatedURLURLComponent(Page page, Shape shape) : base(page, shape)
        {
            InitStyle();
        }

        public RelatedURLURLComponent(Page page, int index, string labelText) : base(page, labelText)
        {
            RationallyType = ShapeNames.TypeRelatedUrlUrl;
            Name = ShapeNames.RelatedUrlUrl;
            Index = index;

            AddAction("addRelatedFile", string.Format(VisioFormulas.Formula_QUEUMARKEREVENT, "addRelatedFile"), Messages.Menu_AddFile, false);
            AddAction("addRelatedUrl", string.Format(VisioFormulas.Formula_QUEUMARKEREVENT, "addRelatedUrl"), Messages.Menu_AddUrl, false);
            AddAction("deleteRelatedDocument", string.Format(VisioFormulas.Formula_QUEUMARKEREVENT, "delete"), Messages.Menu_DeleteDocument, false);

            Width = 4.2;
            InitStyle();
        }

        private void InitStyle()
        {
            UsedSizingPolicy = SizingPolicy.All;
            SetUsedSizingPolicy(UsedSizingPolicy &= ~SizingPolicy.ExpandXIfNeeded);//we want to remove this one from the policy: AND with everything else on true
        }

        public static bool IsRelatedUrlUrlComponent(string name) => UrlUrlRegex.IsMatch(name);
        

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //Visio does this for us
            {
                UpdateReorderFunctions(Globals.RationallyAddIn.Model.Documents.Count - 1);
            }
            base.Repaint();
        }
    }
}
