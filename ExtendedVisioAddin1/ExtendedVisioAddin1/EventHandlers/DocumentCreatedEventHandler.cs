using System.Windows.Forms;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Documents;
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
                ShowMyDialogBox();
                double offsetHeight =Globals.ThisAddIn.Application.ActivePage.PageSheet.CellsU["PageHeight"].Result[VisUnitCodes.visInches] - 0.4;
                //draw the header
                TextLabel header = new TextLabel(Globals.ThisAddIn.Application.ActivePage,model.DecisionName);
                header.SetFontSize(22);
                header.CenterX = 1;
                header.CenterY = offsetHeight;
                Globals.ThisAddIn.View.Children.Add(header);

                //draw the information container
                InformationContainer informationContainer = new InformationContainer(Globals.ThisAddIn.Application.ActivePage, model.Author, model.Date, model.Version)
                    {
                        CenterX = 12,
                        CenterY = offsetHeight
                    };
                //(Globals.ThisAddIn.Application.ActivePage.PageSheet.CellsU["PageWidth"].Result[VisUnitCodes.visInches]);
                Globals.ThisAddIn.View.Children.Add(informationContainer); 

                //Globals.ThisAddIn.View.Children.Add(new AlternativesContainer(Globals.ThisAddIn.Application.ActivePage, model.Alternatives.ToList()));
              
              //new RepaintHandler(model);
                var a = new RelatedDocumentsContainer(Globals.ThisAddIn.Application.ActivePage);
                //a.Children.Add(new RelatedFileComponent(Globals.ThisAddIn.Application.ActivePage, @"C:\Users\Ruben\Desktop\QoSA.pdf"));
              Globals.ThisAddIn.View.Children.Add(a);
                new RepaintHandler();
            }
        }

        private void ShowMyDialogBox()
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
