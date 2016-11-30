﻿using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal static class MarkerEventHandlerRegistry
    {

        private static Dictionary<string, List<IMarkerEventHandler>> registry; 
        public static void Register(string eventKey, IMarkerEventHandler eventHandler)
        {
            if (registry == null)
            {
                registry = new Dictionary<string, List<IMarkerEventHandler>>();
            }

            if (!registry.ContainsKey(eventKey))
            {
                registry[eventKey] = new List<IMarkerEventHandler>();
            }
            registry[eventKey].Add(eventHandler);
        }

        public static void HandleEvent(string eventKey, Shape changedShape, string identifier)
        {
            if (registry == null)
            {
                registry = new Dictionary<string, List<IMarkerEventHandler>>();
            }

            if (registry.ContainsKey(eventKey) && !Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                registry[eventKey].ForEach(eh => eh.Execute(changedShape, identifier));
            }
            else
            {
                Console.WriteLine("NOTICE: marker event requested on key with to registered handlers: " + eventKey);
            }
        }
    }
}
