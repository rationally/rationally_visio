
using System;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    internal class RelatedUrlComponent : RComponent
    {
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
            EventDblClick = "HYPERLINK(\"" + url + "\")";

            Name = "RelatedUrl";
            AddUserRow("rationallyType");
            RationallyType = "relatedUrl";

            //set the preview image of the url
            
            //RShape.ChangePicture(docPath, 0);
        }
    }
}
