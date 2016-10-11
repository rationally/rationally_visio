using System.Windows.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.WindowsFormPopups;

namespace Rationally.Visio.EventHandlers
{
    internal class DocumentCreatedEventHandler
    {
        private readonly RModel model;

        public DocumentCreatedEventHandler(IVDocument document, RModel model)
        {
            this.model = model;
            if (document.Template.Contains(Constants.TemplateName))
            {
                ShowSetupWizard();
                
            }
        }

        private void ShowSetupWizard()
        {
            ProjectSetupWizard test = new ProjectSetupWizard();
            test.Show();
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
