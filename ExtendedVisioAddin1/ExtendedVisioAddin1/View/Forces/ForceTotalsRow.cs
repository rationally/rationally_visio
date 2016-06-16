using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Forces
{
    class ForceTotalsRow : HeaderlessContainer
    {
        private static readonly Regex ForceTotalsRowRegex = new Regex(@"ForceTotalsRow(\.\d+)?$");

        public ForceTotalsRow(Page page) : base(page)
        {
            AddUserRow("rationallyType");
            RationallyType = "forceTotalsRow";
            Name = "ForceTotalsRow";

            InitChildren(page);
            Height = 0.33;
            InitStyle();
        }
        
        public ForceTotalsRow(Page page, Shape forceTotalsShape) : base(page, false)
        {
            RShape = forceTotalsShape;
            InitStyle();

            Array ident = forceTotalsShape.ContainerProperties.GetMemberShapes(16);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();
            if (Children.Count == 0)
            {
                foreach (Shape shape in shapes)
                {

                    if (ForceTotalComponent.IsForceTotalComponent(shape.Name))
                    {
                        Children.Add(new ForceTotalComponent(page, shape));
                    }
                    else if (shape.CellExistsU["User.rationallyType", 0] != 0)
                    {
                        RComponent toAdd = new RComponent(page)
                        {
                            RShape = shape
                        };
                        Children.Add(toAdd);
                    }
                }
            }
            
        }

        private void InitChildren(Page page)
        {
            Document basicDocument = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];

            //dummy element for concern
            RComponent concernDummy = new RComponent(page)
            {
                RShape = page.Drop(rectMaster, 0, 0),
                LinePattern = 0,
                Width = 1,
                Height = 0.33,
                Name = "ConcernDummy",
                Text = "Total:",
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                LineColor = "RGB(89,131,168)"
            };
            concernDummy.LinePattern = 1;
            concernDummy.AddUserRow("rationallyType");
            concernDummy.ToggleBoldFont(true);
            Children.Add(concernDummy);

            RComponent descDummy = new RComponent(page)
            {
                RShape = page.Drop(rectMaster, 0, 0),
                LinePattern = 0,
                Width = 2,
                Height = 0.33,
                Name = "DescDummy",
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                LineColor = "RGB(89,131,168)"
            };
            descDummy.LinePattern = 1;
            descDummy.AddUserRow("rationallyType");
            Children.Add(descDummy);

            basicDocument.Close();
        }

        private void InitStyle()
        {
            MarginBottom = 0.4;

            UsedSizingPolicy |= SizingPolicy.ShrinkYIfNeeded | SizingPolicy.ExpandXIfNeeded;
            LayoutManager = new InlineLayout(this);
        }

        public static bool IsForceTotalsRow(string name)
        {
            return ForceTotalsRowRegex.IsMatch(name);
        }

        [SuppressMessage("ReSharper", "SimplifyLinqExpression")]
        public override void Repaint()
        {
            //foreach alternative in model { add a force value component, if it is not aleady there }
            List<Alternative> alternatives = Globals.ThisAddIn.Model.Alternatives; //todo: moet dit observable? Kan het niet met list

            List<ForceTotalComponent> alreadyThere = Children.Where(c => c is ForceTotalComponent).Cast<ForceTotalComponent>().ToList();
            foreach (Alternative alt in alternatives)
            {
                if (Children.Where(c => c is ForceTotalComponent && ((ForceTotalComponent)c).AlternativeIdentifier == alt.Identifier).ToList().Count != 1)
                {
                    alreadyThere.Add(new ForceTotalComponent(Page, alt.Identifier));
                }
            }

            //at this point, all alternatives have a component in alreadyThere, but there might be components of removed alternatives in there as well
            List<ForceTotalComponent> toRemove = alreadyThere.Where(f => !alternatives.ToList().Any(alt => alt.Identifier == f.AlternativeIdentifier)).ToList();


            alreadyThere = alreadyThere.Where(f => alternatives.ToList().Any(alt => alt.Identifier == f.AlternativeIdentifier)).ToList();

            //finally, order the alternative columns similar to the alternatives container
            alreadyThere = alreadyThere.OrderBy(fc => alternatives.IndexOf(alternatives.First(a => a.Identifier == fc.AlternativeIdentifier))).ToList();

            Children.RemoveAll(c => c is ForceTotalComponent);
            Children.AddRange(alreadyThere);

            //remove the shapes of the deleted components
            toRemove.ForEach(c => c.RShape.DeleteEx(0));
            base.Repaint();
        }
    }
}
