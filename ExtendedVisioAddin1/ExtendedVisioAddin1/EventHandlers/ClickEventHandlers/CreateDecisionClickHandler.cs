using System.Linq;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Information;

namespace Rationally.Visio.EventHandlers.ClickEventHandlers
{
    internal class CreateDecisionClickHandler
    {
        public static void Execute(string author, string decisionName, string date, bool documentCreation)
        {
            RModel model = Globals.RationallyAddIn.Model;

            // Read the contents of setupDialog's TextBox.
            model.Author = author;
            model.DecisionName = decisionName;
            model.DateString = date;
            model.Version = "0.0.1";


            int scopeId = Globals.RationallyAddIn.Application.BeginUndoScope("wizardUpdate");
            if (documentCreation)
            {
                //draw the header
                TitleLabel header = new TitleLabel(Globals.RationallyAddIn.Application.ActivePage, model.DecisionName);
                Globals.RationallyAddIn.View.Children.Add(header);
                RepaintHandler.Repaint(header);

                //draw the information container
                InformationContainer informationContainer = new InformationContainer(Globals.RationallyAddIn.Application.ActivePage, model.Author, model.DateString, model.Version);
                Globals.RationallyAddIn.View.Children.Add(informationContainer);
                RepaintHandler.Repaint(informationContainer);
            }
            else
            {
                RView view = Globals.RationallyAddIn.View;
                if (view.Children.Any(x => x is InformationContainer))
                {
                    InformationContainer container = view.Children.FirstOrDefault(x => x is InformationContainer) as InformationContainer;
                    RepaintHandler.Repaint(container);
                }
                if (view.Children.Any(x => x is TitleLabel))
                {
                    TitleLabel titleLabel = view.Children.FirstOrDefault(x => x is TitleLabel) as TitleLabel;
                    RepaintHandler.Repaint(titleLabel);
                }
            }
            Globals.RationallyAddIn.Application.EndUndoScope(scopeId, true);
        }
    }
}
