using System.Collections.Generic;
using log4net;
using Rationally.Visio.Model;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteEventHandlerRegistry
    {
        private static DeleteEventHandlerRegistry eventHandlerRegistry;
        private readonly Dictionary<string, List<IDeleteEventHandler>> registry;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DeleteEventHandlerRegistry()
        {
            registry = new Dictionary<string, List<IDeleteEventHandler>>();
        }

        public static DeleteEventHandlerRegistry Instance => eventHandlerRegistry ?? (eventHandlerRegistry = new DeleteEventHandlerRegistry());

        public static void Register(string eventKey, IDeleteEventHandler eventHandler)
        {
            if (!Instance.registry.ContainsKey(eventKey))
            {
                Instance.registry[eventKey] = new List<IDeleteEventHandler>();
            }
            Instance.registry[eventKey].Add(eventHandler);
        }

        public void HandleEvent(string eventKey, RationallyModel model, Shape changedShape)
        {
            
            if (registry.ContainsKey(eventKey))
            {
                registry[eventKey].ForEach(eh => eh.Execute(model, changedShape));
            }
            else
            {
                Log.Warn("NOTICE: delete event requested on key with to registered handlers: " + eventKey);
            }
        }
    }
}
