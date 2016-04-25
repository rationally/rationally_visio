using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtendedVisioAddin1.Components
{
    public class TextLabel : RationallyComponent
    {
        private string text;
        private short size;

        public TextLabel(string text)
        {
            this.text = text;
            this.size = 12;

        }

        /// <summary>
        /// Adds the text label to the sheet, on the given coordinates.
        /// </summary>
        /// <param name="x">Amount of inches from the left.</param>
        /// <param name="y">Amount of inches from the bottom.</param>
        public new Shape Draw(double x, double y)
        {
            double fac = (size/12.0);
            Shape textShape = Globals.ThisAddIn.Application.ActivePage.DrawRectangle(x, y, x+text.Length*0.125*fac, y-0.5);
            //headerShape.TextStyle = "Basic";
            textShape.LineStyle = "Text Only";
            textShape.FillStyle = "Text Only";
            textShape.Characters.Text = text;
            textShape.Characters.CharProps[(short)VisCellIndices.visCharacterSize] = size;
            textShape.CellsU["LinePattern"].ResultIU = 0;
            return textShape;
        }

        public void SetFontSize(short size)
        {
            this.size = size;
        }
    }
}
