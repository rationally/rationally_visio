﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.EventHandlers;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    public class RView : RContainer, Model.IObserver<RModel>
    {
        public AlternativesContainer AlternativesContainer { get; }
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

            AlternativesContainer alternativesContainer = (AlternativesContainer)Children.First(c => c is AlternativesContainer);
            ((RContainer)Globals.ThisAddIn.View.Children.First(ch => ch is AlternativesContainer)).Children.Add(new AlternativeContainer(Globals.ThisAddIn.Application.ActivePage, Globals.ThisAddIn.model.Alternatives.Count - 1, alternative));
            new RepaintHandler();
        }

        /// <summary>
        /// Deletes an alternative container from the view.
        /// </summary>
        /// <param name="index">identifier of the alternative.</param>
        public void DeleteAlternative(int index)
        {
            AlternativesContainer alternativesContainer = (AlternativesContainer)Children.First(c => c is AlternativesContainer);
            AlternativeContainer alternative = (AlternativeContainer) alternativesContainer.Children.First(x => x.AlternativeIndex == index && x is AlternativeContainer);
            alternativesContainer.Children.Remove(alternative);
            alternative.RShape.DeleteEx(0); //deletes the alternative, and it's child components
            new RepaintHandler();
        }
    }
}
