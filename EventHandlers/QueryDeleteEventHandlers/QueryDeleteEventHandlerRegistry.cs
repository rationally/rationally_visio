using System;
using System.Collections.Generic;
using Rationally.Visio.View;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal class QueryDeleteEventHandlerRegistry
    {
        private static QueryDeleteEventHandlerRegistry eventHandlerRegistry;
        private readonly Dictionary<string, List<IQueryDeleteEventHandler>> registry; 

        private QueryDeleteEventHandlerRegistry()
        {
            registry = new Dictionary<string, List<IQueryDeleteEventHandler>>();
        }

        public static QueryDeleteEventHandlerRegistry Instance => eventHandlerRegistry ?? (eventHandlerRegistry = new QueryDeleteEventHandlerRegistry());

        public static void Register(string eventKey, IQueryDeleteEventHandler eventHandler)
        {
            if (!eventHandlerRegistry.registry.ContainsKey(eventKey))
            {
                eventHandlerRegistry.registry[eventKey] = new List<IQueryDeleteEventHandler>();
            }
            eventHandlerRegistry.registry[eventKey].Add(eventHandler);
        }

        public void HandleEvent(string eventKey, RationallyView view, Shape changedShape)
        {
            if (registry.ContainsKey(eventKey))
            {
                registry[eventKey].ForEach(eh => eh.Execute(view, changedShape));
            }
            else
            {
                Console.WriteLine("NOTICE: query delete event requested on key with to registered handlers: " + eventKey);
            }
        }
    }
}
