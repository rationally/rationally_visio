using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.ContextMenu
{
    public class ContextMenuEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IDictionary<string, ContextMenuItem> eventRegistry = new Dictionary<string, ContextMenuItem>();


        private ContextMenuEventHandler()
        {
        }

        public static ContextMenuEventHandler Instance { get; } = new ContextMenuEventHandler();

        public void RegisterMenuEvent(string actionId, ContextMenuItem item) => eventRegistry[actionId] = item;

        public void OnContextMenuEvent(Application app, int sequencenum, string contextstring)
        {
            try
            {
                if (eventRegistry.ContainsKey(contextstring) && !Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    Log.Info($"ContextMenuEvent {contextstring} was fired.");
                    ContextMenuItem item = eventRegistry[contextstring];
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