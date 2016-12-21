using System.Linq;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Stakeholders;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    class MoveDownStakeholderHandler : IMarkerEventHandler
    {
        public void Execute(Shape changedShape, string identifier)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            StakeholdersContainer stakeholdersContainer = (StakeholdersContainer)Globals.RationallyAddIn.View.Children.First(c => c is StakeholdersContainer);

            RationallyComponent toChangeComponent = Globals.RationallyAddIn.View.GetComponentByShape(changedShape);
            int currentIndex = toChangeComponent.StakeholderIndex;

            StakeholderContainer toChange = (StakeholderContainer)stakeholdersContainer.Children.First(c => c.StakeholderIndex == currentIndex);
            //locate the stakeholder that we are going to swap with
            StakeholderContainer other = (StakeholderContainer)stakeholdersContainer.Children.First(c => c.StakeholderIndex == currentIndex + 1);

            Stakeholder one = model.Stakeholders[currentIndex];
            model.Stakeholders[currentIndex] = model.Stakeholders[currentIndex + 1];
            model.Stakeholders[currentIndex + 1] = one;

            //update the index of the component and his children
            toChange.SetStakeholderIndex(currentIndex + 1);

            //same, for the other component
            other.SetStakeholderIndex(currentIndex);

            //swap the elements in the view tree
            RationallyComponent temp = stakeholdersContainer.Children[currentIndex];
            stakeholdersContainer.Children[currentIndex] = stakeholdersContainer.Children[currentIndex + 1];
            stakeholdersContainer.Children[currentIndex + 1] = temp;


            RepaintHandler.Repaint();
        }
    }
}
