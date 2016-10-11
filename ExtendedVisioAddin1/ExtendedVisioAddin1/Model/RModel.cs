using System.Collections.Generic;
using System.Linq;
using Rationally.Visio.View.Alternatives;
using Rationally.Visio.View.Documents;
using Rationally.Visio.View.Forces;

namespace Rationally.Visio.Model
{
    /// <summary>
    /// Model for the Rationally application. Name is a shorthand for Rationally Model.
    /// </summary>
    public class RModel //Shorthand for Rationally Model
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
            AlternativeStates = new List<string> {"Accepted", "Challenged", "Discarded", "Proposed", "Rejected"}; //Currently hardcoded, could be user setting in future product.
        }

        public void RegenerateAlternativeIdentifiers()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //Don't update the view during an undo, since the undo does that for us
            {
                int i = 0;
                AlternativesContainer alternativesContainer = (AlternativesContainer) Globals.RationallyAddIn.View.Children.First(c => c is AlternativesContainer);
                alternativesContainer.Children.Where(c => c is AlternativeContainer).ToList().ForEach(c => ((AlternativeContainer) c).SetAlternativeIdentifier(i++));
            }
            int j = 0;
            foreach (Alternative a in Alternatives)
            {
                a.Identifier = (char)(65 + j) + ":"; //TODO: Generating Identifiers seems a good candidate for a GOF Builder Design Pattern  (IIdentifierGenerator, AlphabeticalGenerator, NumericalGenerator) 
                j++;
            }
        }

        public void RegenerateDocumentIdentifiers()
        {
            int i = 0;
            RelatedDocumentsContainer docsContainer = (RelatedDocumentsContainer)Globals.RationallyAddIn.View.Children.First(c => c is RelatedDocumentsContainer);
            docsContainer.Children.Where(c => c is RelatedDocumentContainer).ToList().ForEach(c => ((RelatedDocumentContainer)c).SetDocumentIdentifier(i++));
        }

        public void RegenerateForceIdentifiers()
        {
            int i = 0;
            ForcesContainer forcesContaineresContainer = (ForcesContainer)Globals.RationallyAddIn.View.Children.First(c => c is ForcesContainer);
            forcesContaineresContainer.Children.Where(c => c is ForceContainer).ToList().ForEach(c => ((ForceContainer)c).SetForceIdentifier(i++));
        }
    }
}
