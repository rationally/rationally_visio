using System.Windows.Forms;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal class AddAlternativeEventHandler : EventHandler
    {
        public AddAlternativeEventHandler(RModel model)
        {
            Selection selectedComponents = Globals.ThisAddIn.Application.ActiveWindow.Selection;
            foreach (Shape s in selectedComponents)
            {
                RComponent c = new RComponent(Globals.ThisAddIn.Application.ActivePage) {RShape = s};
                if (c.Type == "alternatives")
                {
                    AddAlternative alternative = new AddAlternative(model);
                    if (alternative.ShowDialog() == DialogResult.OK)
                    {
                        Alternative newAlternative = new Alternative(alternative.alternativeName.Text, alternative.alternativeStatus.SelectedItem.ToString(), "Enter a description here.");
                        model.Alternatives.Add(newAlternative);//TODO vuige code, fix me
                        Globals.ThisAddIn.View.AddAlternative(newAlternative);
                        //model.Alternatives.Last().Paint(s,0);
                    }
                    alternative.Dispose();
                }

            }
        }
    }
}
