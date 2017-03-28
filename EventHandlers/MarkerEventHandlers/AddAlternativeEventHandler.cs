using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.View.Alternatives;

// ReSharper disable ArrangeRedundantParentheses

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class AddAlternativeEventHandler : IMarkerEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(Shape s, string context)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            Log.Debug("Model found: " + (model != null));
            AlternativesContainer alternativesContainer = (AlternativesContainer)Globals.RationallyAddIn.View.Children.First(ch => ch is AlternativesContainer);
            Log.Debug("Alternatives container found in view tree: " + (alternativesContainer != null));
            Log.Debug("Alternatives count: " + model.Alternatives.Count);
            if (model.Alternatives.Count >= Constants.SupportedAmountOfAlternatives) //The view does not handling more than 3 alternatives well, by default.
            {
                AddAlternativeWithWarning alternativePopUp = new AddAlternativeWithWarning(model);
                if (alternativePopUp.ShowDialog() == DialogResult.OK)
                {
                    alternativesContainer?.AddAlternative(alternativePopUp.alternativeName.Text, alternativePopUp.alternativeStatus.SelectedItem.ToString());
                }
                alternativePopUp.Dispose();
            }
            else
            {
                AddAlternative alternativePopUp = new AddAlternative(model);
                if (alternativePopUp.ShowDialog() == DialogResult.OK)
                {
                    Log.Debug("About to enter AddAlternative method");
                    alternativesContainer?.AddAlternative(alternativePopUp.alternativeName.Text, alternativePopUp.alternativeStatus.SelectedItem.ToString());
                }
                alternativePopUp.Dispose();
            }
        }
    }
}
