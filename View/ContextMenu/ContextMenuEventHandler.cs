using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.RationallyConstants;

namespace Rationally.Visio.View.ContextMenu
{
    public class ContextMenuEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IDictionary<string, ContextMenuItem> _eventRegistry = new Dictionary<string, ContextMenuItem>();


        private ContextMenuEventHandler()
        {
        }

        public static ContextMenuEventHandler Instance { get; } = new ContextMenuEventHandler();

        public void RegisterMenuEvent(string actionID, ContextMenuItem item) => _eventRegistry[actionID] = item;

        public void OnContextMenuEvent(Application app, int sequencenum, string contextstring)
        {
            try
            {
                if (_eventRegistry.ContainsKey(contextstring) && !Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    Log.Info($"ContextMenuEvent {contextstring} was fired.");
                    var item = _eventRegistry[contextstring];
                    item.Action?.Invoke();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw new RationallyException($"Exception on context menu {contextstring} occured.", ex);
            }
        }
    }
}