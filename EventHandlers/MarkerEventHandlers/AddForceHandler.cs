﻿using System.Linq;
using System.Reflection;
using log4net;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class AddForceHandler : IMarkerEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(Shape changedShape, string identifier)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            ForcesContainer forcesContainer = (ForcesContainer)Globals.RationallyAddIn.View.Children.First(c => c is ForcesContainer);
            if (forcesContainer.Children.Count == 0)
            {
                //insert header, if it is absent
                if ((forcesContainer.Children.Count == 0) || !forcesContainer.Children.Any(c => c is ForceHeaderRow))
                {
                    forcesContainer.Children.Insert(0, new ForceHeaderRow(Globals.RationallyAddIn.Application.ActivePage));
                }
                //insert footer, if it is absent
                if ((forcesContainer.Children.Count == 0) || !forcesContainer.Children.Any(c => c is ForceTotalsRow))
                {
                    forcesContainer.Children.Add(new ForceTotalsRow(Globals.RationallyAddIn.Application.ActivePage));
                }
                else if (forcesContainer.Children.Any(c => c is ForceTotalsRow))
                {
                    RationallyComponent toMove = forcesContainer.Children.First(c => c is ForceTotalsRow);
                    int toMoveIndex = forcesContainer.Children.IndexOf(toMove);
                    RationallyComponent toSwapWith = forcesContainer.Children.Last();
                    forcesContainer.Children[forcesContainer.Children.Count - 1] = toMove;
                    forcesContainer.Children[toMoveIndex] = toSwapWith;
                }
            }
            forcesContainer.Children.Insert(forcesContainer.Children.Count-1,new ForceContainer(Globals.RationallyAddIn.Application.ActivePage, forcesContainer.Children.Count-2, true));
            //update the model as well
            model.Forces.Add(new Force());
            RepaintHandler.Repaint(forcesContainer);
        }
    }
}
