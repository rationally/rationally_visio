using System.Linq;
using System.Windows.Forms;
using ExtendedVisioAddin1.Components;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal class AddAlternativeEventHandler : EventHandler
    {
        public AddAlternativeEventHandler(RModel model)
        {
            Selection selectedComponents = Globals.ThisAddIn.Application.ActiveWindow.Selection;
            foreach (IVShape s in selectedComponents)
            {
                RationallyComponent c = new RationallyComponent(s);
                if (c.Type == "alternatives")//TODO might be redundant
                {
                    AddAlternative alternative = new AddAlternative(model);
                    if (alternative.ShowDialog() == DialogResult.OK)
                    {
                        model.Alternatives.Add(new Alternative(alternative.alternativeName.Text, alternative.alternativeStatus.SelectedText, ""));
                        model.Alternatives.Last().AddTo(s,0);
                    }
                    alternative.Dispose();
                }
                //TODO remove lock msvSDContainerLocked
            }
        }
    }
}
