using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    class InformationAuthorTextChangedHandler : ITextChangedEventHandler
    {
        public void Execute(string eventKey, RView view, Shape changedShape)
        {
            Globals.RationallyAddIn.Model.Author = changedShape.Text;
        }
    }
}
