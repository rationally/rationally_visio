using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal interface ITextChangedEventHandler
    {
        void Execute(RationallyView view, Shape changedShape);
    }
}
