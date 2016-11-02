using System.Windows.Forms;
using Rationally.Visio.Model;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Forms;
// ReSharper disable ArrangeRedundantParentheses

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class AddAlternativeEventHandler : IMarkerEventHandler
    {
        public void Execute(RationallyModel model, Shape s, string context)
        {
            if (model.Alternatives.Count >= 3) //The view does not handling more than 3 alternatives well, by default.
            {
                AddAlternativeWithWarning alternative = new AddAlternativeWithWarning(model);
                if (alternative.ShowDialog() == DialogResult.OK)
                {
                    Alternative newAlternative = new Alternative(alternative.alternativeName.Text, alternative.alternativeStatus.SelectedItem.ToString());
                    newAlternative.GenerateIdentifier(model.Alternatives.Count);
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
                    Alternative newAlternative = new Alternative(alternative.alternativeName.Text, alternative.alternativeStatus.SelectedItem.ToString());
                    newAlternative.GenerateIdentifier(model.Alternatives.Count);
                    model.Alternatives.Add(newAlternative);
                    Globals.RationallyAddIn.View.AddAlternative(newAlternative);
                }
                alternative.Dispose();
            }
        }
    }
}
