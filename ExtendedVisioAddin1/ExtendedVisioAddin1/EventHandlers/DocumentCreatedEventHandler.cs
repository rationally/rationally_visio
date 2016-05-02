using Microsoft.Office.Interop.Visio;
using rationally_visio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExtendedVisioAddin1.Components;
using ExtendedVisioAddin1.Model;

namespace ExtendedVisioAddin1.EventHandlers
{
    class DocumentCreatedEventHandler
    {
        private RModel model;

        public DocumentCreatedEventHandler(IVDocument document, RModel model)
        {
            this.model = model;
            if (document.Template.ToLower().Contains("rationally"))
            {
                ShowMyDialogBox();
                double offsetHeight =Globals.ThisAddIn.Application.ActivePage.PageSheet.CellsU["PageHeight"].Result[VisUnitCodes.visInches] - 0.4;
                //draw the header
                TextLabel header = new TextLabel(Globals.ThisAddIn.Application.ActivePage,model.DecisionName);
                header.SetFontSize(22);
                header.Draw(1, offsetHeight);

                //draw the information container
                InformationContainer informationContainer = new InformationContainer(model.Author, model.Date, model.Version);
                informationContainer.Draw(Globals.ThisAddIn.Application.ActivePage.PageSheet.CellsU["PageWidth"].Result[VisUnitCodes.visInches] - 8, offsetHeight);
            }
        }

        private void ShowMyDialogBox()
        {
            SheetSetUp testDialog = new SheetSetUp();

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
