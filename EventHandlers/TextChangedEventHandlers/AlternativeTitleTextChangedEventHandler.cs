using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class AlternativeTitleTextChangedEventHandler : ITextChangedEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(RationallyView view, Shape changedShape)
        {
            RationallyComponent alternativeTitleComponent = new RationallyComponent(view.Page) {RShape = changedShape};

            if (Globals.RationallyAddIn.Model.Alternatives.Count <= alternativeTitleComponent.Index) { return;}

            Alternative alternativeToUpdate = Globals.RationallyAddIn.Model.Alternatives[alternativeTitleComponent.Index];
            alternativeToUpdate.Title = alternativeTitleComponent.Text;
        }
    }
}
