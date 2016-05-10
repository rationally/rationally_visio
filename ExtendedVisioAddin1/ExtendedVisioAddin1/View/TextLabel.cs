using System;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    public class TextLabel : RComponent
    {
        private short size;

        public TextLabel(Page page, Shape shape) : base(page)
        {
            RShape = shape;
            size = Convert.ToInt16(shape.Cells["Char.Size"].Formula.Split(' ')[0]);
        }

        public TextLabel(Page page, string labelText) : base(page)
        {
            string text = labelText;
            size = 12;
            double fac = size / 12.0;
            RShape = Globals.ThisAddIn.Application.ActivePage.DrawRectangle(0, 0, text.Length * 0.15 * fac, - 0.5); //TODO: magic numbers
            RShape.LineStyle = "Text Only";
            RShape.FillStyle = "Text Only";
            RShape.Characters.Text = text;
            RShape.Characters.CharProps[(short)VisCellIndices.visCharacterSize] = size;
            RShape.CellsU["LinePattern"].ResultIU = 0;
        }

        public void SetFontSize(short fontSize)
        {
            size = fontSize;
        }
    }
}
