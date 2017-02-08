using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;
using Rationally.Visio.View.Forces;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class ForceDescriptionTextChangedEventHandler : ITextChangedEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(RationallyView view, Shape changedShape)
        {
            ForceDescriptionComponent forceDescription = (ForceDescriptionComponent)view.GetComponentByShape(changedShape);
            if (forceDescription != null)
            {

                Globals.RationallyAddIn.Model.Forces[forceDescription.ForceIndex].Description = forceDescription.Text;
            }
        }
    }
}
