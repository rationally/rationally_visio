namespace ExtendedVisioAddin1.EventHandlers
{
    internal class EventHandlerFactory
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
