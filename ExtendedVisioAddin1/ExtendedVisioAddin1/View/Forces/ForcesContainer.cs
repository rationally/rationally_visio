﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Forces
{
    class ForcesContainer : RContainer
    {
        private static readonly Regex ForcesRegex = new Regex(@"Forces(\.\d+)?$");

        public ForcesContainer(Page page) : base(page)
        {
            Master containerMaster = Globals.ThisAddIn.Model.RationallyDocument.Masters["Forces"];
            RShape = Page.DropContainer(containerMaster, null);
            CenterX = 12.875;
            CenterY = 8.375;

            Name = "Forces";
            
            InitStyle();
        }

        public ForcesContainer(Page page, Shape forcesContainer) : base(page)
        {
            RShape = forcesContainer;
            Array ident = forcesContainer.ContainerProperties.GetMemberShapes(16);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();

            foreach (Shape shape in shapes)
            {
                if (ForceHeaderRow.IsForceHeaderRow(shape.Name))
                {
                    Children.Add(new ForceHeaderRow(page, shape));
                    continue;
                }
                if (ForceContainer.IsForceContainer(shape.Name))
                {
                    Children.Add(new ForceContainer(page, shape));
                    continue;
                }
                if (ForceTotalsRow.IsForceTotalsRow(shape.Name))
                {
                    Children.Add(new ForceTotalsRow(page,shape));
                    continue;
                }
            }
            //insert header, if it is absent
            if (Children.Count == 0 || !Children.Any(c => c is ForceHeaderRow))
            {
                this.Children.Insert(0, new ForceHeaderRow(Page));
            }
            //insert footer, if it is absent
            if (Children.Count == 0 || !Children.Any(c => c is ForceTotalsRow))
            {
                this.Children.Add(new ForceTotalsRow(Page));
            } else if (Children.Any(c => c is ForceTotalsRow))
            {
                RComponent toMove = Children.First(c => c is ForceTotalsRow);
                int toMoveIndex = Children.IndexOf(toMove);
                RComponent toSwapWith = Children.Last();
                Children[Children.Count - 1] = toMove;
                Children[toMoveIndex] = toSwapWith;
            }

            InitStyle();
        }

        private void InitStyle()
        {
            this.UsedSizingPolicy |= SizingPolicy.ShrinkYIfNeeded | SizingPolicy.ExpandYIfNeeded;
            
            LayoutManager = new VerticalStretchLayout(this);
        }

        public static bool IsForcesContainer(string name)
        {
            return ForcesRegex.IsMatch(name);
        }

        public void Repaint()
        {
            
            base.Repaint();
        }
    }
}
