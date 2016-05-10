using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Documents;
using Microsoft.Office.Interop.Visio;
using Shape = Microsoft.Office.Core.Shape;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal class AddRelatedDocumentHandler : EventHandler
    {
        public AddRelatedDocumentHandler()
        {
            var application = Globals.ThisAddIn.Application;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            IVShape selectedShape = null;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (IVShape s in application.ActiveWindow.Selection)
                {
                    if (s.Name.Contains("Related Documents")) //TODO regex
                    {
                        selectedShape = s;
                        break;
                    }
                }

                RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer) Globals.ThisAddIn.View.Children.First(c => c.RShape.Equals(selectedShape));

                RelatedFileComponent relatedFileComponent = new RelatedFileComponent(application.ActivePage, openFileDialog.FileName);

                relatedDocumentsContainer.Children.Add(relatedFileComponent);

                new RepaintHandler();
            }


        }
    }
}
