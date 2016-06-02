using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
            this.AddUserRow("rationallyType");
            this.RationallyType = "forceTotalsRow";
            this.Name = "ForceTotalsRow";

            InitChildren(page);
            InitStyle();
        }

        public ForceTotalsRow(Page page, bool makeShape) : base(page, makeShape)
        {
            InitChildren(page);
            InitStyle();
        }

        public ForceTotalsRow(Page page, Shape forceTotalsShape) : this(page)
        {
            RShape = forceTotalsShape;
            Array ident = forceTotalsShape.ContainerProperties.GetMemberShapes(16);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();
            foreach (Shape shape in shapes)
            {
                if (ForceTotalComponent.IsForceTotalComponent(shape.Name))
                {
                    Children.Add(new ForceTotalComponent(page,shape));
                }
                if (shape.CellExistsU["User.rationallyType", 0] != 0)
                {
                    RComponent toAdd = new RComponent(page);
                    toAdd.RShape = shape;
                    Children.Add(toAdd);
                }
            }
        }

        private void InitChildren(Page page)
        {
            Document basicDocument = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];

            //dummy element for concern
            RComponent concernDummy = new RComponent(page);
            concernDummy.RShape = page.Drop(rectMaster, 0, 0);
            concernDummy.LinePattern = 0;
            concernDummy.Width = 1;
            concernDummy.Height = 0.33;
            concernDummy.LinePattern = 1;
            concernDummy.AddUserRow("rationallyType");
            this.Children.Add(concernDummy);

            RComponent descDummy = new RComponent(page);
            descDummy.RShape = page.Drop(rectMaster, 0, 0);
            descDummy.LinePattern = 0;
            descDummy.Width = 2;
            descDummy.Height = 0.33;
            descDummy.LinePattern = 1;
            descDummy.AddUserRow("rationallyType");
            this.Children.Add(descDummy);

            basicDocument.Close();
        }

        private void InitStyle()
        {
            this.MarginBottom = 0.4;
            this.Height = 0.33;
            this.UsedSizingPolicy |= SizingPolicy.ShrinkYIfNeeded | SizingPolicy.ExpandXIfNeeded;
            this.LayoutManager = new InlineLayout(this);
        }

        public static bool IsForceTotalsRow(string name)
        {
            return ForceTotalsRowRegex.IsMatch(name);
        }

        public override void Repaint()
        {
            //foreach alternative in model { add a force value component, if it is not aleady there }
            ObservableCollection<Alternative> alternatives = Globals.ThisAddIn.Model.Alternatives;

            List<ForceTotalComponent> alreadyThere = Children.Where(c => c is ForceTotalComponent).Cast<ForceTotalComponent>().ToList();
            foreach (Alternative alt in alternatives)
            {
                if (Children.Where(c => c is ForceTotalComponent && ((ForceTotalComponent)c).AlternativeIdentifier == alt.Identifier).ToList().Count != 1)
                {
                    alreadyThere.Add(new ForceTotalComponent(Globals.ThisAddIn.Application.ActivePage, alt.Identifier));
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
