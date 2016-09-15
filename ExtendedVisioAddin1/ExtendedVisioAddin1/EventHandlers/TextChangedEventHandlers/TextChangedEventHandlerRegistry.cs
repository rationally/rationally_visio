using System;
using System.Collections.Generic;
using Rationally.Visio.View;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class TextChangedEventHandlerRegistry
    {
        private static TextChangedEventHandlerRegistry eventHandlerRegistry;
        public Dictionary<string, List<ITextChangedEventHandler>> Registry;

        private TextChangedEventHandlerRegistry()
        {
            Registry = new Dictionary<string, List<ITextChangedEventHandler>>();
        }

        public static TextChangedEventHandlerRegistry Instance => eventHandlerRegistry ?? (eventHandlerRegistry = new TextChangedEventHandlerRegistry());

        public void Register(string eventKey, ITextChangedEventHandler eventHandler)
        {
            if (!eventHandlerRegistry.Registry.ContainsKey(eventKey))
            {
                eventHandlerRegistry.Registry[eventKey] = new List<ITextChangedEventHandler>();
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
                Console.WriteLine("NOTICE: textchanged event requested on key with to registered handlers: " + eventKey);
            }
        }
    }
}
