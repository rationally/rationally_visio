using Rationally.Visio.View;

namespace Rationally.Visio.EventHandlers
{
    internal class RepaintHandler
    {
        public static void Repaint()
        {
            Globals.ThisAddIn.View.Children.ForEach(c => c.Repaint());
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)//Shapes may not be updated during an undo or redo, so don't place the children ourselves
            {
                Globals.ThisAddIn.View.Children.ForEach(c => c.PlaceChildren());
            }
        }

        public static void Repaint(RComponent component)
        {
            if (component != null)
            {
                component.Repaint();
                if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing) //Shapes may not be updated during an undo or redo, so don't place the children ourselves
                {
                    component.PlaceChildren();
                }
            }
        }
    }
}
