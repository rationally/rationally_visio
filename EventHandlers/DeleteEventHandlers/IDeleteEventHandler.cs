using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal interface IDeleteEventHandler
    {
        void Execute(RationallyModel model, Shape changedShape);
    }
}
