﻿using System;
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
            if (Shape.CellExists[Format(VisioFormulas.Action_Action,item.EventId), (short)VisExistsFlags.visExistsAnywhere] != Constants.CellExists)//if not exists
            {
                //after a redo, we don't need to execute this. It would generate a new undo action on top of the readd-alternative action.
                AddAction(item.EventId, Format(VisioFormulas.Formula_QUEUMARKEREVENT, item.ActionId), item.Name, item.IsFlyOut, item.IsEnabled);
            }
            
            ContextMenuEventHandler.Instance.RegisterMenuEvent(item.ActionId, item);
        }

        public void UpdateMenuItem(ContextMenuItem item)
        {

            if (Shape.CellExists[Format(VisioFormulas.Action_Action, item.EventId), (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists)
            {
                Shape.CellsU[Format(VisioFormulas.Action_Disabled, item.EventId)].FormulaU =
                (!item.IsEnabled).ToString().ToUpper();
                Shape.CellsU[Format(VisioFormulas.Action_MenuName, item.EventId)].FormulaU =
              Format(VisioFormulas.Formula_EscapedValue, item.Name);
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
                    AddUserRow(VisioFormulas.RationallyType);
                }
                return Shape.CellsU[VisioFormulas.Cell_RationallyType].ResultStr[VisioFormulas.Value];
            }
            set
            {
                if (!CellExists(VisioFormulas.Formula_RationallyType))
                {
                    AddUserRow(VisioFormulas.RationallyType);
                }
                Shape.Cells[VisioFormulas.Formula_RationallyType].FormulaU = "\"" + value + "\"";
            }
        }

        public int Id
        {
            get
            {
                if (!CellExists(VisioFormulas.Formula_UniqueId))
                {
                    AddUserRow(VisioFormulas.UniqueId);
                }
                return (int)Shape.CellsU[VisioFormulas.Cell_UniqueId].ResultIU;
            } //Id is unique for its type (alternative, related document, stakeholder, etc)
            protected set
            {
                if (!CellExists(VisioFormulas.Formula_UniqueId))
                {
                    AddUserRow(VisioFormulas.UniqueId);
                }
                Shape.CellsU[VisioFormulas.Formula_UniqueId].ResultIU = value;
            }
        }

        public int ForceAlternativeId //Backreference to alternativeId, for the forces table

        {
            get { return (int)Shape.CellsU[VisioFormulas.Cell_AlternativeUniqueId].ResultIU; }
            protected set { Shape.CellsU[VisioFormulas.Formula_AlternativeUniqueId].ResultIU = value; }
        }

        public string AlternativeIdentifierString
        {
            get { return Shape.CellsU[VisioFormulas.Cell_AlternativeIdentifier].ResultStr[VisioFormulas.Value]; }
            set { Shape.Cells[VisioFormulas.Formula_AlternativeIdentifier].FormulaU = "\"" + value + "\""; }
        }

        public virtual int Index
        {
            get
            {
                if (!CellExists(VisioFormulas.Formula_Index))
                {
                    AddUserRow(VisioFormulas.Index);
                }
                return (int)Shape.CellsU[VisioFormulas.Cell_Index].ResultIU;
            }
            set
            {
                if (!CellExists(VisioFormulas.Formula_Index))
                {
                    AddUserRow(VisioFormulas.Index);
                }
                Shape.CellsU[VisioFormulas.Formula_Index].ResultIU = value;
            }
        }

        public string FilePath
        {
            get { return Shape.CellsU[VisioFormulas.Cell_FilePath].ResultStr[VisioFormulas.Value]; }
            set { Shape.Cells[VisioFormulas.Formula_FilePath].FormulaU = "\"" + value + "\""; }
        }

        public double Width
        {
            get { return Shape.CellsU[VisioFormulas.Cell_Width].ResultIU; }
            set { Shape.CellsU[VisioFormulas.Cell_Width].ResultIU = value; }
        }

        public double Height
        {
            get { return Shape.CellsU[VisioFormulas.Cell_Height].ResultIU; }
            set { Shape.CellsU[VisioFormulas.Cell_Height].ResultIU = value; }
        }
        public double CenterX
        {
            get { return Shape.CellsU[VisioFormulas.Cell_PositionX].Result[VisUnitCodes.visInches]; }
            set { Shape.CellsU[VisioFormulas.Cell_PositionX].Result[VisUnitCodes.visInches] = value; }
        }

        public double CenterY
        {
            get { return Shape.CellsU[VisioFormulas.Cell_PositionY].Result[VisUnitCodes.visInches]; }
            set { Shape.CellsU[VisioFormulas.Cell_PositionY].Result[VisUnitCodes.visInches] = value; }
        }

        
        internal void AddAction(string actionRowId, string action, string name, bool flyout, bool enabled=true)
        {
            if (Shape.CellExists[Format(VisioFormulas.Action_Action, actionRowId), (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists)
            {
                Shape.DeleteRow((short)VisSectionIndices.visSectionAction, Shape.CellsRowIndex[Format(VisioFormulas.Action_Action, actionRowId)]);
            }

            Shape.AddNamedRow((short)VisSectionIndices.visSectionAction, actionRowId,
                (short)VisRowTags.visTagDefault);
            Shape.CellsU[Format(VisioFormulas.Action_Action, actionRowId)].FormulaU = action;
            Shape.CellsU[Format(VisioFormulas.Action_MenuName, actionRowId)].FormulaU =
                Format(VisioFormulas.Formula_EscapedValue, name);

            Shape.CellsU[Format(VisioFormulas.Action_Disabled, actionRowId)].FormulaU =
                (!enabled).ToString().ToUpper();

            Shape.CellsU[Format(VisioFormulas.Action_IsFlyoutChild, actionRowId)].FormulaU =
                flyout.ToString().ToUpper();
        }

        protected void DeleteAction(string fieldName)
        {
            if (Shape.CellExistsU[Format(VisioFormulas.Action_Action, fieldName), (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists)
            {
                Shape.DeleteRow((short)VisSectionIndices.visSectionAction, Shape.CellsRowIndex[Format(VisioFormulas.Action_Action, fieldName)]);
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
            set { Shape.CellsU[VisioFormulas.Cell_ShadowPattern].ResultIU = value; }
        }

        public bool LockDelete
        {
            set { Shape.CellsU[VisioFormulas.Cell_LockDelete].ResultIU = (value ? 1 : 0); }
        }

        public bool LockTextEdit
        {
            protected get { return Shape.CellsU[VisioFormulas.Cell_LockTextEdit].ResultIU > 0; }
            set { Shape.CellsU[VisioFormulas.Cell_LockTextEdit].ResultIU = (value ? 1 : 0); }
        }

        public bool LockWidth
        {
            protected get { return Shape.CellsU[VisioFormulas.Cell_LockWidth].ResultIU > 0; }
            set { Shape.CellsU[VisioFormulas.Cell_LockWidth].ResultIU = (value ? 1 : 0); }
        }

        public bool LockHeight
        {
            protected get { return Shape.CellsU[VisioFormulas.Cell_LockHeight].ResultIU > 0; }
            set { Shape.CellsU[VisioFormulas.Cell_LockHeight].ResultIU = (value ? 1 : 0); }
        }

        public bool MsvSdContainerLocked
        {
            get { return Shape.CellsU[VisioFormulas.Cell_ContainerLocked].ResultStr[VisioFormulas.Value] == "TRUE"; }
            set { Shape.CellsU[VisioFormulas.Cell_ContainerLocked].FormulaU = (value ? "TRUE" : "FALSE"); }
        }
        

        /// <summary>
        /// supported values: see  VisHorizontalAlignTypes (This enum is wrong)
        /// </summary>
        public double HAlign
        {
            set { Shape.CellsU[VisioFormulas.Cell_HAlighn].FormulaForceU = "" + value; }
        }


        protected int FontSize
        {
            set { Shape.Cells[VisioFormulas.Cell_FontSize].FormulaU = value + " pt"; }
        }

        public string FontColor
        {
            set { Shape.CellsU[VisioFormulas.Cell_FontColour].FormulaU = value; }
        }

        public bool StrikeThrough
        {
            get { return bool.Parse(Shape.CellsU[VisioFormulas.Cell_StrikeThrough].FormulaU); }
            set { Shape.CellsU[VisioFormulas.Cell_StrikeThrough].FormulaU = value.ToString().ToUpper(); }
        }

        public int Order
        {
            get { return (int)Shape.CellsU[VisioFormulas.Cell_Order].ResultIU; }
            set { Shape.CellsU[VisioFormulas.Formula_Order].ResultIU = value; }
        }

        //line format

        /// <summary>
        /// set this to 0 to remove the border of a container
        /// </summary>
        public double LinePattern
        {
            set { Shape.CellsU[VisioFormulas.Cell_LinePattern].ResultIU = value; }
        }

        //events
        public string EventDblClick
        {
            set { Shape.CellsU[VisioFormulas.Cell_EventDoubleClick].FormulaU = value; }
        }

        //background
        [Obsolete("Use SetBackgroundColor", false)]
        public string BackgroundColor
        {
            get { return Shape.CellsU[VisioFormulas.Cell_BackGroundColour].FormulaU;  }
            set { Shape.CellsU[VisioFormulas.Cell_BackGroundColour].FormulaU = value; }
        }

        public void SetBackgroundColor(Color color)
        {
            try
            {
                Shape.CellsU[VisioFormulas.Cell_BackGroundColour].FormulaU = Format(VisioFormulas.RGB_Color_Formula,color.R,color.G,color.B);
            }
            catch (COMException ex)
            {   
                Log.Error($"Could not set background color of shape {Shape.Name}");
                throw new RationallyException(ex);
            }
        }

        public string LineColor
        {
            set { Shape.CellsU[VisioFormulas.Cell_LineColour].FormulaU = value; }
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

        public virtual void UpdateIndex(int index) => Index = index;

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
            AddAction(VisioFormulas.MoveUp, Format(VisioFormulas.Formula_QUEUMARKEREVENT, VisioFormulas.MoveUp), Messages.MoveUp, false);
            AddAction(VisioFormulas.MoveDown, Format(VisioFormulas.Formula_QUEUMARKEREVENT, VisioFormulas.MoveUp), Messages.MoveDown, false);

            if (Index == 0)
            {
                DeleteAction(VisioFormulas.MoveUp);
            }

            if (Index == max)
            {
                DeleteAction(VisioFormulas.MoveDown);
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
                Master master = rationallyStencils.Masters.ItemU[masterName];
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
