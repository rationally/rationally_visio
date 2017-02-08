using System;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.RationallyConstants;
using Font = System.Drawing.Font;

// ReSharper disable RedundantCast
// ReSharper disable ArrangeRedundantParentheses

namespace Rationally.Visio.View
{
    public class TextLabel : RationallyComponent
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private short size = 12;
        private int lineCount = 1;
        private double characterHeight; //height of one character in inches
        private double characterWidth;
        private double contentTextWidth;
        private static readonly Regex TextLabelRegex = new Regex(@"TextLabel(\.\d+)?$");
        protected SizingPolicy UsedSizingPolicy { get; set; }

        protected TextLabel(Page page, Shape shape) : base(page)
        {
            RShape = shape;

            size = Convert.ToInt16(shape.Cells["Char.Size"].Formula.Split(' ')[0]);
        }

        protected TextLabel(Page page, string labelText) : base(page)
        {
            string text = labelText;
            characterHeight = (1.0/72.0)*(double) size;

            

            contentTextWidth = GetWidthOfString(labelText);// PixelsPerInch;
            Document basicDocument = Globals.RationallyAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            RShape = page.Drop(rectMaster, 0,0);
            basicDocument.Close();
            RShape.LineStyle = "Text Only";
            RShape.FillStyle = "Text Only";
            RShape.Characters.Text = text;
            RShape.Characters.CharProps[(short)VisCellIndices.visCharacterSize] = size;
            RShape.CellsU["LinePattern"].ResultIU = 0;
            RShape.Name = "TextLabel";

            AddUserRow("order"); //allows sorting, even with same-type shapes
            AddUserRow("rationallyType");
            BackgroundColor = "RGB(255,255,255)";
            FontColor = "RGB(89,131,168)";
            ShadowPattern = 0;
        }

        public void SetUsedSizingPolicy(SizingPolicy p) => UsedSizingPolicy = p;

        protected void SetFontSize(short fontSize)
        {
            size = fontSize;
            FontSize = fontSize;
            Repaint();
        }

        private double GetWidthOfString(string str)
        {
            Bitmap objBitmap = new Bitmap(1000, 200);
            Graphics objGraphics = Graphics.FromImage(objBitmap);
            objGraphics.PageUnit = GraphicsUnit.Inch;
            SizeF stringSize = objGraphics.MeasureString(str, new Font("Calibri", size));

            objBitmap.Dispose();
            objGraphics.Dispose();
            return stringSize.Width;
        }

        public override void Repaint()
        {
            string text = RShape.Text.Replace("\n","");
            characterHeight = (1.0 / 72.0) * (double)size;
            
            contentTextWidth = GetWidthOfString(text) + (8*Constants.WidthOfOnePoint);// / PixelsPerInch;
            characterWidth = contentTextWidth/text.Length;
            //sizing
            if (contentTextWidth > Width)
            {
                if ((UsedSizingPolicy & SizingPolicy.ExpandXIfNeeded) > 0)
                {
                    Width = contentTextWidth;
                }
                
                int lineLength = (int)(Width/characterWidth);
                string newContent = "";
                if (!((UsedSizingPolicy & SizingPolicy.ExpandXIfNeeded) > 0) && (text.Length > lineLength))
                {
                    lineCount = 1;
                    for (int i = 0; i < (text.Length - lineLength); i += lineLength)
                    {
                        newContent += text.Substring(i, lineLength) + "\n";
                        lineCount++;
                    }
                    //add the last piece of the string
                    newContent += text.Substring((text.Length/lineLength)*lineLength);//integer devision
                    if ((RShape.Characters.Text != newContent) && !Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                    {
                        bool oldLock = LockTextEdit;
                        LockTextEdit = false;
                        RShape.Characters.Text = newContent;
                        LockTextEdit = oldLock;
                    }
                }
                if ((Height < (characterHeight * (double)lineCount)) && ((UsedSizingPolicy & SizingPolicy.ExpandYIfNeeded) > 0))
                {
                    Height = characterHeight * (double)lineCount;
                }
            }

            if ((contentTextWidth < Width) && ((UsedSizingPolicy & SizingPolicy.ShrinkXIfNeeded) > 0))
            {
                Width = contentTextWidth;
            }

            if ((Height > (characterHeight * (double)lineCount)) && ((UsedSizingPolicy & SizingPolicy.ShrinkYIfNeeded) > 0))
            {
                Height = characterHeight * (double)lineCount;
            }

            if (Text != text) //Don't update text if not needed, fixes un and redo
            {
                Text = text;
            }
        }
        
        public static bool IsTextLabel(string name) => TextLabelRegex.IsMatch(name);
    }
}
