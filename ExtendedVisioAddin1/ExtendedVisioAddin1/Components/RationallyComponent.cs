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

        //property wrappers
        public string Type => Shape1.CellsU["User.rationallyType"].ResultStr["Value"];
        public string Width => Shape1.CellsU["Width"].ResultStr["Value"];
        public string Height => Shape1.CellsU["Height"].ResultStr["Value"];

        public double CenterX => Shape1.CellsU["pinX"].Result[VisUnitCodes.visInches];
        public double CenterY => Shape1.CellsU["pinY"].Result[VisUnitCodes.visInches];

        public void ToggleBoldFont(bool bold)
        {
            //Shape1.CellsU["Character.Style"].ResultIU = Shape1.CellsU["Character.Style"]. | (bold ? 17 : 0);
        }

        //methods
        public IVShape Draw(double x, double y)
        {
            return null;

        }
    }
}
