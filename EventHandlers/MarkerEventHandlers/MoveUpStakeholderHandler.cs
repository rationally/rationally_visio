using System.Linq;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.View;
using Rationally.Visio.View.Stakeholders;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    class MoveUpStakeholderHandler : IMarkerEventHandler
    {
        public void Execute(Shape changedShape, string identifier)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            //locate the stakeholder(component) to move
            RationallyComponent toChangeComponent = Globals.RationallyAddIn.View.GetComponentByShape(changedShape);
            int currentIndex = toChangeComponent.StakeholderIndex;
            //locate the stakeholder to swap with
            StakeholdersContainer stakeholdersContainer = (StakeholdersContainer)Globals.RationallyAddIn.View.Children.First(c => c is StakeholdersContainer);
            StakeholderContainer toChange = (StakeholderContainer)stakeholdersContainer.Children.First(c => (int)c.RShape.CellsU[CellConstants.StakeholderIndex].ResultIU == currentIndex);
            StakeholderContainer other = (StakeholderContainer)stakeholdersContainer.Children.First(c => (int)c.RShape.CellsU[CellConstants.StakeholderIndex].ResultIU == currentIndex - 1);

            //swap
            Stakeholder one = model.Stakeholders[currentIndex];
            model.Stakeholders[currentIndex] = model.Stakeholders[currentIndex - 1];
            model.Stakeholders[currentIndex - 1] = one;

            //update the index of the component and his children
            toChange.SetStakeholderIndex(currentIndex - 1);
            //same, for the other component
            other.SetStakeholderIndex(currentIndex);
            //swap the elements
            RationallyComponent temp = stakeholdersContainer.Children[currentIndex];
            stakeholdersContainer.Children[currentIndex] = stakeholdersContainer.Children[currentIndex - 1];
            stakeholdersContainer.Children[currentIndex - 1] = temp;


            RepaintHandler.Repaint();
        }
    }
}
