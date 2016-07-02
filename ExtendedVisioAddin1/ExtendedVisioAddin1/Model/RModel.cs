using System.Collections.Generic;
using System.Linq;
using ExtendedVisioAddin1.View.Alternatives;
using ExtendedVisioAddin1.View.Documents;
using ExtendedVisioAddin1.View.Forces;

namespace ExtendedVisioAddin1.Model
{
    public class RModel
    {
        public List<Alternative> Alternatives { get; set; }

        public List<RelatedDocument> Documents { get; set; }
        public List<string> AlternativeStates { get; set; }

        public string Author { get; set; }

        public string DecisionName { get; set; }

        public string Date { get; set; }

        public string Version { get; set; }

        public List<Force> Forces { get; set; }

        public RModel()
        {
            Alternatives = new List<Alternative>();
            Documents = new List<RelatedDocument>();
            Forces = new List<Force>();
            AlternativeStates = new List<string> {"Accepted", "Challenged", "Discarded", "Proposed", "Rejected"};
        }

        public void RegenerateAlternativeIdentifiers()
        {
            int i = 0;
            AlternativesContainer alternativesContainer = (AlternativesContainer)Globals.ThisAddIn.View.Children.First(c => c is AlternativesContainer);
            alternativesContainer.Children.Where(c => c is AlternativeContainer).ToList().ForEach(c => ((AlternativeContainer)c).SetAlternativeIdentifier(i++));

            int j = 0;
            foreach (Alternative a in Alternatives)
            {
                a.Identifier = (char)(65 + j) + ":";
                j++;
            }
        }

        public void RegenerateDocumentIdentifiers()
        {
            int i = 0;
            RelatedDocumentsContainer docsContainer = (RelatedDocumentsContainer)Globals.ThisAddIn.View.Children.First(c => c is RelatedDocumentsContainer);
            docsContainer.Children.Where(c => c is RelatedDocumentContainer).ToList().ForEach(c => ((RelatedDocumentContainer)c).SetDocumentIdentifier(i++));
        }

        public void RegenerateForceIdentifiers()
        {
            int i = 0;
            ForcesContainer forcesContaineresContainer = (ForcesContainer)Globals.ThisAddIn.View.Children.First(c => c is ForcesContainer);
            forcesContaineresContainer.Children.Where(c => c is ForceContainer).ToList().ForEach(c => ((ForceContainer)c).SetForceIdentifier(i++));
        }
    }
}
