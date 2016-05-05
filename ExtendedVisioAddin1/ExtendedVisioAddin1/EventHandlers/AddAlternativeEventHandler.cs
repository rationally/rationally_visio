using System.Linq;
using System.Windows.Forms;
using ExtendedVisioAddin1.Components;
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
                RComponent c = new RComponent(Globals.ThisAddIn.Application.ActivePage);
                c.RShape = s;
                if (c.Type == "alternatives")//TODO might be redundant
                {
                    AddAlternative alternative = new AddAlternative(model);
                    if (alternative.ShowDialog() == DialogResult.OK)
                    {
                        model.Alternatives.Add(new Alternative(alternative.alternativeName.Text, alternative.alternativeStatus.SelectedItem.ToString(), ""));
                        new RepaintHandler(model);
                        //model.Alternatives.Last().Paint(s,0);
                    }
                    alternative.Dispose();
                }
                //TODO remove lock msvSDContainerLocked
            }
        }
    }
}
