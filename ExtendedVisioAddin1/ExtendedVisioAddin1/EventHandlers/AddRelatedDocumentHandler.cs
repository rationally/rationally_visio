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
                //container of all related documents:
                RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer) Globals.ThisAddIn.View.Children.First(c => c.RShape.Equals(selectedShape));
                //create a container that wraps the new document
                RelatedDocumentContainer relatedDocumentContainer = new RelatedDocumentContainer(application.ActivePage);
                relatedDocumentsContainer.Children.Add(relatedDocumentContainer);
                //1) make a title component for the source and add it to the container
                RelatedDocumentTitleComponent relatedDocumentTitleComponent = new RelatedDocumentTitleComponent(application.ActivePage, openFileDialog.SafeFileName + ":");
                relatedDocumentContainer.Children.Add(relatedDocumentTitleComponent);
                //2) make a shortcut to the file
                RelatedFileComponent relatedFileComponent = new RelatedFileComponent(application.ActivePage, openFileDialog.FileName);
                relatedDocumentContainer.Children.Add(relatedFileComponent);
                

                new RepaintHandler();
            }


        }
    }
}
