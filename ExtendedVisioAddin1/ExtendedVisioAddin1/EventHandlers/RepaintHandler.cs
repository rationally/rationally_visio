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
        }

        public void PaintAlternatives()
        {
            //sync the model with the view
            UpdateAlternativeTitles();
            //get alternatives shape
            RationallyComponent alternatives = null;
            foreach (Shape s in Globals.ThisAddIn.Application.ActivePage.Shapes)
            {
            }
            //set height to alternative count * factor
            //loop over alternatives, fetch a shape for each
            //add the shapes to the alternatives shape
        }

        public void UpdateAlternativeTitles()
        {
            //get all title shapes
            foreach (Shape s in Globals.ThisAddIn.Application.ActivePage.Shapes)
            {
                RationallyComponent c = new RationallyComponent(s);
                if (c.RationallyType == "alternativeTitle")
                {
                    model.Alternatives[c.AlternativeIndex].Title = c.Text;
                }
            }
        }
    }
}
