using System;
using System.Collections.Generic;
using ExtendedVisioAddin1.View;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    class QueryDeleteEventHandlerRegistry
    {
        private static QueryDeleteEventHandlerRegistry eventHandlerRegistry;
        public Dictionary<string, List<QueryDeleteEventHandler>> Registry; 

        private QueryDeleteEventHandlerRegistry()
        {
            Registry = new Dictionary<string, List<QueryDeleteEventHandler>>();
        }

        public static QueryDeleteEventHandlerRegistry Instance => eventHandlerRegistry ?? (eventHandlerRegistry = new QueryDeleteEventHandlerRegistry());

        public void Register(string eventKey, QueryDeleteEventHandler eventHandler)
        {
            if (!eventHandlerRegistry.Registry.ContainsKey(eventKey))
            {
                eventHandlerRegistry.Registry[eventKey] = new List<QueryDeleteEventHandler>();
            }
            eventHandlerRegistry.Registry[eventKey].Add(eventHandler);
        }

        public void HandleEvent(string eventKey, RView view, Shape changedShape)
        {
            if (Registry.ContainsKey(eventKey))
            {
                Registry[eventKey].ForEach(eh => eh.Execute(eventKey, view, changedShape));
            }
            else
            {
                Console.WriteLine("WARNING: query delete event requested on key with to registered handlers: " + eventKey);
            }
        }
    }
}
