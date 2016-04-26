using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtendedVisioAddin1.EventHandlers
{
    class EventHandlerFactory
    {
        public EventHandler this[string eventName]
        {
            get
            {
                switch (eventName)
                {
                    case "addAlternative":
                        return null;
                    default:
                        return null;
                }
            }
        }
}
}
