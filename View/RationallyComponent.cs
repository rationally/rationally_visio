using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.RationallyConstants;

// ReSharper disable ArrangeRedundantParentheses

namespace Rationally.Visio.View
{
    /// <summary>
    /// Represents a container object in Rationally. Name is a shorthand for Rationally Container.
    /// </summary>
    public class RationallyComponent
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public double MarginTop { get;
            protected set; }
        public double MarginBottom { get; protected set; }
        public double MarginLeft { get; protected set; }
        public double MarginRight { get; protected set; }

        protected void SetMargin(double m)
        {
            MarginTop = m;
            MarginBottom = m;
            MarginLeft = m;
            MarginRight = m;
        }
        public Page Page { get; set; }


        public Shape RShape { get; set; }

        private List<Shape> backUpShapes = new List<Shape>();

        public bool Deleted { get; set; }

        public RationallyComponent(Page page)
        {
            Page = page;
            Deleted = false;

        }

        //---
        //property wrappers
        //---
        public string Name
        {
            get { return RShape.Name; }
            set { RShape.Name = value;
                RShape.NameU = value;
            }
        }

        public string NameU
        {
            get { return RShape.NameU; }
            set { RShape.NameU = value; }
        }

        /*public string NameID
        {
            get { return RShape.NameID; }
            set { RShape.NameID = value; }
        }*/

        //type related

        public string RationallyType
        {
            get { return RShape.CellsU[CellConstants.RationallyType].ResultStr["Value"]; }
            set { RShape.Cells["User.rationallyType.Value"].Formula = "\"" + value + "\""; }
        }

        public int Id
        {
            get { return (int)RShape.CellsU["User.uniqueId"].ResultIU; } //Id is unique for its type (alternative, related document, stakeholder, etc)
            protected set { RShape.CellsU["User.uniqueId.Value"].ResultIU = value; }
        }

        public int ForceAlternativeId //Backreference to alternativeId, for the forces table

        {
            get { return (int)RShape.CellsU["User.alternativeUniqueId"].ResultIU; }
            protected set { RShape.CellsU["User.alternativeUniqueId.Value"].ResultIU = value; }
        }

        public string AlternativeIdentifierString
        {
            get { return RShape.CellsU["User.alternativeIdentifier"].ResultStr["Value"]; }
            set { RShape.Cells["User.alternativeIdentifier.Value"].Formula = "\"" + value + "\""; }
        }

        public virtual int Index
        {
            get { return (int)RShape.CellsU[CellConstants.Index].ResultIU; }
            set { RShape.CellsU[$"{CellConstants.Index}.Value"].ResultIU = value; }
        }

        public string FilePath
        {
            get { return RShape.CellsU["User.filePath"].ResultStr["Value"]; }
            set { RShape.Cells["User.filePath.Value"].Formula = "\"" + value + "\""; }
        }

        public double Width
        {
            get { return RShape.CellsU["Width"].ResultIU; }
            set { RShape.CellsU["Width"].ResultIU = value; }
        }

        public double Height
        {
            get { return RShape.CellsU["Height"].ResultIU; }
            set { RShape.CellsU["Height"].ResultIU = value; }
        }
        public double CenterX
        {
            get { return RShape.CellsU["pinX"].Result[VisUnitCodes.visInches]; }
            set { RShape.CellsU["pinX"].Result[VisUnitCodes.visInches] = value; }
        }

        public double CenterY
        {
            get { return RShape.CellsU["pinY"].Result[VisUnitCodes.visInches]; }
            set { RShape.CellsU["pinY"].Result[VisUnitCodes.visInches] = value; }
        }

        protected void AddAction(string fieldName, string action, string name, bool flyout)
        {
            DeleteAction(fieldName);
            RShape.AddNamedRow((short)VisSectionIndices.visSectionAction, fieldName, (short)VisRowTags.visTagDefault);
            RShape.CellsU["Actions." + fieldName + ".Action"].Formula = action;
            RShape.CellsU["Actions." + fieldName + ".Menu"].Formula = name;
            RShape.CellsU["Actions." + fieldName + ".FlyoutChild"].Formula = flyout.ToString().ToUpper();
        }

        protected void DeleteAction(string fieldName)
        {
            if (RShape.CellExistsU["Actions." + fieldName + ".Action", (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists)
            {
                RShape.DeleteRow((short)VisSectionIndices.visSectionAction, RShape.CellsRowIndex["Actions." + fieldName + ".Action"]);
            }
        }

        public void AddUserRow(string fieldName) => RShape.AddNamedRow((short)VisSectionIndices.visSectionUser, fieldName, (short)VisRowTags.visTagDefault);

        //content related
        public string Text
        {
            get { return RShape.Text; }
            set
            {
                bool textEditLocked = LockTextEdit;
                LockTextEdit = false;
                RShape.Text = value;
                LockTextEdit = textEditLocked;
            }
        }

        //lock related msvSDContainerLocked

        protected int ShadowPattern
        {
            set { RShape.CellsU["ShdwPattern"].ResultIU = value; }
        }

        public bool LockDelete
        {
            set { RShape.CellsU["LockDelete"].ResultIU = (value ? 1 : 0); }
        }

        public bool LockTextEdit
        {
            protected get { return RShape.CellsU["LockTextEdit"].ResultIU > 0; }
            set { RShape.CellsU["LockTextEdit"].ResultIU = (value ? 1 : 0); }
        }

        public bool LockWidth
        {
            protected get { return RShape.CellsU["LockWidth"].ResultIU > 0; }
            set { RShape.CellsU["LockWidth"].ResultIU = (value ? 1 : 0); }
        }

        public bool LockHeight
        {
            protected get { return RShape.CellsU["LockHeight"].ResultIU > 0; }
            set { RShape.CellsU["LockHeight"].ResultIU = (value ? 1 : 0); }
        }

        public bool MsvSdContainerLocked
        {
            get { return RShape.CellsU["User.msvSDContainerLocked"].ResultStr["Value"] == "TRUE"; }
            set { RShape.CellsU["User.msvSDContainerLocked"].Formula = (value ? "TRUE" : "FALSE"); }
        }
        

        /// <summary>
        /// supported values: see  VisHorizontalAlignTypes (This enum is wrong)
        /// </summary>
        public double HAlign
        {
            set { RShape.CellsU["Para.HorzAlign"].FormulaForce = "" + value; }
        }


        protected int FontSize
        {
            set { RShape.Cells["Char.Size"].Formula = value + " pt"; }
        }

        public string FontColor
        {
            set { RShape.CellsU["Char.Color"].Formula = value; }
        }

        public bool StrikeThrough
        {
            set { RShape.CellsU["Char.strikethru"].Formula = value.ToString().ToUpper(); }
        }

        public int Order
        {
            get { return (int)RShape.CellsU["User.order"].ResultIU; }
            set { RShape.CellsU["User.order.Value"].ResultIU = value; }
        }

        //line format

        /// <summary>
        /// set this to 0 to remove the border of a container
        /// </summary>
        public double LinePattern
        {
            set { RShape.CellsU["LinePattern"].ResultIU = value; }
        }

        //events
        public string EventDblClick
        {
            set { RShape.CellsU["EventDblClick"].Formula = value; }
        }

        //background
        public string BackgroundColor
        {
            get { return RShape.CellsU["FillForegnd"].Formula;  }
            set { RShape.CellsU["FillForegnd"].Formula = value; }
        }

        public string LineColor
        {
            set { RShape.CellsU["LineColor"].Formula = value; }
        }

        /// <summary>
        /// Updates shapesheet of the stored IVShape. Character.Style holds information about the font style (bold, italic...) in a bitwise manner.
        /// </summary>
        /// <param name="bold">Whether the font should be bold or not.</param>
        public void ToggleBoldFont(bool bold) => RShape.Characters.CharProps[(short)VisCellIndices.visCharacterStyle] = (short)(RShape.Characters.CharPropsRow[(short)VisCellIndices.visCharacterStyle] | (short)(bold ? 17 : 0));

        //methods

        public virtual void Repaint()
        {

        }

        public virtual void PlaceChildren()
        {

        }

        public virtual void RemoveChildren()
        {

        }
        

        public void MoveChildren(double deltaX, double deltaY)
        {
            if (RShape.ContainerProperties != null) //check if shape is a visio container
            {
                Array ident = RShape.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsDefault);
                List<Shape> shapes = new List<int>((int[])ident).Select(i => RShape.ContainingPage.Shapes.ItemFromID[i]).ToList();
                foreach (RationallyComponent asComponent in shapes.Select(s => new RationallyComponent(RShape.ContainingPage) { RShape = s }))
                {
                    asComponent.CenterX += deltaX;
                    asComponent.CenterY += deltaY;
                }
            }
        }

        /// <summary>
        /// Makes a back up of the current shapes that have RShape as their container.
        /// </summary>
        public void StoreChildren()
        {
            Array ident = RShape.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsDefault);
            backUpShapes = new List<int>((int[])ident).Select(i => RShape.ContainingPage.Shapes.ItemFromID[i]).ToList();
        }

        /// <summary>
        /// Takes all the shapes in BackUpShapes and adds them to this container.
        /// </summary>
        public void RestoreChildren() => backUpShapes.ForEach(s => RShape.ContainerProperties.AddMember(s, VisMemberAddOptions.visMemberAddDoNotExpand));

        public virtual bool ExistsInTree(Shape s) => RShape.Equals(s);

        public virtual void AddToTree(Shape s, bool allowAddOfSubpart)
        {

        }
        
        /// <summary>
        /// Traverses the component tree and looks for the component whose RShape matches s.
        /// </summary>
        /// <param name="s">Shape to match RShape against.</param>
        /// <returns>the component, or null.</returns>
        public virtual RationallyComponent GetComponentByShape(Shape s) => RShape.Equals(s) ? this : null;

        public virtual void RemoveDeleteLock(bool recursive) => LockDelete = false;
    }
}
