using System;
using System.Collections.Generic;
using Rationally.Visio.View;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class TextChangedEventHandlerRegistry
    {
        private static TextChangedEventHandlerRegistry eventHandlerRegistry;
        private readonly Dictionary<string, List<ITextChangedEventHandler>> registry;

        private TextChangedEventHandlerRegistry()
        {
            registry = new Dictionary<string, List<ITextChangedEventHandler>>();
        }

        public static TextChangedEventHandlerRegistry Instance => eventHandlerRegistry ?? (eventHandlerRegistry = new TextChangedEventHandlerRegistry());

        public static void Register(string eventKey, ITextChangedEventHandler eventHandler)
        {
            if (!Instance.registry.ContainsKey(eventKey))
            {
                Instance.registry[eventKey] = new List<ITextChangedEventHandler>();
            }
            Instance.registry[eventKey].Add(eventHandler);
        }

        public void HandleEvent(string eventKey, RationallyView view, Shape changedShape)
        {

            if (registry.ContainsKey(eventKey))
            {
                registry[eventKey].ForEach(eh => eh.Execute(view, changedShape));
            }
            else
            {
                Console.WriteLine("NOTICE: textchanged event requested on key with to registered handlers: " + eventKey);
            }
        }
    }
}
