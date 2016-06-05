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
                double offsetHeight =Globals.ThisAddIn.Application.ActivePage.PageSheet.CellsU["PageHeight"].Result[VisUnitCodes.visInches] - 0.4;
                //draw the header
                TextLabel header = new TextLabel(Globals.ThisAddIn.Application.ActivePage,model.DecisionName);

                header.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded | SizingPolicy.ExpandYIfNeeded | SizingPolicy.ShrinkYIfNeeded);
                header.Width = 7;
                header.Height = 1;
                header.SetFontSize(22);
                header.CenterX = 3.5;
                header.CenterY = offsetHeight;

                //draw the information container
                InformationContainer informationContainer = new InformationContainer(Globals.ThisAddIn.Application.ActivePage, model.Author, model.Date, model.Version)
                    {
                        CenterX = 9,
                        CenterY = offsetHeight
                    };
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
