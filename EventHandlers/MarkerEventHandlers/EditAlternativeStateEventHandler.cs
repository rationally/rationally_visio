namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    /*
    internal class EditAlternativeStateEventHandler : IMarkerEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(Shape s, string newState)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            VisioShape c = new VisioShape(Globals.RationallyAddIn.Application.ActivePage) {Shape = s};

            int index = c.Index;
            Alternative alternative = model.Alternatives[index];
            alternative.Status = newState;
            AlternativeContainer container =
                (AlternativeContainer)
                ((AlternativesContainer) Globals.RationallyAddIn.View.Children.Find(y => y.Name == "Alternatives"))
                .Children.Find(x => (x.Index == index) && x is AlternativeContainer);
            AlternativeStateShape component =
                (AlternativeStateShape) container.Children.Find(x => x is AlternativeStateShape);

            AlternativeState _newAlternativeState;

            Enum.TryParse(newState, out _newAlternativeState);

            component.State = _newAlternativeState;

            RepaintHandler.Repaint(container);
        }
    }
    */
}