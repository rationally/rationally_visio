using ExtendedVisioAddin1.View;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal class RepaintHandler
    {

        public RepaintHandler()
        {
            //Globals.ThisAddIn.View.Children.ForEach(c => c.RemoveChildren());
            Globals.ThisAddIn.View.Children.ForEach(c => c.Repaint());
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
            {
                Globals.ThisAddIn.View.Children.ForEach(c => c.PlaceChildren());
            }
        }

        public RepaintHandler(RComponent component)
        {
            if (component != null)
            {
                //component.RemoveChildren();
                component.Repaint();
                if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
                {
                    component.PlaceChildren();
                }
            }
        }
    }
}
