namespace ExtendedVisioAddin1.EventHandlers
{
    internal class RepaintHandler
    {

        public RepaintHandler()
        {
            //this.Model = Model;
            //remove all current shapes //TODO only our own
            /*foreach (IVShape s in Globals.ThisAddIn.Application.ActivePage.Shapes) {  s.Delete(); }
            int a = Globals.ThisAddIn.Application.ActivePage.Shapes.Count;
            if (Model.Alternatives.Count > 0)
            {
                //AlternativesContainer alternativesContainer = new AlternativesContainer(Globals.ThisAddIn.Application.ActivePage,Model.Alternatives);
                
            }*/
            Globals.ThisAddIn.View.Children.ForEach(c => c.RemoveChildren());
            Globals.ThisAddIn.View.Children.ForEach(c => c.Repaint());
            Globals.ThisAddIn.View.Children.ForEach(c => c.PlaceChildren());
            //Globals.ThisAddIn.View.Repaint();
        }

        /*public void PaintAlternatives()
        {
            //sync the Model with the view
            UpdateAlternativeTitles();
            //get alternatives shape
            RComponent alternatives = null;
            foreach (IVShape s in Globals.ThisAddIn.Application.ActivePage.Shapes)
            {
                RComponent c = new RComponent(Globals.ThisAddIn.Application.ActivePage);
                c.RShape = s;

                if (c.RShape.CellExistsU["User.rationallyType",0] != 0 && c.RationallyType == "alternatives")
                {
                    alternatives = c;
                    continue;
                }
                //remove old alternative shapes
                if (c.RShape.CellExistsU["User.rationallyType.Value", 0] != 0 && (c.RationallyType == "alternative" || c.RationallyType == "alternativeIdentifier" || c.RationallyType == "alternativeTitle" || c.RationallyType == "alternativeDescription" || c.RationallyType == "alternativeState"))
                {
                    c.LockDelete = false;
                    c.RShape.Delete();
                }
            }


            

            //alternatives.RShape.ContainerProperties.ResizeAsNeeded = VisContainerAutoResize.visContainerAutoResizeExpandContract;
            //loop over alternatives, paint and fetch a shape for each
            for (int i = 0; i < Model.Alternatives.Count; i++)
            {
                IVShape droppedAlternative = Model.Alternatives[i].Paint(alternatives.RShape, i, Model);
                //alternatives.RShape.Drop(droppedAlternative, alternatives.CenterX, alternatives.CenterY - (alternatives.Height/2) + i*Alternative.ALTERNATIVE_HEIGHT);
            }
            //add the shapes to the alternatives shape
        }*/

        /*public void UpdateAlternativeTitles()
        {
            //get all title shapes
            foreach (Shape s in Globals.ThisAddIn.Application.ActivePage.Shapes)
            {
                RComponent c = new RComponent(Globals.ThisAddIn.Application.ActivePage);
                c.RShape = s;
                if (c.RShape.CellExistsU["User.rationallyType", 0] != 0 && c.RationallyType == "alternativeTitle")
                {
                    Model.Alternatives[c.AlternativeIndex].Title = c.Text;
                }
            }
        }*/
    }
}
