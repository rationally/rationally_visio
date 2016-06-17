using System;
using System.Collections.Generic;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    class MarkerEventHandlerRegistry
    {
        private static MarkerEventHandlerRegistry eventHandlerRegistry;
        public Dictionary<string, List<MarkerEventHandler>> Registry; 

        private MarkerEventHandlerRegistry()
        {
            Registry = new Dictionary<string, List<MarkerEventHandler>>();
        }

        public static MarkerEventHandlerRegistry Instance => eventHandlerRegistry ?? (eventHandlerRegistry = new MarkerEventHandlerRegistry());

        public void Register(string eventKey, MarkerEventHandler eventHandler)
        {
            if (!eventHandlerRegistry.Registry.ContainsKey(eventKey))
            {
                eventHandlerRegistry.Registry[eventKey] = new List<MarkerEventHandler>();
            }
            eventHandlerRegistry.Registry[eventKey].Add(eventHandler);
        }

        public void HandleEvent(string eventKey, RModel model, Shape changedShape, string identifier)
        {
            if (Registry.ContainsKey(eventKey))
            {
                Registry[eventKey].ForEach(eh => eh.Execute(model, changedShape, identifier));
            }
            else
            {
                Console.WriteLine("WARNING: marker event requested on key with to registered handlers: " + eventKey);
            }
        }
    }
}
