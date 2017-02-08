using System.Collections.Generic;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal static class DeleteEventHandlerRegistry
    {
        private static  Dictionary<string, List<IDeleteEventHandler>> registry;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Register(string eventKey, IDeleteEventHandler eventHandler)
        {
            if (registry == null)
            {
                registry = new Dictionary<string, List<IDeleteEventHandler>>();
            }

            if (!registry.ContainsKey(eventKey))
            {
                registry[eventKey] = new List<IDeleteEventHandler>();
            }
            registry[eventKey].Add(eventHandler);
        }

        public static void HandleEvent(string eventKey, RationallyModel model, Shape changedShape)
        {
            if (registry == null)
            {
                registry = new Dictionary<string, List<IDeleteEventHandler>>();
            }

            if (registry.ContainsKey(eventKey))
            {
                registry[eventKey].ForEach(eh => eh.Execute(model, changedShape));
            }
            else
            {
                Log.Info("NOTICE: delete event requested on key with to registered handlers: " + eventKey);
            }
        }
    }
}
