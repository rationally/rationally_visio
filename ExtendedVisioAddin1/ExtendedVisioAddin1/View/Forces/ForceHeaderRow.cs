﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Forces
{
    class ForceHeaderRow : HeaderlessContainer
    {
        private static readonly Regex ForceHeaderRowRegex = new Regex(@"ForceHeaderRow(\.\d+)?$");
        private Shape shape;

        public ForceHeaderRow(Page page) : base(page)
        {
            this.AddUserRow("rationallyType");
            this.RationallyType = "forceHeaderRow";
            this.Name = "ForceHeaderRow";

            InitChildren(page);
            InitStyle();
        }

        public ForceHeaderRow(Page page, bool makeShape) : base(page, makeShape)
        {
            InitChildren(page);
            InitStyle();
        }

        public ForceHeaderRow(Page page, Shape shape) : this(page)
        {
            RShape = shape;//TODO subelements
        }

        private void InitChildren(Page page)
        {
            Document basicDocument = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            

            RComponent concernLabel = new RComponent(page);
            concernLabel.RShape = page.Drop(rectMaster, 0, 0);
            concernLabel.Text = "Concern";
            concernLabel.ToggleBoldFont(true);
            concernLabel.Width = 1;
            concernLabel.Height = 0.33;
            Children.Add(concernLabel);

            RComponent descLabel = new RComponent(page);
            descLabel.RShape = page.Drop(rectMaster, 0, 0);
            descLabel.Text = "Description";
            descLabel.ToggleBoldFont(true);
            descLabel.Width = 2;
            descLabel.Height = 0.33;
            Children.Add(descLabel);

            basicDocument.Close();
        }

        private void InitStyle()
        {
            this.MarginTop = 0.4;
            this.Height = 0.33;
            this.UsedSizingPolicy |= SizingPolicy.ShrinkYIfNeeded | SizingPolicy.ExpandXIfNeeded;
            this.LayoutManager = new InlineLayout(this);
        }

        public override void Repaint()
        {
            //foreach alternative in model { add a force value component, if it is not aleady there }
            ObservableCollection<Alternative> alternatives = Globals.ThisAddIn.Model.Alternatives;

            List<ForceAlternativeHeaderComponent> alreadyThere = Children.Where(c => c is ForceAlternativeHeaderComponent).Cast<ForceAlternativeHeaderComponent>().ToList();
            foreach (Alternative alt in alternatives)
            {
                if (Children.Where(c => c is ForceAlternativeHeaderComponent && ((ForceAlternativeHeaderComponent)c).AlternativeIdentifier == alt.Identifier).ToList().Count != 1)
                {
                    alreadyThere.Add(new ForceAlternativeHeaderComponent(Globals.ThisAddIn.Application.ActivePage, alt.Identifier));
                }
            }

            //at this point, all alternatives have a component in alreadyThere, but there might be components of removed alternatives in there as well
            List<ForceAlternativeHeaderComponent> toRemove = alreadyThere.Where(f => !alternatives.ToList().Any(alt => alt.Identifier == f.AlternativeIdentifier)).ToList();


            alreadyThere = alreadyThere.Where(f => alternatives.ToList().Any(alt => alt.Identifier == f.AlternativeIdentifier)).ToList();

            //finally, order the alternative columns similar to the alternatives container
            alreadyThere = alreadyThere.OrderBy(fc => alternatives.IndexOf(alternatives.First(a => a.Identifier == fc.AlternativeIdentifier))).ToList();

            Children.RemoveAll(c => c is ForceAlternativeHeaderComponent);
            Children.AddRange(alreadyThere);

            //remove the shapes of the deleted components
            toRemove.ForEach(c => c.RShape.DeleteEx(0));
            base.Repaint();
        }

        public static bool IsForceHeaderRow(string name)
        {
            return ForceHeaderRowRegex.IsMatch(name);
        }
    }
}