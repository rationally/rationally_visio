﻿using System;
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
    internal class ForceHeaderRow : HeaderlessContainer
    {
        private static readonly Regex ForceHeaderRowRegex = new Regex(@"ForceHeaderRow(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ForceHeaderRow(Page page) : base(page)
        {
            RationallyType = "forceHeaderRow";
            Name = "ForceHeaderRow";

            MsvSdContainerLocked = true;
            InitChildren(page);
            Height = 0.33;
            InitStyle();
        }


        public ForceHeaderRow(Page page, Shape forceHeaderShape) : base(page, false)
        {
            Shape = forceHeaderShape;
            Array ident = forceHeaderShape.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsExcludeNested);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();
            if (Children.Count == 0)
            {
                foreach (Shape shape in shapes)
                {

                    if (ForceAlternativeHeaderComponent.IsForceAlternativeHeaderComponent(shape.Name))
                    {
                        Children.Add(new ForceAlternativeHeaderComponent(page, shape));
                    }
                    else if (shape.CellExistsU[VisioFormulas.Cell_RationallyType, (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists)
                    {
                        VisioShape toAdd = new VisioShape(page) { Shape = shape };
                        Children.Add(toAdd);
                    }
                }
            }
            MarginTop = 0.4;
            UsedSizingPolicy |= SizingPolicy.ShrinkYIfNeeded | SizingPolicy.ExpandXIfNeeded;
            LayoutManager = new InlineLayout(this);
        }

        private void InitChildren(Page page)
        {
            Document basicDocument = Globals.RationallyAddIn.Application.Documents.OpenEx(VisioFormulas.BasicStencil, (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];


            VisioShape concernLabel = new VisioShape(page)
            {
                Shape = page.Drop(rectMaster, 0, 0),
                Text = "Concern",
                Name = "ConcernLabel",
                Width = 1,
                Height = 0.33,
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                LineColor = "RGB(89,131,168)"
            };
            concernLabel.ToggleBoldFont(true);
            concernLabel.RationallyType = "concernLabel";
            Children.Add(concernLabel);

            VisioShape descLabel = new VisioShape(page)
            {
                Shape = page.Drop(rectMaster, 0, 0),
                Text = "Description",
                Name = "DescriptionLabel",
                Width = 2,
                Height = 0.33,
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                LineColor = "RGB(89,131,168)"
            };
            descLabel.ToggleBoldFont(true);
            descLabel.RationallyType = "descLabel";
            Children.Add(descLabel);

            basicDocument.Close();
        }

        private void InitStyle()
        {
            MarginTop = 0.4;
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                Shape.ContainerProperties.ResizeAsNeeded = 0;
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
                int index = com.Shape.Text[0] - 63;//text is of the form "A:"; A = 65 and should be inserted at index 2, after the concern and desc column
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
            List<Alternative> alternatives = Globals.RationallyAddIn.Model.Alternatives;
            List<ForceAlternativeHeaderComponent> alreadyThere = Children.Where(c => c is ForceAlternativeHeaderComponent).Cast<ForceAlternativeHeaderComponent>().ToList();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (Alternative alt in alternatives)
            {
                //locate the header cell for the current alternative, if it exsists
                ForceAlternativeHeaderComponent altHeader = (ForceAlternativeHeaderComponent)Children.FirstOrDefault(c => c is ForceAlternativeHeaderComponent && !c.Deleted && (((ForceAlternativeHeaderComponent) c).ForceAlternativeId == alt.Id));
                //if a deleted shape is present, there is no possiblity that we are adding an alternative. Furthermore, the deleted shape still represents an alternative, for each thus no second cell should be added!
                if ((altHeader == null) && Children.All(c => !c.Deleted)) 
                {
                    alreadyThere.Add(new ForceAlternativeHeaderComponent(Page, alt.IdentifierString, alt.Id));
                }
            }

            //at this point, all alternatives have a component in alreadyThere, but there might be components of removed alternatives in there as well
            List<ForceAlternativeHeaderComponent> toRemove = alreadyThere.Where(f => !f.Deleted && !alternatives.ToList().Any(alt => alt.Id == f.ForceAlternativeId)).ToList();
            List<ForceAlternativeHeaderComponent> toRemoveFromTree = alreadyThere.Where(f => f.Deleted || !alternatives.ToList().Any(alt => alt.Id == f.ForceAlternativeId)).ToList();
            alreadyThere.RemoveAll(a => toRemoveFromTree.Contains(a));
            //finally, order the alternative columns similar to the alternatives container
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                alreadyThere = alreadyThere.OrderBy(fc => alternatives.IndexOf(alternatives.First(a => a.Id == fc.ForceAlternativeId))).ToList();
            }
            Children.RemoveAll(c => c is ForceAlternativeHeaderComponent);
            Children.AddRange(alreadyThere);

            //remove the shapes of the deleted components; undo redo do this automatically
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                MsvSdContainerLocked = false;
                toRemove.ForEach(c => c.Shape.DeleteEx((short)VisDeleteFlags.visDeleteNormal));
                MsvSdContainerLocked = true;
            }
            base.Repaint();
        }

        public static bool IsForceHeaderRow(string name) => ForceHeaderRowRegex.IsMatch(name);
    }
}
