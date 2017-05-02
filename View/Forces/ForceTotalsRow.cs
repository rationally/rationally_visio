using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.RationallyConstants;

namespace Rationally.Visio.View.Forces
{
    internal class ForceTotalsRow : HeaderlessContainer
    {
        private static readonly Regex ForceTotalsRowRegex = new Regex(@"ForceTotalsRow(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ForceTotalsRow(Page page) : base(page)
        {
            RationallyType = "forceTotalsRow";
            Name = "ForceTotalsRow";

            MsvSdContainerLocked = true;
            InitChildren(page);
            Height = 0.33;
            InitStyle();
        }

        public ForceTotalsRow(Page page, Shape forceTotalsShape) : base(page, false)
        {
            Shape = forceTotalsShape;
            
            UsedSizingPolicy |= SizingPolicy.ShrinkYIfNeeded | SizingPolicy.ExpandXIfNeeded;
            LayoutManager = new InlineLayout(this);
            MarginBottom = 0.4;

            Array ident = forceTotalsShape.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsExcludeNested);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();
            if (Children.Count == 0)
            {
                foreach (Shape shape in shapes)
                {

                    if (ForceTotalComponent.IsForceTotalComponent(shape.Name))
                    {
                        Children.Add(new ForceTotalComponent(page, shape));
                    }
                    else if (shape.CellExistsU[CellConstants.RationallyType, (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists)
                    {
                        VisioShape toAdd = new VisioShape(page)
                        {
                            Shape = shape
                        };
                        Children.Add(toAdd);
                    }
                }
            }

        }

        private void InitChildren(Page page)
        {
            Document basicDocument = Globals.RationallyAddIn.Application.Documents.OpenEx(VisioFormulas.BasicStencil, (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];

            //dummy element for concern
            VisioShape concernDummy = new VisioShape(page)
            {
                Shape = page.Drop(rectMaster, 0, 0),
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
            concernDummy.RationallyType = "concernDummy";
            concernDummy.ToggleBoldFont(true);
            Children.Add(concernDummy);

            VisioShape descDummy = new VisioShape(page)
            {
                Shape = page.Drop(rectMaster, 0, 0),
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
            descDummy.RationallyType = "descDummy";
            Children.Add(descDummy);

            basicDocument.Close();
        }

        private void InitStyle()
        {
            MarginBottom = 0.4;
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                Shape.ContainerProperties.ResizeAsNeeded = 0;
                ContainerPadding = 0;
            }
            UsedSizingPolicy |= SizingPolicy.ShrinkYIfNeeded | SizingPolicy.ExpandXIfNeeded;
            LayoutManager = new InlineLayout(this);
        }

        public static bool IsForceTotalsRow(string name) => ForceTotalsRowRegex.IsMatch(name);

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            if (ForceTotalComponent.IsForceTotalComponent(s.Name))
            {
                ForceTotalComponent com = new ForceTotalComponent(Page, s);
                if ((2 + com.Index) > Children.Count)
                {
                    Children.Add(com);
                }
                else
                {
                    Children.Insert(2 + com.Index, com);
                }
            }
        }

        [SuppressMessage("ReSharper", "SimplifyLinqExpression")]
        public override void Repaint()
        {

            //foreach alternative in model { add a force value component, if it is not aleady there }
            List<Alternative> alternatives = Globals.RationallyAddIn.Model.Alternatives;

            List<ForceTotalComponent> alreadyThere = Children.Where(c => c is ForceTotalComponent).Cast<ForceTotalComponent>().ToList();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (Alternative alt in alternatives)
            {
                //locate the header cell for the current alternative, if it exsists
                ForceTotalComponent altTotal = (ForceTotalComponent)Children.FirstOrDefault(c => c is ForceTotalComponent && !c.Deleted && (((ForceTotalComponent)c).ForceAlternativeId == alt.Id));
                //if a deleted shape is present, there is no possiblity that we are adding an alternative. Furthermore, the deleted shape still represents an alternative, for each thus no second cell should be added!
                if ((altTotal == null) && Children.All(c => !c.Deleted))
                {
                    alreadyThere.Add(new ForceTotalComponent(Page, alternatives.IndexOf(alt), alt.IdentifierString, alt.Id));
                }
            }

            //at this point, all alternatives have a component in alreadyThere, but there might be components of removed alternatives in there as well
            List<ForceTotalComponent> toRemove = alreadyThere.Where(f => !f.Deleted && !alternatives.ToList().Any(alt => alt.Id == f.ForceAlternativeId)).ToList();
            List<ForceTotalComponent> toRemoveFromTree = alreadyThere.Where(f => f.Deleted || !alternatives.ToList().Any(alt => alt.Id == f.ForceAlternativeId)).ToList();
            alreadyThere.RemoveAll(a => toRemoveFromTree.Contains(a));

            //finally, order the alternative columns similar to the alternatives container
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                alreadyThere = alreadyThere.OrderBy(fc => alternatives.IndexOf(alternatives.First(a => a.Id == fc.ForceAlternativeId))).ToList();
            }
            Children.RemoveAll(c => c is ForceTotalComponent);
            Children.AddRange(alreadyThere);

            if (!Deleted) //no need to change the shape if it exists no more
            {
                //remove the shapes of the deleted components; undo redo do this automatically
                if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                {
                    MsvSdContainerLocked = false;
                    toRemove.ForEach(c => c.Shape.DeleteEx((short) VisDeleteFlags.visDeleteNormal));
                    MsvSdContainerLocked = true;
                }

                base.Repaint();
            }
        }
    }
}
