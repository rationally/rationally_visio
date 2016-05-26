using System.Linq;
using ExtendedVisioAddin1.EventHandlers;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View.Alternatives;
using ExtendedVisioAddin1.View.Documents;
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

            AlternativeContainer alternative = (AlternativeContainer) alternativesContainer?.Children.FirstOrDefault(x => x.AlternativeIndex == index && x is AlternativeContainer); //Return null if no container or no alternative with index
            if (alternative != null)
            {
                alternativesContainer.Children.Remove(alternative);
                if (deleteShape)
                {
                    alternative.RShape.DeleteEx(0); //deletes the alternative, and it's child components. This should not be done when the shape is already gone, such as when the user has deleted it himself.
                }
                int i = 0;
                alternativesContainer.Children.Where(c => c is AlternativeContainer).ToList().ForEach(c => ((AlternativeContainer) c).SetAlternativeIdentifier(i++));
            }
            new RepaintHandler();
        }

        public void DeleteAlternativesContainerByUser()
        {
            AlternativesContainer alternativesContainer = (AlternativesContainer)Children.First(c => c is AlternativesContainer); //We know there exists only one.
            Children.Remove(alternativesContainer);
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
                Children.Add(new AlternativesContainer(Page, s));
            } else if (RelatedDocumentsContainer.IsRelatedDocumentsContainer(s.Name))
            {
                Children.Add(new RelatedDocumentsContainer(Page, s));
            } else if (false)
            {
                /*todo: forces*/
            }
            else
            {
                Children.ForEach(r => r.AddToTree(s));
            }
        }
    }
}
