using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtendedVisioAddin1.Components
{


    public class RationallyComponent
    {
        //constructors
        public RationallyComponent()
        {

        }
        public RationallyComponent(IVShape shape)
        {
            this.Shape1 = shape;
        }
        public IVShape Shape1 { get; }

        //---
        //property wrappers
        //---

        //type related
        public string Type => Shape1.CellsU["User.rationallyType"].ResultStr["Value"];

        public string RationallyType
        {
            get { return Shape1.CellsU["User.rationallyType"].ResultStr["Value"]; }
            set
            {
                //if (Shape1.RowExists["User.rationallyType.Value",0] > 0)
               // {
                    Shape1.CellsU["User.rationallyType.Value"].Formula = value;
                //}
            }
        }

        public double AlternativeIndex
        {
            get { return Shape1.CellsU["User.alternativeIndex"].ResultIU; }
            set { Shape1.CellsU["User.alternativeIndex.Value"].ResultIU = value; }
        }

        public double Width
        {
            get { return Shape1.CellsU["Width"].ResultIU; }
            set { Shape1.CellsU["Width"].ResultIU = value; }
        }

        public double Height
        {
            get { return Shape1.CellsU["Height"].ResultIU; }
            set { Shape1.CellsU["Height"].ResultIU = value; }
        }
        public double CenterX => Shape1.CellsU["pinX"].Result[VisUnitCodes.visInches];
        public double CenterY => Shape1.CellsU["pinY"].Result[VisUnitCodes.visInches];


        //content related
        public string Text { get { return Shape1.Text; } set { Shape1.Text = value; } }

        public bool LockWidth
        {
            get
            {
                return Shape1.CellsU["LockWidth"].ResultIU > 0;
            }

            set { Shape1.CellsU["LockWidth"].ResultIU = (value ? 1 : 0); }
        }


        //lock related

        /// <summary>
        /// Updates shapesheet of the stored IVShape. Character.Style holds information about the font style (bold, italic...) in a bitwise manner.
        /// </summary>
        /// <param name="bold">Whether the font should be bold or not.</param>
        public void ToggleBoldFont(bool bold)
        {
            Shape1.Characters.CharProps[(short)VisCellIndices.visCharacterStyle] = (short)(Shape1.Characters.CharPropsRow[(short)VisCellIndices.visCharacterStyle] | (bold ? 17 : 0));
        }

        //methods
        public IVShape Draw(double x, double y)
        {
            return null;

        }
    }
}
