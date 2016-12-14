﻿using System.Collections.Generic;
using System.Linq;
using Rationally.Visio.Forms;
using Rationally.Visio.Forms.WizardComponents;
using Rationally.Visio.Model;
using Rationally.Visio.View.Documents;
using static System.String;

namespace Rationally.Visio.EventHandlers.WizardPageHandlers
{
    internal class WizardUpdateDocumentsHandler
    {
        public static void Execute(ProjectSetupWizard wizard)
        {
            //validation is done here, so just pick the filled in rows
            List<FlowLayoutDocument> filledInPanels = wizard.TableLayoutMainContentDocuments.Documents.Where(doc => !IsNullOrEmpty(doc.FilePath.Text)).ToList();
            filledInPanels.ForEach(filledInPanel => filledInPanel.UpdateModel());

            //user might have deleted rows => delete them from the model
            List<RelatedDocument> modelDocuments = Globals.RationallyAddIn.Model.Documents;
            //locate documents in the model for which no element in the wizard exists (anymore)
            List<int> scheduledForDeletion = modelDocuments.Where((t, i) => wizard.TableLayoutMainContentDocuments.Documents.FirstOrDefault(doc => doc.DocumentIndex == i) == null).Select((modelDocument, i) => i).ToList();

            RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer)Globals.RationallyAddIn.View.Children.First(c => c is RelatedDocumentsContainer);

            //delete these elements from the view, which will automatically remove them from the model
            scheduledForDeletion.ForEach(docIndex => relatedDocumentsContainer.Children.Cast<RelatedDocumentContainer>().First(rdc => rdc.DocumentIndex == docIndex).RShape.Delete());

            //repaint the view according to the new model
            RepaintHandler.Repaint(relatedDocumentsContainer);
        }
    }
}
