﻿using System.Linq;
using System.Windows.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Documents;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class EditRelatedFileHandler : IMarkerEventHandler
    {

        public void Execute(RationallyModel model, Shape changedShape, string context)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                RationallyComponent comp = new RationallyComponent(Globals.RationallyAddIn.Application.ActivePage) { RShape = changedShape };
                int index = comp.DocumentIndex;

                //container of all related documents:
                RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer) Globals.RationallyAddIn.View.Children.First(c => c is RelatedDocumentsContainer);
                //find the the RelatedDocumentContainer of the selected file
                RelatedDocumentContainer documentContainer = (RelatedDocumentContainer) relatedDocumentsContainer.Children.First(f => f.DocumentIndex == index);

                RelatedDocument doc = model.Documents[index];
                doc.Name = openFileDialog.FileName;
                doc.Path = openFileDialog.FileName;
                documentContainer.EditFile(doc, index);
                RepaintHandler.Repaint(relatedDocumentsContainer);
            }
        }
    }
}