using System.Windows.Forms;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.MarkerEventHandlers
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
                    int timelessId = Alternative.HighestTimelessId == -1 ? 0 : (Alternative.HighestTimelessId + 1);
                    Alternative newAlternative = new Alternative(alternative.alternativeName.Text, alternative.alternativeStatus.SelectedItem.ToString(), "Enter a description here.", identifier, timelessId);
                    model.Alternatives.Add(newAlternative);
                    Globals.ThisAddIn.View.AddAlternative(newAlternative);
                }
                alternative.Dispose();
            }
            else
            {
                AddAlternative alternative = new AddAlternative(model);
                if (alternative.ShowDialog() == DialogResult.OK)
                {
                    string identifier = (char)(65 + model.Alternatives.Count) + ":";
                    int timelessId = Alternative.HighestTimelessId == -1 ? 0 : (Alternative.HighestTimelessId + 1);
                    Alternative newAlternative = new Alternative(alternative.alternativeName.Text, alternative.alternativeStatus.SelectedItem.ToString(), "Enter a description here.", identifier, timelessId);
                    model.Alternatives.Add(newAlternative);
                    Globals.ThisAddIn.View.AddAlternative(newAlternative);
                }
                alternative.Dispose();
            }
        }
    }
}
