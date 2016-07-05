using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Forces
{
    internal class ForceHeaderRow : HeaderlessContainer
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
                        RComponent toAdd = new RComponent(page) { RShape = shape };
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
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
            {
                RShape.ContainerProperties.ResizeAsNeeded = 0;
                ContainerPadding = 0;
            }
            UsedSizingPolicy |= SizingPolicy.ShrinkYIfNeeded | SizingPolicy.ExpandXIfNeeded;
            LayoutManager = new InlineLayout(this);
        }

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            if (ForceAlternativeHeaderComponent.IsForceAlternativeHeaderComponent(s.Name))
            {
                ForceAlternativeHeaderComponent com = new ForceAlternativeHeaderComponent(Page, s);
                int index = com.RShape.Text[0] - 63;//text is of the form "A:"; A = 65 and should be inserted at index 2, after the concern and desc column
                if (Children.Count < index)
                {
                    Children.Add(com);
                }
                else
                {
                    Children.Insert(index, com);
                }
            }
        }

        [SuppressMessage("ReSharper", "SimplifyLinqExpression")]
        public override void Repaint()
        {

            //foreach alternative in model { add a force value component, if it is not aleady there }
            List<Alternative> alternatives = Globals.ThisAddIn.Model.Alternatives;

            List<ForceAlternativeHeaderComponent> alreadyThere = Children.Where(c => c is ForceAlternativeHeaderComponent).Cast<ForceAlternativeHeaderComponent>().ToList();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (Alternative alt in alternatives)
            {
                if (Children.Where(c => (c is ForceAlternativeHeaderComponent && !c.Deleted && ((ForceAlternativeHeaderComponent)c).AlternativeTimelessId == alt.TimelessId)).ToList().Count != 1)
                {
                    alreadyThere.Add(new ForceAlternativeHeaderComponent(Page, alt.Identifier, alt.TimelessId));
                }
            }

            //at this point, all alternatives have a component in alreadyThere, but there might be components of removed alternatives in there as well
            List<ForceAlternativeHeaderComponent> toRemove = alreadyThere.Where(f => !alternatives.ToList().Any(alt => f.Deleted || alt.TimelessId == f.AlternativeTimelessId)).ToList();

            //alreadyThere = alreadyThere - toRemove
            alreadyThere = alreadyThere.Where(f => alternatives.ToList().Any(alt => !f.Deleted && alt.TimelessId == f.AlternativeTimelessId)).ToList();

            //finally, order the alternative columns similar to the alternatives container
            alreadyThere = alreadyThere.OrderBy(fc => alternatives.IndexOf(alternatives.First(a => a.TimelessId == fc.AlternativeTimelessId))).ToList();

            Children.RemoveAll(c => c is ForceAlternativeHeaderComponent);
            Children.AddRange(alreadyThere);

            //remove the shapes of the deleted components; undo redo do this automatically
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
            {
                toRemove.ForEach(c => c.RShape.DeleteEx(0));
            }
            base.Repaint();

        }

        public static bool IsForceHeaderRow(string name)
        {
            return ForceHeaderRowRegex.IsMatch(name);
        }
    }
}
