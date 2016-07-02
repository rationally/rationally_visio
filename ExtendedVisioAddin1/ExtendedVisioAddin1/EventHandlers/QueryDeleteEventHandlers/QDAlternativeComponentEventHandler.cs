﻿using System.Linq;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDAlternativeComponentEventHandler : QueryDeleteEventHandler
    {
        public override void Execute(string eventKey, RView view, Shape changedShape)
        {
            AlternativesContainer cont = (AlternativesContainer)view.Children.First(x => x is AlternativesContainer);
            foreach (AlternativeContainer alternativeContainer in cont.Children.Where(c => c is AlternativeContainer).Cast<AlternativeContainer>().ToList())
            {
                if (alternativeContainer.Children.Where(c => c.RShape.Equals(changedShape)).ToList().Count > 0) //check if this alternative contains the to be deleted component
                {
                    if (!alternativeContainer.Deleted)
                    {
                        alternativeContainer.Deleted = true;
                        alternativeContainer.RShape.Delete(); //delete the parent wrapper of s
                    }
                }
            }
        }
    }
}
