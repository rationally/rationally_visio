
using System;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    internal class RelatedUrlComponent : RComponent
    {
        private static readonly Regex RelatedRegex = new Regex(@"RelatedUrl(\.\d+)?$");
        public RelatedUrlComponent(Page page, Shape urlShape) : base(page)
        {
            RShape = urlShape;
        }

        public RelatedUrlComponent(Page page, int index, string url) : base(page)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Shapes\RationallyHidden.vssx";
            Document rationallyDocument = Globals.ThisAddIn.Application.Documents.OpenEx(docPath, (short)VisOpenSaveArgs.visAddHidden); //todo: handling for file is open
            Master rectMaster = rationallyDocument.Masters["LinkIcon"]; 
            RShape = page.Drop(rectMaster, 0, 0);
             //todo: create shappie properly

            Hyperlink link = RShape.AddHyperlink();
            link.Address = url;
            EventDblClick = "HYPERLINK(Hyperlink.Row_1.Address)";

            InitStyle();

            Name = "RelatedUrl";
            AddUserRow("rationallyType");
            RationallyType = "relatedUrl";
            AddUserRow("documentIndex");
            DocumentIndex = index;

            rationallyDocument.Close();
        }

        private void InitStyle()
        {
            Width = 0.6;
            Height = 0.6;
            SetMargin(0.1);
        }

        internal static bool IsRelatedUrlComponent(string name)
        {
            return RelatedRegex.IsMatch(name);
        }
    }
}
