using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    class AlternativeTitleTextChangedEventHandler : ITextChangedEventHandler
    {
        public void Execute(string eventKey, RationallyView view, Shape changedShape)
        {
            RationallyComponent alternativeTitleComponent = new RationallyComponent(view.Page);
            alternativeTitleComponent.RShape = changedShape;
            Alternative alternativeToUpdate = Globals.RationallyAddIn.Model.Alternatives[alternativeTitleComponent.AlternativeIndex];
            alternativeToUpdate.Title = alternativeTitleComponent.Text;
        }
    }
}
