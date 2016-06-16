using System.Linq;
using ExtendedVisioAddin1.EventHandlers;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View.Alternatives;
using ExtendedVisioAddin1.View.Documents;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    public class RView : RContainer
    {
        public RView(Page page) : base(page)
        {

        }

        public void AddAlternative(Alternative alternative)
        {
            AlternativesContainer container = (AlternativesContainer) Globals.ThisAddIn.View.Children.First(ch => ch is AlternativesContainer);
            container.Children.Add(new AlternativeContainer(Globals.ThisAddIn.Application.ActivePage, Globals.ThisAddIn.Model.Alternatives.Count - 1, alternative));
            new RepaintHandler();
        }

        public void AddRelatedDocument(RelatedDocument document)
        {
            //container of all related documents:
            RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer)Globals.ThisAddIn.View.Children.First(c => c is RelatedDocumentsContainer);
            //create a container that wraps the new document
            RelatedDocumentContainer relatedDocumentContainer = new RelatedDocumentContainer(Globals.ThisAddIn.Application.ActivePage, Globals.ThisAddIn.Model.Documents.Count - 1, document);
            relatedDocumentsContainer.Children.Add(relatedDocumentContainer);

            new RepaintHandler(relatedDocumentsContainer);
        }

        /// <summary>
        /// Deletes an alternative container from the view.
        /// </summary>
        /// <param name="index">identifier of the alternative.</param>
        /// <param name="deleteShape">Shape was already deleted</param>
        public void DeleteAlternative(int index, bool deleteShape)
        {
            AlternativesContainer alternativesContainer = (AlternativesContainer)Children.FirstOrDefault(c => c is AlternativesContainer); //May have been deleted by the user
            if (alternativesContainer == null)
            {
                new RepaintHandler();
                return;
            }
            AlternativeContainer alternative = (AlternativeContainer)alternativesContainer?.Children.FirstOrDefault(x => x.AlternativeIndex == index && x is AlternativeContainer); //Return null if no container or no alternative with index
            if (alternative != null)
            {
                alternativesContainer.Children.Remove(alternative);
                if (deleteShape)
                {
                    alternative.RShape.DeleteEx(0); //deletes the alternative, and it's child components. This should not be done when the shape is already gone, such as when the user has deleted it himself.
                }
                int i = 0;
                alternativesContainer.Children.Where(c => c is AlternativeContainer).ToList().ForEach(c => ((AlternativeContainer)c).SetAlternativeIdentifier(i++));
                i = 0;
                foreach (Alternative a in Globals.ThisAddIn.Model.Alternatives)
                {
                    a.Identifier = (char)(65 + i) + ":";
                    i++;
                }
                new RepaintHandler();
            }
        }
        
        public override bool ExistsInTree(Shape s)
        {
            return Children.Exists(x => x.ExistsInTree(s));
        }

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            if (AlternativesContainer.IsAlternativesContainer(s.Name))
            {
                if (Children.Exists(x => AlternativesContainer.IsAlternativesContainer(x.Name)))
                {
                    if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
                    {
                        //TODO: Show message
                        s.DeleteEx(0);
                    }
                }
                else
                {
                    Children.Add(new AlternativesContainer(Page, s));
                }
            }
            else if (RelatedDocumentsContainer.IsRelatedDocumentsContainer(s.Name))
            {
                if (Children.Exists(x => RelatedDocumentsContainer.IsRelatedDocumentsContainer(x.Name)))
                {
                    if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
                    {
                        //TODO: Show message
                        s.DeleteEx(0);
                    }
                }
                else
                {
                    RelatedDocumentsContainer rdc = new RelatedDocumentsContainer(Page, s);
                    Children.Add(rdc);
                    if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
                    {
                        new RepaintHandler(rdc);
                    }
                }
            }
            else if (ForcesContainer.IsForcesContainer(s.Name))
            {
                if (Children.Exists(x => ForcesContainer.IsForcesContainer(x.Name)))
                {
                    if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
                    {
                        //TODO: Show message
                        s.DeleteEx(0);
                    }
                }
                else
                {
                    ForcesContainer forcesContainer = new ForcesContainer(Page, s);
                    Children.Add(forcesContainer);
                    if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
                    {
                        new RepaintHandler(forcesContainer);
                    }
                }
            }
            else if(allowAddOfSubpart)
            {
                Children.ForEach(r => r.AddToTree(s, true));
            }
        }

        public override RComponent GetComponentByShape(Shape s)
        {
            return Children.FirstOrDefault(c => c.GetComponentByShape(s) != null)?.GetComponentByShape(s);
        }
    }
}
