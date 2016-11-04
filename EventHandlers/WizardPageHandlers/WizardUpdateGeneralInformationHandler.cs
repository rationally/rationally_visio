using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rationally.Visio.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Information;

namespace Rationally.Visio.EventHandlers.WizardPageHandlers
{
    internal static class WizardUpdateGeneralInformationHandler
    {
        public static void Execute(ProjectSetupWizard wizard)
        {
            //performs validation, or enters 
            ValidateIfNotDebugging(wizard);

            UpdateGeneralInformationInModel(wizard.tableLayoutMainContentGeneral.TextAuthor.Text,
                                                wizard.tableLayoutMainContentGeneral.TextDecisionTopic.Text,
                                                wizard.tableLayoutMainContentGeneral.DateTimePickerCreationDate.Value.ToLongDateString(), ProjectSetupWizard.DocumentCreation);

        }

        private static void ValidateIfNotDebugging(ProjectSetupWizard wizard)
        {
            if (string.IsNullOrWhiteSpace(wizard.tableLayoutMainContentGeneral.TextDecisionTopic.Text))
            {
#if DEBUG
                wizard.tableLayoutMainContentGeneral.TextDecisionTopic.Text = "Title";
#else
                MessageBox.Show("Enter a decision topic.", "Decision topic missing");
                DialogResult = DialogResult.None;
                return;
#endif
            }
            if (string.IsNullOrWhiteSpace(wizard.tableLayoutMainContentGeneral.TextAuthor.Text))
            {
#if DEBUG
                wizard.tableLayoutMainContentGeneral.TextAuthor.Text = "Author";
#else
                MessageBox.Show("Enter the author's name.", "Author's name missing");
                DialogResult = DialogResult.None;
                return;
#endif
            }
        }


        private static void UpdateGeneralInformationInModel(string author, string decisionName, string date, bool documentCreation)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;

            // Read the contents of setupDialog's TextBox.
            model.Author = author;
            model.DecisionName = decisionName;
            model.DateString = date;
            model.Version = "0.0.1";//TODO this should not be here


            //int scopeId = Globals.RationallyAddIn.Application.BeginUndoScope("wizardUpdate");
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
                RationallyView view = Globals.RationallyAddIn.View;
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
            //Globals.RationallyAddIn.Application.EndUndoScope(scopeId, true);
        }
    }
}
