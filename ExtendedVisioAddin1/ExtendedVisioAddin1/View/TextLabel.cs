using System;
using System.Drawing;
using Microsoft.Office.Interop.Visio;
using Font = System.Drawing.Font;

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
            double characterHeight = (1.0/72.0)*(double)size; //height of one character in inches
            double characterWidth = characterHeight*0.55;
            //double textW = Graphics.MeasureString(text, new Font("calibri",size), 999);

            RShape = Globals.ThisAddIn.Application.ActivePage.DrawRectangle(0, 0, characterWidth * (double)text.Length + 0.2, - 0.5); //TODO: magic numbers
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
