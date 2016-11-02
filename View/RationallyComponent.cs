using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        public double X { get; set; }
        public double Y { get; set; }
        public double MarginTop { get; set; }
        public double MarginBottom { get; set; }
        public double MarginLeft { get; set; }
        public double MarginRight { get; set; }

        public void SetMargin(double m)
        {
            MarginTop = m;
            MarginBottom = m;
            MarginLeft = m;
            MarginRight = m;
        }
        public Page Page { get; set; }


        public Shape RShape { get; set; }

        public List<Shape> BackUpShapes = new List<Shape>();

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
            set { RShape.Name = value; }
        }


        //type related
        public string Type => RShape.CellsU[CellConstants.RationallyType].ResultStr["Value"];

        public string RationallyType
        {
            get { return RShape.CellsU[CellConstants.RationallyType].ResultStr["Value"]; }
            set { RShape.Cells["User.rationallyType.Value"].Formula = "\"" + value + "\""; }
        }

        public string IsStub
        {
            get { return RShape.CellsU[CellConstants.Stub].ResultStr["Value"]; }
            set { RShape.Cells["User.isStub.Value"].Formula = "\"" + value + "\""; }
        }

        public virtual int AlternativeIndex
        {
            get { return (int)RShape.CellsU[CellConstants.AlternativeIndex].ResultIU; }
            set { RShape.CellsU["User.alternativeIndex.Value"].ResultIU = value; }
        }

        public int UniqueIdentifier
        {
            get { return (int)RShape.CellsU["User.uniqueId"].ResultIU; }
            set { RShape.CellsU["User.uniqueId.Value"].ResultIU = value; }
        }

        public int AlternativeUniqueIdentifier
        {
            get { return (int)RShape.CellsU["User.alternativeUniqueId"].ResultIU; }
            set { RShape.CellsU["User.alternativeUniqueId.Value"].ResultIU = value; }
        }

        public string AlternativeIdentifier
        {
            get { return RShape.CellsU["User.alternativeIdentifier"].ResultStr["Value"]; }
            set { RShape.Cells["User.alternativeIdentifier.Value"].Formula = "\"" + value + "\""; }
        }

        public virtual int ForceIndex
        {
            get { return (int)RShape.CellsU["User.forceIndex"].ResultIU; }
            set { RShape.CellsU["User.forceIndex.Value"].ResultIU = value; }
        }

        public virtual int DocumentIndex
        {
            get { return (int)RShape.CellsU["User.documentIndex"].ResultIU; }
            set { RShape.CellsU["User.documentIndex.Value"].ResultIU = value; }
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

        public void AddAction(string fieldName, string action, string name, bool flyout)
        {
            DeleteAction(fieldName);
            RShape.AddNamedRow((short)VisSectionIndices.visSectionAction, fieldName, (short)VisRowTags.visTagDefault);
            RShape.CellsU["Actions." + fieldName + ".Action"].Formula = action;
            RShape.CellsU["Actions." + fieldName + ".Menu"].Formula = name;
            RShape.CellsU["Actions." + fieldName + ".FlyoutChild"].Formula = flyout.ToString().ToUpper();
        }

        public void DeleteAction(string fieldName)
        {
            if (RShape.CellExistsU["Actions." + fieldName + ".Action", (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists)
            {
                RShape.DeleteRow((short)VisSectionIndices.visSectionAction, RShape.CellsRowIndex["Actions." + fieldName + ".Action"]);
            }
        }

        public void AddUserRow(string fieldName)
        {
            RShape.AddNamedRow((short)VisSectionIndices.visSectionUser, fieldName, (short)VisRowTags.visTagDefault);
        }

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

        public bool LockWidth
        {
            get { return RShape.CellsU["LockWidth"].ResultIU > 0; }
            set { RShape.CellsU["LockWidth"].ResultIU = (value ? 1 : 0); }
        }
        public bool LockHeight
        {
            get { return RShape.CellsU["LockHeight"].ResultIU > 0; }
            set { RShape.CellsU["LockHeight"].ResultIU = (value ? 1 : 0); }
        }

        public bool LockMoveX
        {
            get { return RShape.CellsU["LockMoveX"].ResultIU > 0; }
            set { RShape.CellsU["LockMoveX"].ResultIU = (value ? 1 : 0); }
        }
        public bool LockMoveY
        {
            get { return RShape.CellsU["LockMoveY"].ResultIU > 0; }
            set { RShape.CellsU["LockMoveY"].ResultIU = (value ? 1 : 0); }
        }

        public bool LockRotate
        {
            get { return RShape.CellsU["LockRotate"].ResultIU > 0; }
            set { RShape.CellsU["LockRotate"].ResultIU = (value ? 1 : 0); }
        }

        public int ShadowPattern
        {
            get { return int.Parse(RShape.CellsU["ShdwPattern"].ResultIU.ToString(CultureInfo.CurrentCulture)); }
            set { RShape.CellsU["ShdwPattern"].ResultIU = value; }
        }

        public bool LockDelete
        {
            get { return RShape.CellsU["LockDelete"].ResultIU > 0; }
            set { RShape.CellsU["LockDelete"].ResultIU = (value ? 1 : 0); }
        }

        public bool LockTextEdit
        {
            get { return RShape.CellsU["LockTextEdit"].ResultIU > 0; }
            set { RShape.CellsU["LockTextEdit"].ResultIU = (value ? 1 : 0); }
        }

        public bool MsvSdContainerLocked
        {
            get { return RShape.CellsU["User.msvSDContainerLocked"].ResultStr["Value"] == "TRUE"; }
            set { RShape.CellsU["User.msvSDContainerLocked"].Formula = (value ? "TRUE" : "FALSE"); }
        }

        public double IndFirst
        {
            get { return RShape.CellsU["Para.IndFirst"].ResultIU; }
            set { RShape.CellsU["Para.IndFirst"].ResultIU = value; }
        }

        public double IndLeft
        {
            get { return RShape.CellsU["Para.IndLeft"].ResultIU; }
            set { RShape.CellsU["Para.IndLeft"].ResultIU = value; }
        }

        public double IndRight
        {
            get { return RShape.CellsU["Para.IndRight"].ResultIU; }
            set { RShape.CellsU["Para.IndRight"].ResultIU = value; }
        }

        public double SpLine
        {
            get { return RShape.CellsU["Para.SpLine"].ResultIU; }
            set { RShape.CellsU["Para.SpLine"].ResultIU = value; }
        }

        public double SpBefore
        {
            get { return RShape.CellsU["Para.SpBefore"].ResultIU; }
            set { RShape.CellsU["Para.SpBefore"].ResultIU = value; }
        }

        public double SpAfter
        {
            get { return RShape.CellsU["Para.SpAfter"].ResultIU; }
            set { RShape.CellsU["Para.SpAfter"].ResultIU = value; }
        }

        /// <summary>
        /// supported values: see  VisHorizontalAlignTypes (This enum is wrong)
        /// </summary>
        public double HAlign
        {
            get { return RShape.CellsU["Para.HorzAlign"].ResultIU; }
            set { RShape.CellsU["Para.HorzAlign"].FormulaForce = "" + value; }
        }

        public double Bullet
        {
            get { return RShape.CellsU["Para.Bullet"].ResultIU; }
            set { RShape.CellsU["Para.Bullet"].ResultIU = value; }
        }

        public string BulletString
        {
            get { return RShape.CellsU["Para.BulletStr"].ResultStr["Value"]; }
            set { RShape.CellsU["Para.BulletStr"].Formula = "\"" + value + "\""; }
        }

        public double BulletFont
        {
            get { return RShape.CellsU["Para.BulletFont"].ResultIU; }
            set { RShape.CellsU["Para.BulletFont"].ResultIU = value; }
        }

        public double TextPosAfterBullet
        {
            get { return RShape.CellsU["Para.TextPosAfterBullet"].ResultIU; }
            set { RShape.CellsU["Para.TextPosAfterBullet"].ResultIU = value; }
        }

        public double BulletSize
        {
            get { return RShape.CellsU["Para.BulletFontSize"].ResultIU; }
            set { RShape.CellsU["Para.BulletFontSize"].ResultIU = value; }
        }

        public int FontSize
        {
            get { return int.Parse(RShape.Cells["Char.Size"].Formula.Split(' ')[0]); }
            set { RShape.Cells["Char.Size"].Formula = value + " pt"; }
        }

        public string FontColor
        {
            get { return RShape.CellsU["Char.Color"].ResultStr["Value"]; }
            set { RShape.CellsU["Char.Color"].Formula = value; }
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
            get { return RShape.CellsU["LinePattern"].ResultIU; }
            set { RShape.CellsU["LinePattern"].ResultIU = value; }
        }

        //events
        public string EventDblClick
        {
            get { return RShape.CellsU["EventDblClick"].ResultStr["Value"]; }
            set { RShape.CellsU["EventDblClick"].Formula = value; }
        }

        //background
        public string BackgroundColor
        {
            get { return RShape.CellsU["FillForegnd"].ResultStr["Value"]; }
            set { RShape.CellsU["FillForegnd"].Formula = value; }
        }

        public string LineColor
        {
            get { return RShape.CellsU["LineColor"].ResultStr["Value"]; }
            set { RShape.CellsU["LineColor"].Formula = value; }
        }

        /// <summary>
        /// Updates shapesheet of the stored IVShape. Character.Style holds information about the font style (bold, italic...) in a bitwise manner.
        /// </summary>
        /// <param name="bold">Whether the font should be bold or not.</param>
        public void ToggleBoldFont(bool bold)
        {
            RShape.Characters.CharProps[(short)VisCellIndices.visCharacterStyle] = (short)(RShape.Characters.CharPropsRow[(short)VisCellIndices.visCharacterStyle] | (short)(bold ? 17 : 0));
        }

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

        public virtual void Move(double deltaX, double deltaY)
        {
            CenterX += deltaX;
            CenterY += deltaY;

            MoveChildren(deltaX, deltaY);
        }


        public virtual void MoveChildren(double deltaX, double deltaY)
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
        public virtual void StoreChildren()
        {
            Array ident = RShape.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsDefault);
            BackUpShapes = new List<int>((int[])ident).Select(i => RShape.ContainingPage.Shapes.ItemFromID[i]).ToList();
        }

        /// <summary>
        /// Takes all the shapes in BackUpShapes and adds them to this container.
        /// </summary>
        public virtual void RestoreChildren()
        {
            BackUpShapes.ForEach(s => RShape.ContainerProperties.AddMember(s, VisMemberAddOptions.visMemberAddDoNotExpand));
        }

        public virtual bool ExistsInTree(Shape s)
        {
            return RShape.Equals(s);
        }

        public virtual void AddToTree(Shape s, bool allowAddOfSubpart)
        {

        }
        
        /// <summary>
        /// Traverses the component tree and looks for the component whose RShape matches s.
        /// </summary>
        /// <param name="s">Shape to match RShape against.</param>
        /// <returns>the component, or null.</returns>
        public virtual RationallyComponent GetComponentByShape(Shape s)
        {
            return RShape.Equals(s) ? this : null;
        }

        public virtual void RemoveDeleteLock(bool recursive)
        {
            LockDelete = false;
        }
    }
}
