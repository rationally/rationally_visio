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

        public void Register(string eventKey, IDeleteEventHandler eventHandler)
        {
            if (!eventHandlerRegistry.registry.ContainsKey(eventKey))
            {
                eventHandlerRegistry.registry[eventKey] = new List<IDeleteEventHandler>();
            }
            eventHandlerRegistry.registry[eventKey].Add(eventHandler);
        }

        public void HandleEvent(string eventKey, RationallyModel model, Shape changedShape)
        {
            
            if (registry.ContainsKey(eventKey))
            {
                registry[eventKey].ForEach(eh => eh.Execute(eventKey, model, changedShape));
            }
            else
            {
                Log.Warn("NOTICE: delete event requested on key with to registered handlers: " + eventKey);
            }
        }
    }
}
