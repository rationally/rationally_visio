using System.Windows.Forms;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal class RemoveAlternativeEventHandler : MarkerEventHandler
    {
        public override void Execute(RModel model, Shape s, string context)
        {
            
            RComponent c = new RComponent(Globals.ThisAddIn.Application.ActivePage) { RShape = s };

            int index = c.AlternativeIndex;
            Alternative alternative = model.Alternatives[index];
            DialogResult confirmResult = MessageBox.Show("Are you sure you want to delete " + alternative.Title, "Confirm Deletion", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                //model.Alternatives.Remove(alternative);//TODO might need to turn this on
                Globals.ThisAddIn.View.DeleteAlternative(index, true);
            }
            new RepaintHandler();
        }
    }
}
