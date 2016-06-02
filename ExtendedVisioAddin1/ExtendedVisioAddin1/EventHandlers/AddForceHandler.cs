﻿using System.Linq;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal class AddForceHandler : MarkerEventHandler
    {
        public override void Execute(RModel model, Shape changedShape, string identifier)
        {
            ForcesContainer forcesContainer = (ForcesContainer)Globals.ThisAddIn.View.Children.First(c => c is ForcesContainer);
            forcesContainer.Children.Insert(forcesContainer.Children.Count-1,new ForceContainer(changedShape.ContainingPage));
            new RepaintHandler();
        }
    }
}