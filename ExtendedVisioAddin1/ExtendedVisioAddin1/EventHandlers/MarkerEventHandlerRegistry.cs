using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    class MarkerEventHandlerRegistry
    {
        private static MarkerEventHandlerRegistry eventHandlerRegistry = null;
        public Dictionary<string, List<MarkerEventHandler>> registry; 

        private MarkerEventHandlerRegistry()
        {
            registry = new Dictionary<string, List<MarkerEventHandler>>();
        }

        public static MarkerEventHandlerRegistry Instance {
            get
            {
                if (eventHandlerRegistry == null)
                {
                    eventHandlerRegistry = new MarkerEventHandlerRegistry();
                }
                return eventHandlerRegistry;
            }
        }

        public void Register(string eventKey, MarkerEventHandler eventHandler)
        {
            if (!eventHandlerRegistry.registry.ContainsKey(eventKey))
            {
                eventHandlerRegistry.registry[eventKey] = new List<MarkerEventHandler>();
            }
            eventHandlerRegistry.registry[eventKey].Add(eventHandler);
        }

        public void HandleEvent(string eventKey, RModel model, Shape changedShape, string identifier)
        {
            registry[eventKey].ForEach(eh => eh.Execute(model, changedShape, identifier));
        }
    }
}
