using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Stakeholders;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    class StartDeleteStakeholderEventHandler : IMarkerEventHandler
    {
        public void Execute(Shape s, string identifier)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            RationallyComponent component = new RationallyComponent(Globals.RationallyAddIn.Application.ActivePage) { RShape = s };

            int index = component.StakeholderIndex;
            Stakeholder stakeholder = model.Stakeholders[index];
            DialogResult confirmResult = MessageBox.Show("Are you sure you want to remove " + stakeholder.Name + "?", "Confirm Deletion", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                Shape shapeToPass;

                if (StakeholderContainer.IsStakeholderContainer(s.Name))
                {
                    shapeToPass = s;
                }
                else //subpart of stakeholder container
                {
                    //trace stakeholders container
                    StakeholdersContainer stakeholdersContainer = (StakeholdersContainer)Globals.RationallyAddIn.View.Children.First(c => c is StakeholdersContainer);
                    //trace the correct stakeholder container
                    StakeholderContainer stakeholderContainer = (StakeholderContainer)stakeholdersContainer.Children.First(c => c is StakeholderContainer && (component.StakeholderIndex == c.StakeholderIndex));

                    shapeToPass = stakeholderContainer.RShape;
                }
                //initiate a delete handler with the container's shape
                shapeToPass.Delete();
            }
        }
    }
}
