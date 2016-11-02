using System;
using System.Collections.Generic;
using Rationally.Visio.Model;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class MarkerEventHandlerRegistry
    {
        private static MarkerEventHandlerRegistry eventHandlerRegistry;
        private readonly Dictionary<string, List<IMarkerEventHandler>> registry; 

        private MarkerEventHandlerRegistry()
        {
            registry = new Dictionary<string, List<IMarkerEventHandler>>();
        }

        public static MarkerEventHandlerRegistry Instance => eventHandlerRegistry ?? (eventHandlerRegistry = new MarkerEventHandlerRegistry());

        public static void Register(string eventKey, IMarkerEventHandler eventHandler)
        {
            if (!Instance.registry.ContainsKey(eventKey))
            {
                Instance.registry[eventKey] = new List<IMarkerEventHandler>();
            }
            Instance.registry[eventKey].Add(eventHandler);
        }

        public void HandleEvent(string eventKey, RationallyModel model, Shape changedShape, string identifier)
        {
            if (registry.ContainsKey(eventKey) && !Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                registry[eventKey].ForEach(eh => eh.Execute(model, changedShape, identifier));
            }
            else
            {
                Console.WriteLine("NOTICE: marker event requested on key with to registered handlers: " + eventKey);
            }
        }
    }
}
