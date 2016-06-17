using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.DeleteEventHandlers
{
    class DeleteEventHandlerRegistry
    {
        private static DeleteEventHandlerRegistry eventHandlerRegistry;
        public Dictionary<string, List<DeleteEventHandler>> Registry;

        private DeleteEventHandlerRegistry()
        {
            Registry = new Dictionary<string, List<DeleteEventHandler>>();
        }

        public static DeleteEventHandlerRegistry Instance => eventHandlerRegistry ?? (eventHandlerRegistry = new DeleteEventHandlerRegistry());

        public void Register(string eventKey, DeleteEventHandler eventHandler)
        {
            if (!eventHandlerRegistry.Registry.ContainsKey(eventKey))
            {
                eventHandlerRegistry.Registry[eventKey] = new List<DeleteEventHandler>();
            }
            eventHandlerRegistry.Registry[eventKey].Add(eventHandler);
        }

        public void HandleEvent(string eventKey, RModel model, Shape changedShape)
        {
            
            if (Registry.ContainsKey(eventKey))
            {
                Registry[eventKey].ForEach(eh => eh.Execute(eventKey, model, changedShape));
            }
            else
            {
                Console.WriteLine("WARNING: delete event requested on key with to registered handlers: " + eventKey);
            }
        }
    }
}
