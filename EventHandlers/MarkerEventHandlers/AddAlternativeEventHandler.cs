using System.Linq;
using System.Windows.Forms;
using Rationally.Visio.Model;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Forms;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.View.Alternatives;

// ReSharper disable ArrangeRedundantParentheses

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class AddAlternativeEventHandler : IMarkerEventHandler
    {
        public void Execute(Shape s, string context)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            AlternativesContainer alternativesContainer = (AlternativesContainer)Globals.RationallyAddIn.View.Children.First(ch => ch is AlternativesContainer);
            if (model.Alternatives.Count >= Constants.SupportedAmountOfAlternatives) //The view does not handling more than 3 alternatives well, by default.
            {
                AddAlternativeWithWarning alternativePopUp = new AddAlternativeWithWarning(model);
                if (alternativePopUp.ShowDialog() == DialogResult.OK)
                {
                    Alternative newAlternative = new Alternative(alternativePopUp.alternativeName.Text, alternativePopUp.alternativeStatus.SelectedItem.ToString());
                    newAlternative.GenerateIdentifier(model.Alternatives.Count);
                    model.Alternatives.Add(newAlternative);

                    alternativesContainer?.AddAlternative(newAlternative);
                }
                alternativePopUp.Dispose();
            }
            else
            {
                AddAlternative alternativePopUp = new AddAlternative(model);
                if (alternativePopUp.ShowDialog() == DialogResult.OK)
                {
                    Alternative newAlternative = new Alternative(alternativePopUp.alternativeName.Text, alternativePopUp.alternativeStatus.SelectedItem.ToString());
                    newAlternative.GenerateIdentifier(model.Alternatives.Count);
                    model.Alternatives.Add(newAlternative);
                    alternativesContainer?.AddAlternative(newAlternative);
                }
                alternativePopUp.Dispose();
            }
        }
    }
}
