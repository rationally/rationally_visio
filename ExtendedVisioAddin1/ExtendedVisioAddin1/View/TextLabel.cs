using System;
using System.Drawing;
using Microsoft.Office.Interop.Visio;
using Font = System.Drawing.Font;

//using Font = Microsoft.Office.Interop.Visio.Font;

namespace ExtendedVisioAddin1.View
{
    public class TextLabel : RComponent
    {
        private short size = 12;
        private int lineCount = 1;
        private double characterHeight; //height of one character in inches
        private double characterWidth;
        private double contentTextWidth;

        private double PIXELS_PER_INCH = 90;
        public SizingPolicy UsedSizingPolicy { get; set; }

        public TextLabel(Page page, Shape shape) : base(page)
        {
            RShape = shape;
            size = Convert.ToInt16(shape.Cells["Char.Size"].Formula.Split(' ')[0]);
        }

        public TextLabel(Page page, string labelText) : base(page)
        {
            //UsedSizingPolicy = 0 | SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded | SizingPolicy.ExpandYIfNeeded | SizingPolicy.ShrinkYIfNeeded;
            

            string text = labelText;
            characterHeight = 1.0/72.0*(double) size;

            //characterWidth = characterHeight*0.45;
            //contentTextWidth = characterWidth * (double)text.Length + 0.2;
            contentTextWidth = GetWidthOfString(labelText)/ PIXELS_PER_INCH;
            Document basicDocument = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            RShape = page.Drop(rectMaster, 0,0);
            basicDocument.Close(); //TODO: magic numbers
            RShape.LineStyle = "Text Only";
            RShape.FillStyle = "Text Only";
            RShape.Characters.Text = text;
            RShape.Characters.CharProps[(short)VisCellIndices.visCharacterSize] = size;
            RShape.CellsU["LinePattern"].ResultIU = 0;
            RShape.Name = "TextLabel";
            
            BackgroundColor = "RGB(255,255,255)";
            FontColor = "RGB(89,131,168)";
            ShadowPattern = 0;

            Repaint(); //todo: moet dit echt hier

        }

        public void SetUsedSizingPolicy(SizingPolicy p)
        {
            UsedSizingPolicy = p;
            Repaint();//todo: moet dit echt hier
        }

        public void SetFontSize(short fontSize)
        {
            size = fontSize;//TODO remove this variable and refs
            FontSize = fontSize;
            Repaint();//todo: moet dit echt hier
        }

        private double GetWidthOfString(string str)
        {
            Bitmap objBitmap = default(Bitmap);
            Graphics objGraphics = default(Graphics);

            objBitmap = new Bitmap(1000, 200);
            objGraphics = Graphics.FromImage(objBitmap);

            SizeF stringSize = objGraphics.MeasureString(str, new Font("Calibri", size));

            objBitmap.Dispose();
            objGraphics.Dispose();
            return stringSize.Width;
        }

        public override void Repaint()
        {
            string text = RShape.Text.Replace("\n","");
            characterHeight = 1.0 / 72.0 * (double)size;

            //contentTextWidth = characterWidth * (double)text.Length + 0.2;
            contentTextWidth = GetWidthOfString(text) / PIXELS_PER_INCH;
            characterWidth = contentTextWidth/text.Length;
            //sizing
            if (contentTextWidth > Width)
            {
                if ((UsedSizingPolicy & SizingPolicy.ExpandXIfNeeded) > 0)
                {
                    Width = contentTextWidth;
                }


                //int lineLength = (int)Math.Round(Width / characterWidth);
                int lineLength = (int)(Width/characterWidth);
                string newContent = "";
                if (!((UsedSizingPolicy & SizingPolicy.ExpandXIfNeeded) > 0) && text.Length > lineLength)
                {
                    for (int i = 0; i < (text.Length - lineLength); i += lineLength)
                    {
                        newContent += text.Substring(i, lineLength) + "\n";
                        lineCount++;
                    }
                    //add the last piece of the string
                    newContent += text.Substring(text.Length/lineLength*lineLength);//integer devision
                    RShape.Characters.Text = newContent;
                }

                
                


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

            Text = text;
        }
    }
}
