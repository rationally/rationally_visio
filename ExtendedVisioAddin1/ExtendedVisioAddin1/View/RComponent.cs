using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    public class RComponent
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double MarginTop { get; set; }
        public double MarginBottom { get; set; }
        public double MarginLeft { get; set; }
        public double MarginRight { get; set; }

        public Page Page { get; set; }


        public Shape RShape { get; set; }

        public RComponent(Page page)
        {
            this.Page = page;
        }

        //---
        //property wrappers
        //---
        public string Name
        {
            get
            {
                return RShape.Name;
            }
            set
            {
                RShape.Name = value;
            }
        }


        //type related
        public string Type => RShape.CellsU["User.rationallyType"].ResultStr["Value"];

        public string RationallyType
        {
            get
            {
                return RShape.CellsU["User.rationallyType"].ResultStr["Value"];
            }
            set
            {
                RShape.Cells["User.rationallyType.Value"].Formula = "\"" + value + "\"";
            }
        }

        public int AlternativeIndex
        {
            get
            {
                return (int)RShape.CellsU["User.alternativeIndex"].ResultIU;
            }
            set
            {
                RShape.CellsU["User.alternativeIndex.Value"].ResultIU = value;
            }
        }

        public double Width
        {
            get
            {
                return RShape.CellsU["Width"].ResultIU;
            }
            set
            {
                RShape.CellsU["Width"].ResultIU = value;
            }
        }

        public double Height
        {
            get
            {
                return RShape.CellsU["Height"].ResultIU;
            }
            set
            {
                RShape.CellsU["Height"].ResultIU = value;
            }
        }
        public double CenterX
        {
            get
            {
                return RShape.CellsU["pinX"].Result[VisUnitCodes.visInches];
            }
            set
            {
                RShape.CellsU["pinX"].Result[VisUnitCodes.visInches] = value;
            }
        }

        public double CenterY
        {
            get
            {
                return RShape.CellsU["pinY"].Result[VisUnitCodes.visInches];
            }
            set
            {
                RShape.CellsU["pinY"].Result[VisUnitCodes.visInches] = value;
            }
        }

        public void AddAction(string fieldName, string action, string name, bool flyout)
        {
            RShape.AddNamedRow((short)VisSectionIndices.visSectionAction, fieldName, (short)VisRowTags.visTagDefault);
            RShape.CellsU["Actions." + fieldName + ".Action"].Formula = action;
            RShape.CellsU["Actions." + fieldName + ".Menu"].Formula = name;
            RShape.CellsU["Actions." + fieldName + ".FlyoutChild"].Formula = flyout.ToString().ToUpper();
        }

        public void AddUserRow(string fieldName)
        {
            RShape.AddNamedRow((short)VisSectionIndices.visSectionUser, fieldName, (short)VisRowTags.visTagDefault);
        }

        //content related
        public string Text { get { return RShape.Text; } set { RShape.Text = value; } }

        //lock related msvSDContainerLocked

        public bool LockWidth
        {
            get
            {
                return RShape.CellsU["LockWidth"].ResultIU > 0;
            }

            set { RShape.CellsU["LockWidth"].ResultIU = (value ? 1 : 0); }
        }
        public bool LockHeight
        {
            get
            {
                return RShape.CellsU["LockHeight"].ResultIU > 0;
            }

            set { RShape.CellsU["LockHeight"].ResultIU = (value ? 1 : 0); }
        }

        public bool LockMoveX
        {
            get
            {
                return RShape.CellsU["LockMoveX"].ResultIU > 0;
            }

            set { RShape.CellsU["LockMoveX"].ResultIU = (value ? 1 : 0); }
        }
        public bool LockMoveY
        {
            get
            {
                return RShape.CellsU["LockMoveY"].ResultIU > 0;
            }

            set { RShape.CellsU["LockMoveY"].ResultIU = (value ? 1 : 0); }
        }

        public bool LockRotate
        {
            get
            {
                return RShape.CellsU["LockRotate"].ResultIU > 0;
            }

            set { RShape.CellsU["LockRotate"].ResultIU = (value ? 1 : 0); }
        }

        public bool LockDelete
        {
            get
            {
                return RShape.CellsU["LockDelete"].ResultIU > 0;
            }

            set { RShape.CellsU["LockDelete"].ResultIU = (value ? 1 : 0); }
        }

        public bool LockTextEdit
        {
            get
            {
                return RShape.CellsU["LockTextEdit"].ResultIU > 0;
            }

            set { RShape.CellsU["LockTextEdit"].ResultIU = (value ? 1 : 0); }
        }

        public bool MsvSdContainerLocked
        {
            get
            {
                return RShape.CellsU["User.msvSDContainerLocked"].ResultStr["Value"] == "TRUE";
            }

            set { RShape.CellsU["User.msvSDContainerLocked"].Formula = (value ? "TRUE" : "FALSE"); }
        }


        /// <summary>
        /// Updates shapesheet of the stored IVShape. Character.Style holds information about the font style (bold, italic...) in a bitwise manner.
        /// </summary>
        /// <param name="bold">Whether the font should be bold or not.</param>
        public void ToggleBoldFont(bool bold)
        {
            RShape.Characters.CharProps[(short)VisCellIndices.visCharacterStyle] = (short)(RShape.Characters.CharPropsRow[(short)VisCellIndices.visCharacterStyle] | (bold ? 17 : 0));
        }

        //methods
        public IVShape Draw(double x, double y)
        {
            return null;

        }

        public virtual void Repaint()
        {

        }

        public virtual void PlaceChildren()
        {
            
        }
    }
}
