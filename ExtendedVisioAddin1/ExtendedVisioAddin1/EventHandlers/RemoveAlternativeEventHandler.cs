using System.Windows.Forms;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal class RemoveAlternativeEventHandler : EventHandler
    {
        public RemoveAlternativeEventHandler(RModel model)
        {
            Selection selectedComponents = Globals.ThisAddIn.Application.ActiveWindow.Selection;
            foreach (Shape s in selectedComponents)
            {
                RComponent c = new RComponent(Globals.ThisAddIn.Application.ActivePage) {RShape = s};
                if (c.Type == "alternative")
                {
                    int index = c.AlternativeIndex;
                    Alternative alternative = model.Alternatives[index];
                    DialogResult confirmResult = MessageBox.Show("Are you sure you want to delete " + alternative.Title, "Confirm Deletion", MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        model.Alternatives.Remove(alternative);
                        Globals.ThisAddIn.View.DeleteAlternative(index);
                    }
                }
            }
        }
    }
}
