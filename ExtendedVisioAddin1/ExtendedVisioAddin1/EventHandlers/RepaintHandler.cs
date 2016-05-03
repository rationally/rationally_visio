using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Components;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal class RepaintHandler
    {
        private RModel model;

        public RepaintHandler(RModel model)
        {
            this.model = model;
            //remove all current shapes //TODO only our own
            foreach (IVShape s in Globals.ThisAddIn.Application.ActivePage.Shapes) {  s.Delete(); }
            int a = Globals.ThisAddIn.Application.ActivePage.Shapes.Count;
            if (model.Alternatives.Count > 0)
            {
                AlternativesContainer alternativesContainer = new AlternativesContainer(Globals.ThisAddIn.Application.ActivePage,model.Alternatives);
                alternativesContainer.PlaceChildren();
                alternativesContainer.Repaint();
            }
            //Globals.ThisAddIn.View.Repaint();
        }

        /*public void PaintAlternatives()
        {
            //sync the model with the view
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
            for (int i = 0; i < model.Alternatives.Count; i++)
            {
                IVShape droppedAlternative = model.Alternatives[i].Paint(alternatives.RShape, i, model);
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
                    model.Alternatives[c.AlternativeIndex].Title = c.Text;
                }
            }
        }*/
    }
}
