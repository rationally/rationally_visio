using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using Rationally.Visio.Model;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Forces
{
    internal class ForceTotalsRow : HeaderlessContainer
    {
        private static readonly Regex ForceTotalsRowRegex = new Regex(@"ForceTotalsRow(\.\d+)?$");

        public ForceTotalsRow(Page page) : base(page)
        {
            AddUserRow("rationallyType");
            RationallyType = "forceTotalsRow";
            Name = "ForceTotalsRow";

            MsvSdContainerLocked = true;
            InitChildren(page);
            Height = 0.33;
            InitStyle();
        }

        public ForceTotalsRow(Page page, Shape forceTotalsShape) : base(page, false)
        {
            RShape = forceTotalsShape;
            
            UsedSizingPolicy |= SizingPolicy.ShrinkYIfNeeded | SizingPolicy.ExpandXIfNeeded;
            LayoutManager = new InlineLayout(this);
            MarginBottom = 0.4;

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
                LineColor = "RGB(89,131,168)",
                LockDelete = true
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
                LineColor = "RGB(89,131,168)",
                LockDelete = true
            };
            descDummy.LinePattern = 1;
            descDummy.AddUserRow("rationallyType");
            Children.Add(descDummy);

            basicDocument.Close();
        }

        private void InitStyle()
        {
            MarginBottom = 0.4;
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
            {
                RShape.ContainerProperties.ResizeAsNeeded = 0;
                ContainerPadding = 0;
            }
            UsedSizingPolicy |= SizingPolicy.ShrinkYIfNeeded | SizingPolicy.ExpandXIfNeeded;
            LayoutManager = new InlineLayout(this);
        }

        public static bool IsForceTotalsRow(string name)
        {
            return ForceTotalsRowRegex.IsMatch(name);
        }

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            if (ForceTotalComponent.IsForceTotalComponent(s.Name))
            {
                ForceTotalComponent com = new ForceTotalComponent(Page, s);
                if (2 + com.AlternativeIndex > Children.Count)
                {
                    Children.Add(com);
                }
                else
                {
                    Children.Insert(2 + com.AlternativeIndex, com);
                }
            }
        }

        [SuppressMessage("ReSharper", "SimplifyLinqExpression")]
        public override void Repaint()
        {

            //foreach alternative in model { add a force value component, if it is not aleady there }
            List<Alternative> alternatives = Globals.ThisAddIn.Model.Alternatives;

            List<ForceTotalComponent> alreadyThere = Children.Where(c => c is ForceTotalComponent).Cast<ForceTotalComponent>().ToList();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (Alternative alt in alternatives)
            {
                //locate the header cell for the current alternative, if it exsists
                ForceTotalComponent altTotal = (ForceTotalComponent)Children.FirstOrDefault(c => (c is ForceTotalComponent && !c.Deleted && ((ForceTotalComponent)c).AlternativeTimelessId == alt.TimelessId));
                //if a deleted shape is present, there is no possiblity that we are adding an alternative. Furthermore, the deleted shape still represents an alternative, for each thus no second cell should be added!
                if (altTotal == null && Children.All(c => !c.Deleted))
                {
                    alreadyThere.Add(new ForceTotalComponent(Page, alternatives.IndexOf(alt), alt.Identifier, alt.TimelessId));
                }
            }

            //at this point, all alternatives have a component in alreadyThere, but there might be components of removed alternatives in there as well
            List<ForceTotalComponent> toRemove = alreadyThere.Where(f => !f.Deleted && !alternatives.ToList().Any(alt => alt.TimelessId == f.AlternativeTimelessId)).ToList();
            List<ForceTotalComponent> toRemoveFromTree = alreadyThere.Where(f => f.Deleted || !alternatives.ToList().Any(alt => alt.TimelessId == f.AlternativeTimelessId)).ToList();
            alreadyThere.RemoveAll(a => toRemoveFromTree.Contains(a));

            //finally, order the alternative columns similar to the alternatives container
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
            {
                alreadyThere = alreadyThere.OrderBy(fc => alternatives.IndexOf(alternatives.First(a => a.TimelessId == fc.AlternativeTimelessId))).ToList();
            }
            Children.RemoveAll(c => c is ForceTotalComponent);
            Children.AddRange(alreadyThere);

            //remove the shapes of the deleted components; undo redo do this automatically
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
            {
                MsvSdContainerLocked = false;
                toRemove.ForEach(c => c.RShape.DeleteEx(0));
                MsvSdContainerLocked = true;
            }
            base.Repaint();
        }
    }
}
