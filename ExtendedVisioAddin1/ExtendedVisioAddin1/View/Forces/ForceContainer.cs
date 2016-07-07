using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Forces
{
    internal sealed class ForceContainer : HeaderlessContainer
    {
        private static readonly Regex ForceContaineRegex = new Regex(@"ForceContainer(\.\d+)?$");

        public ForceContainer(Page page, int forceIndex, bool makeChildren) : base(page)
        {
            AddUserRow("forceIndex");
            ForceIndex = forceIndex;
            if (makeChildren)
            {
                ForceConcernComponent concern = new ForceConcernComponent(page, forceIndex);
                Children.Add(concern);

                ForceDescriptionComponent description = new ForceDescriptionComponent(page, forceIndex);
                Children.Add(description);
            }
            AddUserRow("rationallyType");
            RationallyType = "forceContainer";
            Name = "ForceContainer";

            AddAction("addForce", "QUEUEMARKEREVENT(\"add\")", "\"Add force\"", false);
            AddAction("deleteForce", "QUEUEMARKEREVENT(\"delete\")", "\"Delete this force\"", false);

            MsvSdContainerLocked = true;
            Height = 0.33;
            UsedSizingPolicy |= SizingPolicy.ExpandXIfNeeded;
            InitStyle();
        }


        public ForceContainer(Page page, Shape forceContainer) : base(page, false)
        {
            RShape = forceContainer;
            Array ident = forceContainer.ContainerProperties.GetMemberShapes(16);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();
            string concern = null;
            string description = null;
            if (Children.Count == 0)
            {
                foreach (Shape shape in shapes)
                {

                    if (ForceConcernComponent.IsForceConcern(shape.Name))
                    {
                        Children.Add(new ForceConcernComponent(page, shape));
                        concern = shape.Text;
                    }
                    else if (ForceDescriptionComponent.IsForceDescription(shape.Name))
                    {
                        Children.Add(new ForceDescriptionComponent(page, shape));
                        description = shape.Text;
                    }
                    else if (ForceValueComponent.IsForceValue(shape.Name))
                    {
                        Children.Add(new ForceValueComponent(page, shape));
                    }
                }

                if (concern != null && description != null)
                {
                    if (ForceIndex <= Globals.ThisAddIn.Model.Forces.Count)
                    {
                        Globals.ThisAddIn.Model.Forces.Insert(ForceIndex, new Force(concern, description));
                    }
                    else
                    {
                        Globals.ThisAddIn.Model.Forces.Add(new Force(concern, description));
                    }
                }
            }
            //InitStyle();
            UsedSizingPolicy |= SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkYIfNeeded;
            LayoutManager = new InlineLayout(this);
        }

        private void InitStyle()
        {
            UsedSizingPolicy |= SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkYIfNeeded;
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
            {
                RShape.ContainerProperties.ResizeAsNeeded = 0;
                ContainerPadding = 0;
            }
            LayoutManager = new InlineLayout(this);
        }

        private void UpdateReorderFunctions()
        {
            AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "\"Move up\"", false);
            AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "\"Move down\"", false);
            if (ForceIndex == 0)
            {
                DeleteAction("moveUp");
            }

            if (ForceIndex == Globals.ThisAddIn.Model.Forces.Count - 1)
            {
                DeleteAction("moveDown");
            }
        }

        [SuppressMessage("ReSharper", "SimplifyLinqExpression")]
        public override void Repaint()
        {
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing) //Visio does this for us
            {
                UpdateReorderFunctions();
            }
            //foreach alternative in model { add a force value component, if it is not aleady there }
            List<Alternative> alternatives = Globals.ThisAddIn.Model.Alternatives;

            List<ForceValueComponent> alreadyThere = Children.Where(c => c is ForceValueComponent).Cast<ForceValueComponent>().ToList();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (Alternative alt in alternatives)
            {

                //locate the header cell for the current alternative, if it exsists
                ForceValueComponent altValue = (ForceValueComponent)Children.FirstOrDefault(c => (c is ForceValueComponent && !c.Deleted && ((ForceValueComponent)c).AlternativeTimelessId == alt.TimelessId));
                //if a deleted shape is present, there is no possiblity that we are adding an alternative. Furthermore, the deleted shape still represents an alternative, for each thus no second cell should be added!
                if (altValue == null && Children.All(c => !c.Deleted))
                {
                    alreadyThere.Add(new ForceValueComponent(Page, alt.TimelessId, alt.Identifier, ForceIndex));
                }
            }

            //at this point, all alternatives have a component in alreadyThere, but there might be components of removed alternatives in there as well
            List<ForceValueComponent> toRemove = alreadyThere.Where(f => !f.Deleted && !alternatives.ToList().Any(alt => alt.TimelessId == f.AlternativeTimelessId)).ToList();
            List<ForceValueComponent> toRemoveFromTree = alreadyThere.Where(f => f.Deleted || !alternatives.ToList().Any(alt => alt.TimelessId == f.AlternativeTimelessId)).ToList();
            alreadyThere.RemoveAll(a => toRemoveFromTree.Contains(a));
            //finally, order the alternative columns similar to the alternatives container
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
            {
                alreadyThere = alreadyThere.OrderBy(fc => alternatives.IndexOf(alternatives.First(a => a.TimelessId == fc.AlternativeTimelessId))).ToList();
            }
            Children.RemoveAll(c => c is ForceValueComponent);
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

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            if (ForceConcernComponent.IsForceConcern(s.Name))
            {
                ForceConcernComponent com = new ForceConcernComponent(Page, s);
                if (com.ForceIndex == ForceIndex)
                {
                    Children.Add(com);
                }
            }
            else if (ForceDescriptionComponent.IsForceDescription(s.Name))
            {
                ForceDescriptionComponent com = new ForceDescriptionComponent(Page, s);
                if (com.ForceIndex == ForceIndex)
                {
                    Children.Add(com);
                }
            }
            else if (ForceValueComponent.IsForceValue(s.Name))
            {
                ForceValueComponent com = new ForceValueComponent(Page, s);
                if (com.ForceIndex == ForceIndex)
                {
                    Children.Add(com);
                }
            }
        }

        public void SetForceIdentifier(int forceIndex)
        {
            Children.ForEach(c => c.ForceIndex = forceIndex);
            ForceIndex = forceIndex;
            InitStyle();
        }

        public static bool IsForceContainer(string name)
        {
            return ForceContaineRegex.IsMatch(name);
        }

    }
}
