using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View.Documents;
using Microsoft.Office.Interop.Visio;
using Application = Microsoft.Office.Interop.Visio.Application;

namespace ExtendedVisioAddin1.EventHandlers
{
    class EditRelatedFileHandler : MarkerEventHandler
    {

        public override void Execute(RModel model, Shape changedShape, string context)
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
                ThisAddIn.PreventAddEvent = true;
                //container of all related documents:
                RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer) Globals.ThisAddIn.View.Children.First(c => c is RelatedDocumentsContainer);
                //find the the RelatedDocumentContainer of the selected file
                RelatedDocumentContainer oldRelatedDocumentContainer = (RelatedDocumentContainer) relatedDocumentsContainer.Children.First(f => ((RelatedDocumentContainer) f).Children.Where(c => c.RShape == selectedShape).ToList().Count > 0);
                //find out the index, so we can insert the new file on the right spot
                int insertIndex = relatedDocumentsContainer.Children.IndexOf(oldRelatedDocumentContainer);
                oldRelatedDocumentContainer.Children.First().RShape.Delete(); //this will also trigger the deletion of its child components (thanks to an eventhandler)


                //create a container that wraps the new document
                RelatedDocumentContainer relatedDocumentContainer = new RelatedDocumentContainer(application.ActivePage);
                relatedDocumentsContainer.Children.Insert(insertIndex, relatedDocumentContainer); //insert at the original index
                //1) make a title component for the source and add it to the container
                RelatedDocumentTitleComponent relatedDocumentTitleComponent = new RelatedDocumentTitleComponent(application.ActivePage, openFileDialog.FileName + ":");
                relatedDocumentContainer.Children.Add(relatedDocumentTitleComponent);
                //2) make a shortcut to the file
                RelatedFileComponent relatedFileComponent = new RelatedFileComponent(application.ActivePage, openFileDialog.FileName);
                relatedDocumentContainer.Children.Add(relatedFileComponent);


                new RepaintHandler();
                ThisAddIn.PreventAddEvent = false;
            }
        }
    }
}
