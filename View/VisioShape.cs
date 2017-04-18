using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.View.ContextMenu;
using static System.String;
using Color = System.Drawing.Color;

// ReSharper disable ArrangeRedundantParentheses

namespace Rationally.Visio.View
{
    /// <summary>
    /// Represents a container object in Rationally. Name is a shorthand for Rationally Container.
    /// </summary>
    public class VisioShape
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


        public Shape Shape { get; set; }

        private List<Shape> backUpShapes = new List<Shape>();

        public bool Deleted { get; set; }

        public VisioShape(Page page)
        {
            Page = page;
            Deleted = false;

        }

        //---
        //property wrappers
        //---
        public string Name
        {
            get { return Shape.Name; }
            set { Shape.Name = value;
                Shape.NameU = value;
            }
        }

        public void AddMenuItem(ContextMenuItem item)
        {
            AddAction(item.EventId, string.Format(VisioFormulas.Formula_QUEUMARKEREVENT, item.ActionId), item.Name,item.IsFlyOut,item.IsEnabled);
            ContextMenuEventHandler.Instance.RegisterMenuEvent(item.ActionId, item);
        }

        public void UpdateMenuItem(ContextMenuItem item)
        {

            if (Shape.CellExists[string.Format(VisioFormulas.Action_Action, item.EventId), (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists)
            {
                Shape.CellsU[string.Format(VisioFormulas.Action_Disabled, item.EventId)].Formula =
                (!item.IsEnabled).ToString().ToUpper();
                Shape.CellsU[string.Format(VisioFormulas.Action_MenuName, item.EventId)].Formula =
              string.Format(VisioFormulas.Formula_EscapedValue, item.Name);
            }
        }

        public string NameU
        {
            get { return Shape.NameU; }
            set { Shape.NameU = value; }
        }

        /*public string NameID
        {
            get { return Shape.NameID; }
            set { Shape.NameID = value; }
        }*/

        //type related

        public string RationallyType
        {
            get
            {
                if (!CellExists(VisioFormulas.Formula_RationallyType))
                {
                    AddUserRow("rationallyType");
                }
                return Shape.CellsU[CellConstants.RationallyType].ResultStr["Value"];
            }
            set
            {
                if (!CellExists(VisioFormulas.Formula_RationallyType))
                {
                    AddUserRow("rationallyType");
                }
                Shape.Cells[VisioFormulas.Formula_RationallyType].Formula = "\"" + value + "\"";
            }
        }

        public int Id
        {
            get { return (int)Shape.CellsU["User.uniqueId"].ResultIU; } //Id is unique for its type (alternative, related document, stakeholder, etc)
            protected set { Shape.CellsU["User.uniqueId.Value"].ResultIU = value; }
        }

        public int ForceAlternativeId //Backreference to alternativeId, for the forces table

        {
            get { return (int)Shape.CellsU["User.alternativeUniqueId"].ResultIU; }
            protected set { Shape.CellsU["User.alternativeUniqueId.Value"].ResultIU = value; }
        }

        public string AlternativeIdentifierString
        {
            get { return Shape.CellsU["User.alternativeIdentifier"].ResultStr["Value"]; }
            set { Shape.Cells["User.alternativeIdentifier.Value"].Formula = "\"" + value + "\""; }
        }

        public virtual int Index
        {
            get
            {
                if (!CellExists(VisioFormulas.Formula_Index))
                {
                    AddUserRow("Index");
                }
                return (int)Shape.CellsU[CellConstants.Index].ResultIU;
            }
            set
            {
                if (!CellExists(VisioFormulas.Formula_Index))
                {
                    AddUserRow("Index");
                }
                Shape.CellsU[$"{CellConstants.Index}.Value"].ResultIU = value;
            }
        }

        public string FilePath
        {
            get { return Shape.CellsU["User.filePath"].ResultStr["Value"]; }
            set { Shape.Cells["User.filePath.Value"].Formula = "\"" + value + "\""; }
        }

        public double Width
        {
            get { return Shape.CellsU["Width"].ResultIU; }
            set { Shape.CellsU["Width"].ResultIU = value; }
        }

        public double Height
        {
            get { return Shape.CellsU["Height"].ResultIU; }
            set { Shape.CellsU["Height"].ResultIU = value; }
        }
        public double CenterX
        {
            get { return Shape.CellsU["pinX"].Result[VisUnitCodes.visInches]; }
            set { Shape.CellsU["pinX"].Result[VisUnitCodes.visInches] = value; }
        }

        public double CenterY
        {
            get { return Shape.CellsU["pinY"].Result[VisUnitCodes.visInches]; }
            set { Shape.CellsU["pinY"].Result[VisUnitCodes.visInches] = value; }
        }

        
        internal void AddAction(string actionRowID, string action, string name, bool flyout, bool enabled=true)
        {


            if (Shape.CellExists[string.Format(VisioFormulas.Action_Action, actionRowID), (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists)
            {
                Shape.DeleteRow((short)VisSectionIndices.visSectionAction, Shape.CellsRowIndex[string.Format(VisioFormulas.Action_Action, actionRowID)]);
            }

            Shape.AddNamedRow((short)VisSectionIndices.visSectionAction, actionRowID,
                (short)VisRowTags.visTagDefault);
            Shape.CellsU[string.Format(VisioFormulas.Action_Action, actionRowID)].Formula = action;
            Shape.CellsU[string.Format(VisioFormulas.Action_MenuName, actionRowID)].Formula =
                string.Format(VisioFormulas.Formula_EscapedValue, name);

            Shape.CellsU[string.Format(VisioFormulas.Action_Disabled, actionRowID)].Formula =
                (!enabled).ToString().ToUpper();

            Shape.CellsU[string.Format(VisioFormulas.Action_IsFlyoutChild, actionRowID)].Formula =
                flyout.ToString().ToUpper();
        }

        protected void DeleteAction(string fieldName)
        {
            if (Shape.CellExistsU["Actions." + fieldName + ".Action", (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists)
            {
                Shape.DeleteRow((short)VisSectionIndices.visSectionAction, Shape.CellsRowIndex["Actions." + fieldName + ".Action"]);
            }
        }

        public void AddUserRow(string fieldName) => Shape.AddNamedRow((short)VisSectionIndices.visSectionUser, fieldName, (short)VisRowTags.visTagDefault);

        //content related
        public string Text
        {
            get { return Shape.Text; }
            set
            {
                bool textEditLocked = LockTextEdit;
                LockTextEdit = false;
                Shape.Text = value;
                LockTextEdit = textEditLocked;
            }
        }

        //lock related msvSDContainerLocked

        protected int ShadowPattern
        {
            set { Shape.CellsU["ShdwPattern"].ResultIU = value; }
        }

        public bool LockDelete
        {
            set { Shape.CellsU["LockDelete"].ResultIU = (value ? 1 : 0); }
        }

        public bool LockTextEdit
        {
            protected get { return Shape.CellsU["LockTextEdit"].ResultIU > 0; }
            set { Shape.CellsU["LockTextEdit"].ResultIU = (value ? 1 : 0); }
        }

        public bool LockWidth
        {
            protected get { return Shape.CellsU["LockWidth"].ResultIU > 0; }
            set { Shape.CellsU["LockWidth"].ResultIU = (value ? 1 : 0); }
        }

        public bool LockHeight
        {
            protected get { return Shape.CellsU["LockHeight"].ResultIU > 0; }
            set { Shape.CellsU["LockHeight"].ResultIU = (value ? 1 : 0); }
        }

        public bool MsvSdContainerLocked
        {
            get { return Shape.CellsU["User.msvSDContainerLocked"].ResultStr["Value"] == "TRUE"; }
            set { Shape.CellsU["User.msvSDContainerLocked"].Formula = (value ? "TRUE" : "FALSE"); }
        }
        

        /// <summary>
        /// supported values: see  VisHorizontalAlignTypes (This enum is wrong)
        /// </summary>
        public double HAlign
        {
            set { Shape.CellsU["Para.HorzAlign"].FormulaForce = "" + value; }
        }


        protected int FontSize
        {
            set { Shape.Cells["Char.Size"].Formula = value + " pt"; }
        }

        public string FontColor
        {
            set { Shape.CellsU["Char.Color"].Formula = value; }
        }

        public bool StrikeThrough
        {
            get { return Boolean.Parse(Shape.CellsU["Char.strikethru"].FormulaU); }
            set { Shape.CellsU["Char.strikethru"].Formula = value.ToString().ToUpper(); }
        }

        public int Order
        {
            get { return (int)Shape.CellsU["User.order"].ResultIU; }
            set { Shape.CellsU["User.order.Value"].ResultIU = value; }
        }

        //line format

        /// <summary>
        /// set this to 0 to remove the border of a container
        /// </summary>
        public double LinePattern
        {
            set { Shape.CellsU["LinePattern"].ResultIU = value; }
        }

        //events
        public string EventDblClick
        {
            set { Shape.CellsU["EventDblClick"].Formula = value; }
        }

        //background
        [Obsolete("Use SetBackgroundColor", false)]
        public string BackgroundColor
        {
            get { return Shape.CellsU["FillForegnd"].Formula;  }
            set { Shape.CellsU["FillForegnd"].Formula = value; }
        }

        public void SetBackgroundColor(Color color)
        {
            try
            {
                Shape.CellsU["FillForegnd"].Formula = Format(VisioFormulas.RGB_Color_Formula,color.R,color.G,color.B);
            }
            catch (COMException ex)
            {
                Log.Error($"Could not set background color of shape {Shape.Name}");
                throw new RationallyException(ex);
            }
        }

        public string LineColor
        {
            set { Shape.CellsU["LineColor"].Formula = value; }
        }

        /// <summary>
        /// Updates shapesheet of the stored IVShape. Character.Style holds information about the font style (bold, italic...) in a bitwise manner.
        /// </summary>
        /// <param name="bold">Whether the font should be bold or not.</param>
        public void ToggleBoldFont(bool bold) => Shape.Characters.CharProps[(short)VisCellIndices.visCharacterStyle] = (short)(Shape.Characters.CharPropsRow[(short)VisCellIndices.visCharacterStyle] | (short)(bold ? 17 : 0));

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

        public virtual void UpdateIndex(int index)
        {
            //set our own index to the new value
            Index = index;
        }

        public void MoveChildren(double deltaX, double deltaY)
        {
            if (Shape.ContainerProperties != null) //check if shape is a visio container
            {
                Array ident = Shape.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsDefault);
                List<Shape> shapes = new List<int>((int[])ident).Select(i => Shape.ContainingPage.Shapes.ItemFromID[i]).ToList();
                foreach (VisioShape asComponent in shapes.Select(s => new VisioShape(Shape.ContainingPage) { Shape = s }))
                {
                    asComponent.CenterX += deltaX;
                    asComponent.CenterY += deltaY;
                }
            }
        }

        /// <summary>
        /// Makes a back up of the current shapes that have Shape as their container.
        /// </summary>
        public void StoreChildren()
        {
            Array ident = Shape.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsDefault);
            backUpShapes = new List<int>((int[])ident).Select(i => Shape.ContainingPage.Shapes.ItemFromID[i]).ToList();
        }

        /// <summary>
        /// Takes all the shapes in BackUpShapes and adds them to this container.
        /// </summary>
        public void RestoreChildren() => backUpShapes.ForEach(s => Shape.ContainerProperties.AddMember(s, VisMemberAddOptions.visMemberAddDoNotExpand));

        public virtual bool ExistsInTree(Shape s) => Shape.Equals(s);

        public virtual void AddToTree(Shape s, bool allowAddOfSubpart)
        {

        }
        
        /// <summary>
        /// Traverses the component tree and looks for the component whose Shape matches s.
        /// </summary>
        /// <param name="s">Shape to match Shape against.</param>
        /// <returns>the component, or null.</returns>
        public virtual VisioShape GetComponentByShape(Shape s) => Shape.Equals(s) ? this : null;

        public virtual void RemoveDeleteLock(bool recursive) => LockDelete = false;

        /// <summary>
        /// Marks all child components as deleted, deletes them and then does the same for this component.
        /// </summary>
        public virtual void DeleteRecursive()
        {
            Deleted = true;
            Shape.Delete();
        }

        public void UpdateReorderFunctions(int max)
        {
            AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "Move up", false);
            AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "Move down", false);

            if (Index == 0)
            {
                DeleteAction("moveUp");
            }

            if (Index == max)
            {
                DeleteAction("moveDown");
            }
        }

        public static Shape CreateShapeFromStencilMaster(Page page,  string pathToStencil,  string masterName)
        {

            Document rationallyStencils = null;
            Shape shape = null;

            try
            {
                rationallyStencils = Globals.RationallyAddIn.Application.Documents.OpenEx(pathToStencil,
                    (short)(VisOpenSaveArgs.visOpenRO | VisOpenSaveArgs.visOpenHidden));
                var master = rationallyStencils.Masters.ItemU[masterName];
                shape = page.Drop(master, 0,0);

            }
            catch (COMException ex)
            {
                Log.Error(ex);
                throw new RationallyException($"Could not create shape {masterName} from stencil {pathToStencil}",ex);
            }
            finally
            {
                rationallyStencils?.Close();
            }
            return shape;
        }

        /// <summary>
        /// Finds out whether a cell is the shapeSheet of Shape.
        /// </summary>
        /// <param name="cellName">cellname, INCLUDING section (example: "User.rationallyType")</param>
        /// <returns></returns>
        public bool CellExists(string cellName) => Shape.CellExists[cellName, (short) VisExistsFlags.visExistsAnywhere] == Constants.CellExists;
    }
}
