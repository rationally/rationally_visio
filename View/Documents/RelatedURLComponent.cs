﻿using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.RationallyConstants;

namespace Rationally.Visio.View.Documents
{
    internal sealed class RelatedUrlComponent : VisioShape
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex RelatedRegex = new Regex(@"RelatedUrl(\.\d+)?$");
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
            EventDblClick = "HYPERLINK(Hyperlink.Row_1.Address)";

            InitStyle();

            Name = "RelatedUrl";
            RationallyType = "relatedUrl";
            Index = index;

            AddAction("addRelatedFile", "QUEUEMARKEREVENT(\"addRelatedFile\")", "Add file", false);
            AddAction("addRelatedUrl", "QUEUEMARKEREVENT(\"addRelatedUrl\")", "Add url", false);
            AddAction("deleteRelatedDocument", "QUEUEMARKEREVENT(\"delete\")", "Delete document", false);

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
