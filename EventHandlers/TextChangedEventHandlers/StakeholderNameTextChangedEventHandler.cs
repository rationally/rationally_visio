using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    class StakeholderNameTextChangedEventHandler : ITextChangedEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyView view, Shape changedShape)
        {
            RationallyComponent stakeholderNameComponent = new RationallyComponent(view.Page) { RShape = changedShape };

            if (Globals.RationallyAddIn.Model.Stakeholders.Count <= stakeholderNameComponent.StakeholderIndex) { return; }

            Stakeholder toUpdate = Globals.RationallyAddIn.Model.Stakeholders[stakeholderNameComponent.StakeholderIndex];
            toUpdate.Name = stakeholderNameComponent.Text;
        }
    }
}
