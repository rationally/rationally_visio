using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExtendedVisioAddin1.View;
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
            OpenFileDialog selectUrlDialog = new OpenFileDialog();
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

                RelatedDocumentsContainer relatedDocumentsContainer =
                    (RelatedDocumentsContainer)
                        Globals.ThisAddIn.View.Children.First(c => c.RShape.Equals(selectedShape));

                RelatedUrlComponent relatedFileComponent = new RelatedUrlComponent(application.ActivePage, "");

                relatedDocumentsContainer.Children.Add(relatedFileComponent);

                new RepaintHandler();
            }
        }
    }
}
