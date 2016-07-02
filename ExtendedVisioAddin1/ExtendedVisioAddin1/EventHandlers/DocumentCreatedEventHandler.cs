using System.Windows.Forms;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal class DocumentCreatedEventHandler
    {
        private readonly RModel model;

        public DocumentCreatedEventHandler(IVDocument document, RModel model)
        {
            this.model = model;
            if (document.Template.ToLower().Contains("rationally"))
            {
                ShowSheetSetUpBox();
                //draw the header
                TextLabel header = new TextLabel(Globals.ThisAddIn.Application.ActivePage,model.DecisionName);

                header.SetUsedSizingPolicy(SizingPolicy.FixedSize);
                header.HAlign = 0;//left
                header.Width = 10.5;
                header.Height = 0.3056;
                header.SetFontSize(22);
                header.CenterX = 5.5;
                header.CenterY = 22.483;

                //draw the information container
                InformationContainer informationContainer = new InformationContainer(Globals.ThisAddIn.Application.ActivePage, model.Author, model.Date, model.Version);
                new RepaintHandler(informationContainer);
            }
        }

        private void ShowSheetSetUpBox()
        {
            SheetSetUpFormPopUp testDialog = new SheetSetUpFormPopUp();

            model.Author = "";
            model.DecisionName ="";
            model.Date = "";
            model.Version = "";

            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            if (testDialog.ShowDialog() == DialogResult.OK)
            {
                // Read the contents of testDialog's TextBox.
                model.Author = testDialog.author.Text;
                model.DecisionName = testDialog.decisionName.Text;
                model.Date = testDialog.date.Text;
                model.Version = testDialog.version.Text;
            }
            testDialog.Dispose();
        }
    }
}
