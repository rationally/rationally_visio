using System.Linq;
using ExtendedVisioAddin1.EventHandlers;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View.Alternatives;
using ExtendedVisioAddin1.View.Documents;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    public class RView : RContainer, IObserver<RModel>
    {
        public RView(Page page) : base(page)
        {

        }

        public void Notify(RModel model)
        {
            UpdateAlternatives(model);
        }

        private void UpdateAlternatives(RModel model)
        {
            //trace the alternatives container

        }

        public void AddAlternative(Alternative alternative)
        {
            //todo: first is wrong, since we don;t know how many
            ((RContainer)Globals.ThisAddIn.View.Children.First(ch => ch is AlternativesContainer)).Children.Add(new AlternativeContainer(Globals.ThisAddIn.Application.ActivePage, Globals.ThisAddIn.Model.Alternatives.Count - 1, alternative));
            new RepaintHandler();
        }

        /// <summary>
        /// Deletes an alternative container from the view.
        /// </summary>
        /// <param name="index">identifier of the alternative.</param>
        /// <param name="deleteShape">Shape was already deleted</param>
        public void DeleteAlternative(int index, bool deleteShape)
        {
            AlternativesContainer alternativesContainer = (AlternativesContainer)Children.FirstOrDefault(c => c is AlternativesContainer); //May have been deleted by the user

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
            }
            new RepaintHandler();
        }

        public override bool ExistsInTree(Shape s)
        {
            return Children.Exists(x => x.ExistsInTree(s));
        }

        public override void AddToTree(Shape s)
        {
            if (AlternativesContainer.IsAlternativesContainer(s.Name))
            {
                if (Children.Exists(x => AlternativesContainer.IsAlternativesContainer(x.Name)))
                {
                    //TODO: Show message
                    s.DeleteEx(0);
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
                    //TODO: Show message
                    s.DeleteEx(0);
                }
                else
                {
                    Children.Add(new RelatedDocumentsContainer(Page, s));
                }
            }
            else if (ForcesContainer.IsForcesContainer(s.Name))
            {
                if (Children.Exists(x => ForcesContainer.IsForcesContainer(x.Name)))
                {
                    //TODO: Show message
                    s.DeleteEx(0);
                }
                else
                {
                    Children.Add(new ForcesContainer(Page, s));
                }
            }
            else
            {
                Children.ForEach(r => r.AddToTree(s));
            }
        }

        public override RComponent GetComponentByShape(Shape s)
        {

            foreach (RComponent c in Children)
            {
                if (c.GetComponentByShape(s) != null)
                {
                    return c.GetComponentByShape(s);
                }
            }

            return null;
        }
    }
}
