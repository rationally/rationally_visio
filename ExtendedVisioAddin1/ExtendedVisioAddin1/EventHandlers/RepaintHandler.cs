﻿using ExtendedVisioAddin1.View;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal class RepaintHandler
    {

        public RepaintHandler()
        {
            Globals.ThisAddIn.View.Children.ForEach(c => c.RemoveChildren());
            Globals.ThisAddIn.View.Children.ForEach(c => c.Repaint());
            Globals.ThisAddIn.View.Children.ForEach(c => c.PlaceChildren());
        }

        public RepaintHandler(RContainer component)
        {
            component.RemoveChildren();
            component.Repaint();
            component.PlaceChildren();
        }
    }
}
