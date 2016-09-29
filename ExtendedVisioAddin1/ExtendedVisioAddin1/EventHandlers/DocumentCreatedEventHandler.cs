using System.Windows.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers
{
    internal class DocumentCreatedEventHandler
    {
        private readonly RModel model;

        public DocumentCreatedEventHandler(IVDocument document, RModel model)
        {
            this.model = model;
            if (document.Template.Contains(Rationally.Visio.ThisAddIn.TemplateName))
            {
                ShowSheetSetUpBox();
                //draw the header
                TextLabel header = new TextLabel(Globals.ThisAddIn.Application.ActivePage,model.DecisionName);

                header.SetUsedSizingPolicy(SizingPolicy.FixedSize);
                header.HAlign = 0;//left, since the enum is wrong
                header.Width = 10.5;
                header.Height = 0.3056;
                header.SetFontSize(22);
                header.CenterX = 5.5;
                header.CenterY = 22.483;

                //draw the information container
                InformationContainer informationContainer = new InformationContainer(Globals.ThisAddIn.Application.ActivePage, model.Author, model.Date, model.Version);
                RepaintHandler.Repaint(informationContainer);
            }
        }

        private void ShowSheetSetUpBox()
        {
            SheetSetUpFormPopUp setupDialog = new SheetSetUpFormPopUp();

            model.Author = "";
            model.DecisionName ="";
            model.Date = "";
            model.Version = "";

            // Show setupDialog as a modal dialog and determine if DialogResult = OK.
            if (setupDialog.ShowDialog() == DialogResult.OK)
            {
                // Read the contents of setupDialog's TextBox.
                model.Author = setupDialog.author.Text;
                model.DecisionName = setupDialog.decisionName.Text;
                model.Date = setupDialog.date.Text;
                model.Version = setupDialog.version.Text;
            }
            setupDialog.Dispose();
        }
    }
}
