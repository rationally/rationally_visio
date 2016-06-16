using System.Windows.Forms;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal class AddAlternativeEventHandler : MarkerEventHandler
    {
        public override void Execute(RModel model, Shape s, string context)
        {
            if (model.Alternatives.Count >= 3)
            {
                MessageBox.Show("Adding more than three alternative solutions may have a negative effect on the layout.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            AddAlternative alternative = new AddAlternative(model);
            if (alternative.ShowDialog() == DialogResult.OK)
            {
                string identifier = (char)(65 + model.Alternatives.Count) + ":";
                Alternative newAlternative = new Alternative(alternative.alternativeName.Text, alternative.alternativeStatus.SelectedItem.ToString(), "Enter a description here.", identifier);
                model.Alternatives.Add(newAlternative);
                Globals.ThisAddIn.View.AddAlternative(newAlternative);
            }
            alternative.Dispose();

        }
    }
}
