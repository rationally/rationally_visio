using Microsoft.Office.Interop.Visio;
using Rationally.Visio.RationallyConstants;

namespace Rationally.Visio.View.ContextMenu
{
    public delegate void ContextMenuAction();

    public class ContextMenuItem
    {
        private bool isEnabled;

        private string name;

        private ContextMenuItem(VisioShape shape, string eventId, string name, bool isFlyOut = false)
        {
            Shape = shape;
            EventId = eventId;
            IsFlyOut = isFlyOut;
            ActionId = Shape.Shape.UniqueID[(short) VisUniqueIDArgs.visGetOrMakeGUID] + EventId;
           
        }

        public static ContextMenuItem CreateAndRegister(VisioShape shape, string eventId, string name,
            bool isFlyOut = false)
        {
            ContextMenuItem menuItem = new ContextMenuItem(shape,eventId,name,isFlyOut);
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                //Assign to local varibale and not field to avoid updatemenuitem being called
                menuItem.name = name;
                menuItem.isEnabled = true;
                menuItem.Shape.AddMenuItem(menuItem);
            }
            else
            {
                if (
                    menuItem.Shape.Shape.CellExists[
                        string.Format(VisioFormulas.Action_Action, menuItem.EventId), (short)VisExistsFlags.visExistsAnywhere] ==
                    Constants.CellExists)
                {
                    menuItem.name =
                        menuItem.Shape.Shape.CellsU[string.Format(VisioFormulas.Action_MenuName, menuItem.EventId)].ResultStr["Value"];
                    menuItem.isEnabled =
                        !(menuItem.Shape.Shape.CellsU[string.Format(VisioFormulas.Action_Disabled, menuItem.EventId)].ResultIU > 0);
                }
            }
            return menuItem;
        }

        public string EventId { get; }
        public bool IsFlyOut { get; }
        public string ActionId { get; }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    Shape.UpdateMenuItem(this);
                }
            }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
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