
using Rationally.Visio.Model;
using Rationally.Visio.View;

namespace Rationally.Visio.EventHandlers.ClickEventHandlers
{
    internal class CreateDecisionClickHandler
    {
        public static void Execute(string author, string decisionName, string date)
        {
            RModel model = Globals.RationallyAddIn.Model;

            // Read the contents of setupDialog's TextBox.
            model.Author = author;
            model.DecisionName = decisionName;
            model.Date = date;
            model.Version = "0.0.1";


            int scopeId = Globals.RationallyAddIn.Application.BeginUndoScope("HeaderAddition");
            //draw the header
            TextLabel header = new TextLabel(Globals.RationallyAddIn.Application.ActivePage, model.DecisionName);

            header.SetUsedSizingPolicy(SizingPolicy.FixedSize);
            header.HAlign = 0;//left, since the enum is wrong
            header.Width = 10.5;
            header.Height = 0.3056;
            header.SetFontSize(22);
            header.CenterX = 5.5;
            header.CenterY = 22.483;

            //draw the information container
            InformationContainer informationContainer = new InformationContainer(Globals.RationallyAddIn.Application.ActivePage, model.Author, model.Date, model.Version);
            
            Globals.RationallyAddIn.View.Children.Add(informationContainer);

            RepaintHandler.Repaint(informationContainer);
            Globals.RationallyAddIn.Application.EndUndoScope(scopeId, true);
        }
    }
}
