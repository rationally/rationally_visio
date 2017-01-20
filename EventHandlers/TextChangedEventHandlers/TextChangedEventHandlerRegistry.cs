using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using Rationally.Visio.View;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal static class TextChangedEventHandlerRegistry
    {
        private static Dictionary<string, List<ITextChangedEventHandler>> registry;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Register(string eventKey, ITextChangedEventHandler eventHandler)
        {
            if (registry == null)
            {
                registry = new Dictionary<string, List<ITextChangedEventHandler>>();
            }

            if (!registry.ContainsKey(eventKey))
            {
                registry[eventKey] = new List<ITextChangedEventHandler>();
            }
            registry[eventKey].Add(eventHandler);
        }

        public static void HandleEvent(string eventKey, RationallyView view, Shape changedShape)
        {
            if (registry == null)
            {
                registry = new Dictionary<string, List<ITextChangedEventHandler>>();
            }

            if (registry.ContainsKey(eventKey))
            {
                registry[eventKey].ForEach(eh => eh.Execute(view, changedShape));
            }
            else
            {
                Log.Info("NOTICE: textchanged event requested on key with to registered handlers: " + eventKey);
            }
        }
    }
}
