
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

        public RelatedUrlComponent(Page page, string url) : base(page)
        {

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Shapes\DecisionsStencil.vssx";
            Document rationallyDocument = Globals.ThisAddIn.Application.Documents.OpenEx(docPath, (short)VisOpenSaveArgs.visAddHidden);
            Master rectMaster = rationallyDocument.Masters["LinkIcon"]; 
            RShape = page.Drop(rectMaster, 0, 0);
            rationallyDocument.Close();

            Width = 0.6;
            Height = 0.6;
             //todo: create shappie properly

            Hyperlink link = RShape.AddHyperlink();
            link.Address = url;
            EventDblClick = "HYPERLINK(Hyperlink.Row_1.Address)";//"HYPERLINK(\"" + url + "\")";

            Name = "RelatedUrl";
            AddUserRow("rationallyType");
            RationallyType = "relatedUrl";

            //set the preview image of the url
            
            //RShape.ChangePicture(docPath, 0);
        }

        internal static bool IsRelatedUrlComponent(string name)
        {
            return RelatedRegex.IsMatch(name);
        }
    }
}
