﻿using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;
using Rationally.Visio.View.Documents;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDRelatedDocumentContainerEventHandler : IQueryDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(RationallyView view, Shape changedShape)
        {
            VisioShape comp = view.Children.Find(x => x is RelatedDocumentsContainer);
            if (comp is RelatedDocumentsContainer)
            {
                comp.MsvSdContainerLocked = false; //Child shapes can now be removed.
            }
        }
    }
}
