using System.Reflection;
using log4net;
using Rationally.Visio.View;

namespace Rationally.Visio.EventHandlers
{
    internal static class RepaintHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void Repaint()
        {
            Globals.RationallyAddIn.View.Children.ForEach(c => c.Repaint());
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)//Shapes may not be updated during an undo or redo, so don't place the children ourselves
            {
                Globals.RationallyAddIn.View.Children.ForEach(c => c.PlaceChildren());
            }
        }

        public static void Repaint(VisioShape component)
        {
            if (component != null)
            {
                Log.Debug("Repaint on:" + component.Name);
                component.Repaint();
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //Shapes may not be updated during an undo or redo, so don't place the children ourselves
                {
                    component.PlaceChildren();
                }
            }
        }
    }
}
