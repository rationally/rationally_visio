using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Components;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal class RepaintHandler
    {
        private RModel model;

        public RepaintHandler(RModel model)
        {
            this.model = model;
            PaintAlternatives();
        }

        public void PaintAlternatives()
        {
            //sync the model with the view
            UpdateAlternativeTitles();
            //get alternatives shape
            RationallyComponent alternatives = null;
            foreach (Shape s in Globals.ThisAddIn.Application.ActivePage.Shapes)
            {
                RationallyComponent c = new RationallyComponent(s);
                if (c.Shape1.CellExistsU["User.rationallyType",0] != 0 && c.RationallyType == "alternatives")
                {
                    alternatives = c;
                    continue;
                }
                //remove old alternative shapes
                if (c.Shape1.CellExistsU["User.rationallyType.Value", 0] != 0 && (c.RationallyType == "alternative" || c.RationallyType == "alternativeIdentifier" || c.RationallyType == "alternativeTitle" || c.RationallyType == "alternativeDescription" || c.RationallyType == "alternativeState"))
                {
                    c.LockDelete = false;
                    c.Shape1.Delete();
                }
            }

            //set height to alternative count * factor
            if (alternatives != null)
            {
                alternatives.Height = Alternative.ALTERNATIVE_HEIGHT*model.Alternatives.Count;
            }
            else
            {
                return;//nothing to paint
            }


            

            //alternatives.Shape1.ContainerProperties.ResizeAsNeeded = VisContainerAutoResize.visContainerAutoResizeExpandContract;
            //loop over alternatives, paint and fetch a shape for each
            for (int i = 0; i < model.Alternatives.Count; i++)
            {
                IVShape droppedAlternative = model.Alternatives[i].Paint(alternatives.Shape1, i, model);
                //alternatives.Shape1.Drop(droppedAlternative, alternatives.CenterX, alternatives.CenterY - (alternatives.Height/2) + i*Alternative.ALTERNATIVE_HEIGHT);
            }
            //add the shapes to the alternatives shape
        }

        public void UpdateAlternativeTitles()
        {
            //get all title shapes
            foreach (Shape s in Globals.ThisAddIn.Application.ActivePage.Shapes)
            {
                RationallyComponent c = new RationallyComponent(s);
                if (c.Shape1.CellExistsU["User.rationallyType", 0] != 0 && c.RationallyType == "alternativeTitle")
                {
                    model.Alternatives[c.AlternativeIndex].Title = c.Text;
                }
            }
        }
    }
}
