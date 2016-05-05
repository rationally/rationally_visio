using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.View;

namespace ExtendedVisioAddin1.Components
{
    public class TextLabel : RComponent
    {
        private string text;
        private short size;

        public TextLabel(Page page, Shape shape) : base(page)
        {
            this.RShape = shape;
            this.text = shape.Text;
            short size = Convert.ToInt16(shape.Cells["Char.Size"].ResultIU);
        }

        public TextLabel(Page page, string text) : base(page)
        {
            this.text = text;
            this.size = 12;
            double fac = (size / 12.0);
            this.RShape = Globals.ThisAddIn.Application.ActivePage.DrawRectangle(0, 0, text.Length * 0.125 * fac, - 0.5);
            RShape.LineStyle = "Text Only";
            RShape.FillStyle = "Text Only";
            RShape.Characters.Text = text;
            RShape.Characters.CharProps[(short)VisCellIndices.visCharacterSize] = size;
            RShape.CellsU["LinePattern"].ResultIU = 0;
        }

        public void SetFontSize(short size)
        {
            this.size = size;
        }
    }
}
