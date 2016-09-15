using System;
using System.Collections.Generic;
using Rationally.Visio.Model;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class MarkerEventHandlerRegistry
    {
        private static MarkerEventHandlerRegistry eventHandlerRegistry;
        public Dictionary<string, List<IMarkerEventHandler>> Registry; 

        private MarkerEventHandlerRegistry()
        {
            Registry = new Dictionary<string, List<IMarkerEventHandler>>();
        }

        public static MarkerEventHandlerRegistry Instance => eventHandlerRegistry ?? (eventHandlerRegistry = new MarkerEventHandlerRegistry());

        public void Register(string eventKey, IMarkerEventHandler eventHandler)
        {
            if (!eventHandlerRegistry.Registry.ContainsKey(eventKey))
            {
                eventHandlerRegistry.Registry[eventKey] = new List<IMarkerEventHandler>();
            }
            eventHandlerRegistry.Registry[eventKey].Add(eventHandler);
        }

        public void HandleEvent(string eventKey, RModel model, Shape changedShape, string identifier)
        {
            if (Registry.ContainsKey(eventKey) && !Globals.ThisAddIn.Application.IsUndoingOrRedoing)
            {
                Registry[eventKey].ForEach(eh => eh.Execute(model, changedShape, identifier));
            }
            else
            {
                Console.WriteLine("NOTICE: marker event requested on key with to registered handlers: " + eventKey);
            }
        }
    }
}
