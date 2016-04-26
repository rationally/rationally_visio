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
        public RationallyComponent(Shape shape)
        {
            this.Shape1 = shape;
        }
        public Shape Shape1 { get; }

        //property wrappers
        public string Type => Shape1.CellsU["User.rationallyType"].ResultStr["Value"];


        //methods
        public Shape Draw(double x, double y)
        {
            return null;

        }
    }
}
