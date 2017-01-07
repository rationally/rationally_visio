using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Documents;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class RelatedDocumentTitleTextChangedEventHandler : ITextChangedEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(RationallyView view, Shape changedShape)
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                //find shape in view tree
                RelatedDocumentTitleComponent relatedDocumentTitle = (RelatedDocumentTitleComponent) Globals.RationallyAddIn.View.GetComponentByShape(changedShape);
                //locate connected model object
                RelatedDocument document = Globals.RationallyAddIn.Model.Documents[relatedDocumentTitle.DocumentIndex];
                //update the document name
                document.Name = relatedDocumentTitle.Text;
            }
        }
    }
}
