using System.Windows.Forms;
using Rationally.Visio.Model;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Forms;
using Rationally.Visio.RationallyConstants;

// ReSharper disable ArrangeRedundantParentheses

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class AddAlternativeEventHandler : IMarkerEventHandler
    {
        public void Execute(RationallyModel model, Shape s, string context)
        {
            if (model.Alternatives.Count >= Constants.SupportedAmountOfAlternatives) //The view does not handling more than 3 alternatives well, by default.
            {
                AddAlternativeWithWarning alternativePopUp = new AddAlternativeWithWarning(model);
                if (alternativePopUp.ShowDialog() == DialogResult.OK)
                {
                    int timelessId = Alternative.HighestUniqueIdentifier == -1 ? 0 : (Alternative.HighestUniqueIdentifier + 1);
                    Alternative newAlternative = new Alternative(alternativePopUp.alternativeName.Text, alternativePopUp.alternativeStatus.SelectedItem.ToString(), timelessId);
                    newAlternative.GenerateIdentifier(model.Alternatives.Count);
                    model.Alternatives.Add(newAlternative);
                    Globals.RationallyAddIn.View.AddAlternative(newAlternative);
                }
                alternativePopUp.Dispose();
            }
            else
            {
                AddAlternative alternativePopUp = new AddAlternative(model);
                if (alternativePopUp.ShowDialog() == DialogResult.OK)
                {
                    int timelessId = Alternative.HighestUniqueIdentifier == -1 ? 0 : (Alternative.HighestUniqueIdentifier + 1);
                    Alternative newAlternative = new Alternative(alternativePopUp.alternativeName.Text, alternativePopUp.alternativeStatus.SelectedItem.ToString(), timelessId);
                    newAlternative.GenerateIdentifier(model.Alternatives.Count);
                    model.Alternatives.Add(newAlternative);
                    Globals.RationallyAddIn.View.AddAlternative(newAlternative);
                }
                alternativePopUp.Dispose();
            }
        }
    }
}
