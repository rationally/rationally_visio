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
                TextLabel header = new TextLabel(Globals.RationallyAddIn.Application.ActivePage, model.DecisionName);

                header.SetUsedSizingPolicy(SizingPolicy.FixedSize);
                header.HAlign = 0; //left, since the enum is wrong
                header.Width = 7.7;
                header.Height = 0.3056;
                header.SetFontSize(22);
                header.CenterX = 4.15;
                header.CenterY = 22.483;


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
            }
            Globals.RationallyAddIn.Application.EndUndoScope(scopeId, true);
        }
    }
}
