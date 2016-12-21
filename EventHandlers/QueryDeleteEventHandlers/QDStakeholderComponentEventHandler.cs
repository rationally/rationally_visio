using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;
using Rationally.Visio.View.Stakeholders;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDStakeholderComponentEventHandler : IQueryDeleteEventHandler
    {
        public void Execute(RationallyView view, Shape changedShape)
        {
            StakeholdersContainer cont = (StakeholdersContainer)view.Children.First(x => x is StakeholdersContainer);

            foreach (StakeholderContainer stakeholderContainer in cont.Children.Where(c => c is StakeholderContainer).Cast<StakeholderContainer>().ToList())
            {
                if ((stakeholderContainer.Children.Where(c => c.RShape.Equals(changedShape)).ToList().Count > 0) && !stakeholderContainer.Deleted) //check if this stakeholder contains the to be deleted component and is not already deleted
                {
                    stakeholderContainer.Deleted = true;
                    stakeholderContainer.RShape.Delete(); //delete the parent wrapper of s
                }
            }
        }
    }
}
