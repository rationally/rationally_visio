﻿using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class StakeholderRoleTextChangedEventHandler : ITextChangedEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyView view, Shape changedShape)
        {
            VisioShape stakeholderRoleComponent = new VisioShape(view.Page) { Shape = changedShape };

            if (Globals.RationallyAddIn.Model.Stakeholders.Count <= stakeholderRoleComponent.Index) { return; }

            Stakeholder toUpdate = Globals.RationallyAddIn.Model.Stakeholders[stakeholderRoleComponent.Index];
            toUpdate.Role = stakeholderRoleComponent.Text;
        }
    }
}
