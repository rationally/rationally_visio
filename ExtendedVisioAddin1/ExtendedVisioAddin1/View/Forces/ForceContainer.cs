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
    internal class ForceContainer : HeaderlessContainer
    {
        private static readonly Regex ForceContaineRegex = new Regex(@"ForceContainer(\.\d+)?$");


        public ForceContainer(Page page, int forceIndex) : base(page)
        {
            AddUserRow("forceIndex");
            ForceIndex = forceIndex;

            ForceConcernComponent concern = new ForceConcernComponent(page, forceIndex);
            Children.Add(concern);

            ForceDescriptionComponent description = new ForceDescriptionComponent(page, forceIndex);
            Children.Add(description);

            AddUserRow("rationallyType");
            RationallyType = "forceContainer";
            Name = "ForceContainer";

            AddAction("addForce", "QUEUEMARKEREVENT(\"add\")", "\"Add force\"", false);
            AddAction("deleteForce", "QUEUEMARKEREVENT(\"delete\")", "\"Delete this force\"", false);

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
                    if(ForceIndex <= Globals.ThisAddIn.Model.Forces.Count)
                    {
                        Globals.ThisAddIn.Model.Forces.Insert(ForceIndex, new Force(concern, description));
                    }
                    else
                    {
                        Globals.ThisAddIn.Model.Forces.Add(new Force(concern, description));
                    }
                }
            }
            InitStyle();
        }

        private void InitStyle()
        {
            UsedSizingPolicy |= SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkYIfNeeded;
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
            UpdateReorderFunctions();


            //foreach alternative in model { add a force value component, if it is not aleady there }
            List<Alternative> alternatives = Globals.ThisAddIn.Model.Alternatives;

            List<ForceValueComponent> alreadyThere = Children.Where(c => c is ForceValueComponent).Cast<ForceValueComponent>().ToList();
            foreach (Alternative alt in alternatives)
            {
                if (Children.Where(c => c is ForceValueComponent && ((ForceValueComponent)c).AlternativeIdentifier == alt.Identifier).ToList().Count != 1)
                {
                    alreadyThere.Add(new ForceValueComponent(Page, alt.Identifier, this.ForceIndex));
                }
            }

            //at this point, all alternatives have a component in alreadyThere, but there might be components of removed alternatives in there as well
            List<ForceValueComponent> toRemove = alreadyThere.Where(f => !alternatives.ToList().Any(alt => alt.Identifier == f.AlternativeIdentifier)).ToList();
            

            alreadyThere = alreadyThere.Where(f => alternatives.ToList().Any(alt => alt.Identifier == f.AlternativeIdentifier)).ToList();

            //finally, order the alternative columns similar to the alternatives container
            alreadyThere = alreadyThere.OrderBy(fc => alternatives.IndexOf(alternatives.First(a => a.Identifier == fc.AlternativeIdentifier))).ToList();

            Children.RemoveAll(c => c is ForceValueComponent);
            Children.AddRange(alreadyThere);

            //remove the shapes of the deleted components
            toRemove.ForEach(c => c.RShape.DeleteEx(0));

            base.Repaint();
        }

        public static bool IsForceContainer(string name)
        {
            return ForceContaineRegex.IsMatch(name);
        }
    }
}
