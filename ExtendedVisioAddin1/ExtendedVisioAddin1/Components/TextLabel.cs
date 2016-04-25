using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtendedVisioAddin1.Components
{
    class TextLabel : RationallyComponent
    {
        private String text;

        public TextLabel(String text)
        {
            this.text = text;
            

        }

        public void Draw(double x, double y)
        {
            Shape textShape = Globals.ThisAddIn.Application.ActivePage.DrawRectangle(x, y, x+text.Length*0.3, y+30);
            //headerShape.TextStyle = "Basic";
            textShape.LineStyle = "Text Only";
            textShape.FillStyle = "Text Only";
            textShape.Characters.Text = text;
            textShape.Characters.CharProps[(short)VisCellIndices.visCharacterSize] = 22;
        }
    }
}
