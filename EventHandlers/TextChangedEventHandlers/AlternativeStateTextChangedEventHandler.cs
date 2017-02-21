using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class AlternativeStateTextChangedEventHandler : ITextChangedEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(RationallyView view, Shape changedShape)
        {
            AlternativeStateComponent alternativeState = (AlternativeStateComponent)view.GetComponentByShape(changedShape);
            if (alternativeState == null) { return;}

            int index = alternativeState.Index;
            Globals.RationallyAddIn.Model.Alternatives[index].Status = alternativeState.Text;
        }
    }
}
