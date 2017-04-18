using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Documents
{
    internal sealed class RelatedURLURLComponent : TextLabel
    {
        private static readonly Regex UrlUrlRegex = new Regex(@"RelatedUrlUrl(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public RelatedURLURLComponent(Page page, Shape shape) : base(page, shape)
        {
            InitStyle();
        }

        public RelatedURLURLComponent(Page page, int index, string labelText) : base(page, labelText)
        {
            RationallyType = "relatedUrlUrl";
            Name = "RelatedUrlUrl";
            Index = index;

            AddAction("addRelatedFile", "QUEUEMARKEREVENT(\"addRelatedFile\")", "Add file", false);
            AddAction("addRelatedUrl", "QUEUEMARKEREVENT(\"addRelatedUrl\")", "Add url", false);
            AddAction("deleteRelatedDocument", "QUEUEMARKEREVENT(\"delete\")", "Delete document", false);

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
