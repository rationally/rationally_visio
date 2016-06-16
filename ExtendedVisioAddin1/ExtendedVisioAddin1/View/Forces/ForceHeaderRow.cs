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
    class ForceHeaderRow : HeaderlessContainer
    {
        private static readonly Regex ForceHeaderRowRegex = new Regex(@"ForceHeaderRow(\.\d+)?$");

        public ForceHeaderRow(Page page) : base(page)
        {
            AddUserRow("rationallyType");
            RationallyType = "forceHeaderRow";
            Name = "ForceHeaderRow";

            InitChildren(page);
            Height = 0.33;
            InitStyle();
        }
        

        public ForceHeaderRow(Page page, Shape forceHeaderShape) : base(page, false)
        {
            RShape = forceHeaderShape;
            Array ident = forceHeaderShape.ContainerProperties.GetMemberShapes(16);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();
            if (Children.Count == 0)
            {
                foreach (Shape shape in shapes)
                {

                    if (ForceAlternativeHeaderComponent.IsForceAlternativeHeaderComponent(shape.Name))
                    {
                        Children.Add(new ForceAlternativeHeaderComponent(page, shape));
                    }
                    else if (shape.CellExistsU["User.rationallyType", 0] != 0)
                    {
                        RComponent toAdd = new RComponent(page) {RShape = shape};
                        Children.Add(toAdd);
                    }
                }
            }
            InitStyle();
        }

        private void InitChildren(Page page)
        {
            Document basicDocument = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];


            RComponent concernLabel = new RComponent(page)
            {
                RShape = page.Drop(rectMaster, 0, 0),
                Text = "Concern",
                Name = "ConcernLabel",
                Width = 1,
                Height = 0.33,
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                LineColor = "RGB(89,131,168)"
            };
            concernLabel.ToggleBoldFont(true);
            concernLabel.AddUserRow("rationallyType");
            Children.Add(concernLabel);

            RComponent descLabel = new RComponent(page)
            {
                RShape = page.Drop(rectMaster, 0, 0),
                Text = "Description",
                Name = "DescriptionLabel",
                Width = 2,
                Height = 0.33,
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                LineColor = "RGB(89,131,168)"
            };
            descLabel.ToggleBoldFont(true);
            descLabel.AddUserRow("rationallyType");
            Children.Add(descLabel);

            basicDocument.Close();
        }

        private void InitStyle()
        {
            MarginTop = 0.4;
            
            UsedSizingPolicy |= SizingPolicy.ShrinkYIfNeeded | SizingPolicy.ExpandXIfNeeded;
            LayoutManager = new InlineLayout(this);
        }

        [SuppressMessage("ReSharper", "SimplifyLinqExpression")]
        public override void Repaint()
        {
            //foreach alternative in model { add a force value component, if it is not aleady there }
            List<Alternative> alternatives = Globals.ThisAddIn.Model.Alternatives; //todo: y u no list

            List<ForceAlternativeHeaderComponent> alreadyThere = Children.Where(c => c is ForceAlternativeHeaderComponent).Cast<ForceAlternativeHeaderComponent>().ToList();
            foreach (Alternative alt in alternatives)
            {
                if (Children.Where(c => c is ForceAlternativeHeaderComponent && ((ForceAlternativeHeaderComponent)c).AlternativeIdentifier == alt.Identifier).ToList().Count != 1)
                {
                    alreadyThere.Add(new ForceAlternativeHeaderComponent(Page, alt.Identifier));
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
