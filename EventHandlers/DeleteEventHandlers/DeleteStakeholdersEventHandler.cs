using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Logger;
using Rationally.Visio.Model;
using Rationally.Visio.View.Alternatives;
using Rationally.Visio.View.Stakeholders;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    class DeleteStakeholdersEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape changedShape)
        {
            Globals.RationallyAddIn.View.Children.RemoveAll(obj => obj.RShape.Equals(changedShape));
            TempFileLogger.Log("Handler of delete stakeholders entered.");
            if (!Globals.RationallyAddIn.View.Children.Any(x => x is StakeholdersContainer))
            {
                model.Stakeholders.Clear();
                TempFileLogger.Log("model stakeholders list emptied.");
                RepaintHandler.Repaint();
            }
        }
    }
}
