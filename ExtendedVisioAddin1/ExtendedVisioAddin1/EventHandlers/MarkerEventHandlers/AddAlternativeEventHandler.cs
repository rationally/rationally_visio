using System.Windows.Forms;
using Rationally.Visio.Model;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.WindowsFormPopups;
// ReSharper disable ArrangeRedundantParentheses

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class AddAlternativeEventHandler : IMarkerEventHandler
    {
        public void Execute(RModel model, Shape s, string context)
        {
            if (model.Alternatives.Count >= 3) //The view does not handling more than 3 alternatives well, by default.
            {
                AddAlternativeWithWarning alternative = new AddAlternativeWithWarning(model);
                if (alternative.ShowDialog() == DialogResult.OK)
                {
                    string identifier = (char)(65 + model.Alternatives.Count) + ":";
                    int timelessId = Alternative.HighestUniqueIdentifier == -1 ? 0 : (Alternative.HighestUniqueIdentifier + 1);
                    Alternative newAlternative = new Alternative(alternative.alternativeName.Text, alternative.alternativeStatus.SelectedItem.ToString(), "Enter a description here.", identifier, timelessId);
                    model.Alternatives.Add(newAlternative);
                    Globals.RationallyAddIn.View.AddAlternative(newAlternative);
                }
                alternative.Dispose();
            }
            else
            {
                AddAlternative alternative = new AddAlternative(model);
                if (alternative.ShowDialog() == DialogResult.OK)
                {
                    string identifier = (char)(65 + model.Alternatives.Count) + ":";
                    int timelessId = Alternative.HighestUniqueIdentifier == -1 ? 0 : (Alternative.HighestUniqueIdentifier + 1);
                    Alternative newAlternative = new Alternative(alternative.alternativeName.Text, alternative.alternativeStatus.SelectedItem.ToString(), "Enter a description here.", identifier, timelessId);
                    model.Alternatives.Add(newAlternative);
                    Globals.RationallyAddIn.View.AddAlternative(newAlternative);
                }
                alternative.Dispose();
            }
        }
    }
}
