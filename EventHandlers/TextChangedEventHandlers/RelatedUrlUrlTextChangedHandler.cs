using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Documents;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    public class RelatedUrlUrlTextChangedHandler : ITextChangedEventHandler
    {
        public void Execute(RationallyView view, Shape changedShape)
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                //find shape in view tree
                RelatedURLURLComponent urlUrl = (RelatedURLURLComponent) Globals.RationallyAddIn.View.GetComponentByShape(changedShape);
                //locate connected model object
                RelatedDocument document = Globals.RationallyAddIn.Model.Documents[urlUrl.DocumentIndex];
                //update the url value
                document.Path = urlUrl.Text;
            }
        }
    }
}
