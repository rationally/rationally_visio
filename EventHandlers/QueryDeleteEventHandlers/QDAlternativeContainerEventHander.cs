﻿using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDAlternativeContainerEventHander : IQueryDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(RationallyView view, Shape changedShape)
        {
            VisioShape comp = view.Children.Find(x => x is AlternativesContainer);
            if (comp is AlternativesContainer)
            {
                comp.MsvSdContainerLocked = false;
            }
        }
    }
}
