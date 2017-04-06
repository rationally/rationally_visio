using Microsoft.Office.Interop.Visio;
using Rationally.Visio.RationallyConstants;

namespace Rationally.Visio.View.ContextMenu
{
    public delegate void ContextMenuAction();

    public class ContextMenuItem
    {
        private bool _isEnabled;

        private string _name;

        private ContextMenuItem(VisioShape shape, string eventID, string name, bool isFlyOut = false)
        {
            Shape = shape;
            EventID = eventID;
            IsFlyOut = isFlyOut;
            ActionID = Shape.Shape.UniqueID[(short) VisUniqueIDArgs.visGetOrMakeGUID] + EventID;
           
        }

        public static ContextMenuItem CreateAndRegister(VisioShape shape, string eventID, string name,
            bool isFlyOut = false)
        {
            var menuItem = new ContextMenuItem(shape,eventID,name,isFlyOut);
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                //Assign to local varibale and not field to avoid updatemenuitem being called
                menuItem._name = name;
                menuItem._isEnabled = true;
                menuItem.Shape.AddMenuItem(menuItem);
            }
            else
            {
                if (
                    menuItem.Shape.Shape.CellExists[
                        string.Format(VisioFormulas.Action_Action, menuItem.EventID), (short)VisExistsFlags.visExistsAnywhere] ==
                    Constants.CellExists)
                {
                    menuItem._name =
                        menuItem.Shape.Shape.CellsU[string.Format(VisioFormulas.Action_MenuName, menuItem.EventID)].ResultStr["Value"];
                    menuItem._isEnabled =
                        !(menuItem.Shape.Shape.CellsU[string.Format(VisioFormulas.Action_Disabled, menuItem.EventID)].ResultIU > 0);
                }
            }
            return menuItem;
        }

        public string EventID { get; }
        public bool IsFlyOut { get; }
        public string ActionID { get; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    Shape.UpdateMenuItem(this);
                }
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    Shape.UpdateMenuItem(this);
                }
            }
        }

        public VisioShape Shape { get; }
        public ContextMenuAction Action { get; set; }
    }
}