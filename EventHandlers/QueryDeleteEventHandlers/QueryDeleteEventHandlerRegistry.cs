using System.Collections.Generic;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal static class QueryDeleteEventHandlerRegistry
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Dictionary<string, List<IQueryDeleteEventHandler>> registry; 
        public static void Register(string eventKey, IQueryDeleteEventHandler eventHandler)
        {
            if (registry == null)
            {
                registry = new Dictionary<string, List<IQueryDeleteEventHandler>>();
            }

            if (!registry.ContainsKey(eventKey))
            {
                registry[eventKey] = new List<IQueryDeleteEventHandler>();
            }
            registry[eventKey].Add(eventHandler);
        }

        public static void HandleEvent(string eventKey, RationallyView view, Shape changedShape)
        {
            if (registry == null)
            {
                registry = new Dictionary<string, List<IQueryDeleteEventHandler>>();
            }

            if (registry.ContainsKey(eventKey))
            {
                registry[eventKey].ForEach(eh => eh.Execute(view, changedShape));
            }
            else
            {
               Log.Info("NOTICE: query delete event requested on key with to registered handlers: " + eventKey);
            }
        }
    }
}
