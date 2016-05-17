using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExtendedVisioAddin1.View.Documents;
using Microsoft.Office.Interop.Visio;
using Application = Microsoft.Office.Interop.Visio.Application;

namespace ExtendedVisioAddin1.EventHandlers
{
    class EditRelatedFileHandler
    {
        public EditRelatedFileHandler()
        {
            Application application = Globals.ThisAddIn.Application;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true
            };
            IVShape selectedShape = null;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //find the shape on which the edit action was performed
                foreach (IVShape s in application.ActiveWindow.Selection)
                {
                    if (s.Name.Contains("RelatedFile"))
                    {
                        selectedShape = s;
                        break;
                    }
                }
                selectedShape.InsertFromFile(openFileDialog.FileName, (short)VisInsertObjArgs.visInsertLink | (short)VisInsertObjArgs.visInsertIcon);

                /*//container of all related documents:
                RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer)Globals.ThisAddIn.View.Children.First(c => c.RShape.Equals(selectedShape));
                //find the the RelatedDocumentContainer of the selected file
                RelatedDocumentContainer relatedDocumentContainer  = (RelatedDocumentContainer)relatedDocumentsContainer.Children.First(f => ((RelatedDocumentContainer) f).Children.Where(c => c.RShape == selectedShape).ToList().Count > 0);
                //update the shape of the relatedDocumentContainer link to point to the new file
                relatedDocumentContainer.Children.First(c => c is RelatedFileComponent).RShape.
                //1) make a title component for the source and add it to the container
                RelatedDocumentTitleComponent relatedDocumentTitleComponent = new RelatedDocumentTitleComponent(application.ActivePage, openFileDialog.SafeFileName + ":");
                relatedDocumentContainer.Children.Add(relatedDocumentTitleComponent);
                //2) make a shortcut to the file
                RelatedFileComponent relatedFileComponent = new RelatedFileComponent(application.ActivePage, openFileDialog.FileName);
                relatedDocumentContainer.Children.Add(relatedFileComponent);*/


                new RepaintHandler();
            }
        }
    }
}
