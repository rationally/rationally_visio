using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.View;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal abstract class QueryDeleteEventHandler
    {
        public abstract void Execute(string eventKey, RView view, Shape changedShape);
    }
}
