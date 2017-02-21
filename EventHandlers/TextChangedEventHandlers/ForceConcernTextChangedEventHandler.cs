using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;
using Rationally.Visio.View.Forces;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class ForceConcernTextChangedEventHandler : ITextChangedEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(RationallyView view, Shape changedShape)
        {
            ForceConcernComponent forceConcern = (ForceConcernComponent)view.GetComponentByShape(changedShape);
            if (forceConcern != null)
            {
                if (forceConcern.Text == string.Empty)
                {
                    forceConcern.Text = ForceConcernComponent.DefaultConcern;
                }
                Globals.RationallyAddIn.Model.Forces[forceConcern.Index].Concern = forceConcern.Text;
            }
        }
    }
}
