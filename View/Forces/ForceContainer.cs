using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.View.Forces
{
    internal sealed class ForceContainer : HeaderlessContainer
    {
        private static readonly Regex ForceContaineRegex = new Regex(@"ForceContainer(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ForceContainer(Page page, int index, int forceId) : base(page)
        {
            AddUserRow("index");
            Index = index;
            AddUserRow("uniqueId");
            Id = forceId;
                ForceConcernComponent concern = new ForceConcernComponent(page, index);
                Children.Add(concern);

                ForceDescriptionComponent description = new ForceDescriptionComponent(page, index);
                Children.Add(description);
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
            Shape = forceContainer;
            Array ident = forceContainer.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsExcludeNested);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();
            string concern = null;
            string description = null;
            Dictionary<int, string> forceValuesDictionary = new Dictionary<int, string>();
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
                        ForceValueComponent comp = new ForceValueComponent(page, shape);
                        Children.Add(comp);
                        forceValuesDictionary.Add(comp.ForceAlternativeId, comp.Text);
                    }
                }

                if ((concern != null) && (description != null))
                {
                    Force correspondingForce = new Force(concern, description, forceValuesDictionary, Id);
                    if (Index <= Globals.RationallyAddIn.Model.Forces.Count)
                    {
                        Globals.RationallyAddIn.Model.Forces.Insert(Index, correspondingForce);
                    }
                    else
                    {
                        Globals.RationallyAddIn.Model.Forces.Add(correspondingForce);
                    }
                }
            }
            UsedSizingPolicy |= SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkYIfNeeded;
            LayoutManager = new InlineLayout(this);
        }

        private void InitStyle()
        {
            UsedSizingPolicy |= SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkYIfNeeded;
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                Shape.ContainerProperties.ResizeAsNeeded = 0;
                ContainerPadding = 0;
            }
            LayoutManager = new InlineLayout(this);
        }

        [SuppressMessage("ReSharper", "SimplifyLinqExpression")]
        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //Visio does this for us
            {
                UpdateReorderFunctions(Globals.RationallyAddIn.Model.Forces.Count - 1);
            }
            //foreach alternative in model { add a force value component, if it is not aleady there }
            List<Alternative> alternatives = Globals.RationallyAddIn.Model.Alternatives;

            List<ForceValueComponent> alreadyThere = Children.Where(c => c is ForceValueComponent).Cast<ForceValueComponent>().ToList();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (Alternative alt in alternatives)
            {

                //locate the header cell for the current alternative, if it exsists
                ForceValueComponent altValue = (ForceValueComponent)Children.FirstOrDefault(c => c is ForceValueComponent && !c.Deleted && (((ForceValueComponent)c).ForceAlternativeId == alt.Id));
                //if a deleted shape is present, there is no possiblity that we are adding an alternative. Furthermore, the deleted shape still represents an alternative, for each thus no second cell should be added!
                if ((altValue == null) && Children.All(c => !c.Deleted))
                {
                    alreadyThere.Add(new ForceValueComponent(Page, alt.Id, alt.IdentifierString, Index));
                }
            }

            //at this point, all alternatives have a component in alreadyThere, but there might be components of removed alternatives in there as well
            List<ForceValueComponent> toRemove = alreadyThere.Where(f => !f.Deleted && !alternatives.ToList().Any(alt => alt.Id == f.ForceAlternativeId)).ToList();
            List<ForceValueComponent> toRemoveFromTree = alreadyThere.Where(f => f.Deleted || !alternatives.ToList().Any(alt => alt.Id == f.ForceAlternativeId)).ToList();
            alreadyThere.RemoveAll(a => toRemoveFromTree.Contains(a));
            //finally, order the alternative columns similar to the alternatives container
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                alreadyThere = alreadyThere.OrderBy(fc => alternatives.IndexOf(alternatives.First(a => a.Id == fc.ForceAlternativeId))).ToList();
            }
            Children.RemoveAll(c => c is ForceValueComponent);
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

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            if (ForceConcernComponent.IsForceConcern(s.Name))
            {
                ForceConcernComponent com = new ForceConcernComponent(Page, s);
                if (com.Index == Index)
                {
                    Children.Add(com);
                }
            }
            else if (ForceDescriptionComponent.IsForceDescription(s.Name))
            {
                ForceDescriptionComponent com = new ForceDescriptionComponent(Page, s);
                if (com.Index == Index)
                {
                    Children.Add(com);
                }
            }
            else if (ForceValueComponent.IsForceValue(s.Name))
            {
                ForceValueComponent com = new ForceValueComponent(Page, s);
                if (com.Index == Index)
                {
                    Children.Add(com);
                }
            }
        }

        public void SetForceIdentifier(int forceIndex)
        {
            Children.ForEach(c => c.Index = forceIndex);
            Index = forceIndex;
            InitStyle();
        }

        public static bool IsForceContainer(string name) => ForceContaineRegex.IsMatch(name);
    }
}
