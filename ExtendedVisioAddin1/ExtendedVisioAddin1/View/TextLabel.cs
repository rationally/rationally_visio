using System;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    public class TextLabel : RComponent
    {
        private short size;
        private int lineCount = 1;
        private readonly double characterHeight; //height of one character in inches
        private readonly double characterWidth;
        private readonly double contentTextWidth;
        public SizingPolicy UsedSizingPolicy { get; set; }

        public TextLabel(Page page, Shape shape) : base(page)
        {
            RShape = shape;
            size = Convert.ToInt16(shape.Cells["Char.Size"].Formula.Split(' ')[0]);
        }

        public TextLabel(Page page, string labelText) : base(page)
        {
            UsedSizingPolicy = 0 | SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded | SizingPolicy.ExpandYIfNeeded | SizingPolicy.ShrinkYIfNeeded;

            string text = labelText;
            size = 12;
            characterHeight = 1.0/72.0*(double) size;
            characterWidth = characterHeight*0.55;
            contentTextWidth = characterWidth * (double)text.Length + 0.2;
            
            Document basicDocument = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            RShape = page.Drop(rectMaster, 0,0);
            basicDocument.Close();
            //RShape = Globals.ThisAddIn.Application.ActivePage.DrawRectangle(0, 0, contentTextWidth, - 0.5); //TODO: magic numbers
            RShape.LineStyle = "Text Only";
            RShape.FillStyle = "Text Only";
            RShape.Text = text;
            RShape.Characters.CharProps[(short)VisCellIndices.visCharacterSize] = size;
            RShape.CellsU["LinePattern"].ResultIU = 0;

            

            

        }

        public void SetFontSize(short fontSize)
        {
            size = fontSize;
        }

        public override void Repaint()
        {
            string text = RShape.Text;
            //sizing
            if (contentTextWidth > Width)
            {
                if ((UsedSizingPolicy & SizingPolicy.ExpandXIfNeeded) > 0)
                {
                    Width = contentTextWidth;
                }


                int lineLength = (int)(Width / characterWidth);
                string newContent = "";
                for (int i = 0; i < (text.Length - lineLength); i += lineLength)
                {
                    newContent += text.Substring(i, lineLength) + "\n";
                    lineCount++;
                }
                //add the last piece of the string
                newContent += text.Substring(text.Length);
                RShape.Text = newContent;


                if ((Height < characterHeight * (double)lineCount) && (UsedSizingPolicy & SizingPolicy.ExpandYIfNeeded) > 0)
                {
                    Height = characterHeight * (double)lineCount;
                }
            }

            if (contentTextWidth < Width && (UsedSizingPolicy & SizingPolicy.ShrinkXIfNeeded) > 0)
            {
                Width = contentTextWidth;
            }

            if (Height > characterHeight * (double)lineCount && (UsedSizingPolicy & SizingPolicy.ShrinkYIfNeeded) > 0)
            {
                Height = characterHeight * (double)lineCount;
            }
        }
    }
}
