﻿using System.Linq;
using System.Windows.Forms;
using ExtendedVisioAddin1.View.Documents;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal class AddRelatedUrlHandler
    {
        public AddRelatedUrlHandler()
        {
            //TODO: popUp and validation? etc
            var application = Globals.ThisAddIn.Application;
            UrlSelecter selectUrlDialog = new UrlSelecter();


            IVShape selectedShape = null; 
            if (selectUrlDialog.ShowDialog() == DialogResult.OK)
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
                RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer)Globals.ThisAddIn.View.Children.First(c => c.RShape.Equals(selectedShape));
                //create a container that wraps the new document
                RelatedDocumentContainer relatedDocumentContainer = new RelatedDocumentContainer(application.ActivePage);
                relatedDocumentsContainer.Children.Add(relatedDocumentContainer);
                //1) make a title component for the source and add it to the container
                RelatedDocumentTitleComponent relatedDocumentTitleComponent = new RelatedDocumentTitleComponent(application.ActivePage, selectUrlDialog.nameTextbox.Text + ":");
                relatedDocumentContainer.Children.Add(relatedDocumentTitleComponent);
                //2) make a shortcut to the file
                RelatedUrlComponent relatedUrlComponent = new RelatedUrlComponent(application.ActivePage, selectUrlDialog.urlTextBox.Text);
                relatedDocumentContainer.Children.Add(relatedUrlComponent);

                new RepaintHandler();
            }
            selectUrlDialog.Dispose();
        }
    }
}