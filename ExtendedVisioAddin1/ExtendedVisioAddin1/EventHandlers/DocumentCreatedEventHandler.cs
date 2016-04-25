using Microsoft.Office.Interop.Visio;
using rationally_visio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
            }
        }

        private void ShowMyDialogBox()
        {
            SheetSetUp testDialog = new SheetSetUp();

            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            if (testDialog.ShowDialog() == DialogResult.OK)
            {
                // Read the contents of testDialog's TextBox.
                model.Author = testDialog.textBoxAuthor.Text ?? "";
                model.DecisionName = testDialog.textBoxName.Text ?? "";
                model.Date = testDialog.textBoxHeader.Text ?? "";
                model.Version = testDialog.textBoxHeader.Text ?? "";//TODO other reading fields
            }
            testDialog.Dispose();
        }
    }
}
