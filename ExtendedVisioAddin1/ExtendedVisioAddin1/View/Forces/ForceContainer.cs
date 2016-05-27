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

        public ForceContainer(Page page) : base(page)
        {
            ForceConcernComponent concern = new ForceConcernComponent(page);
            Children.Add(concern);

            ForceDescriptionComponent description = new ForceDescriptionComponent(page);
            Children.Add(description);

            AddUserRow("rationallyType");
            RationallyType = "forceContainer";
            Name = "ForceContainer";
            Height = 0.33;
            UsedSizingPolicy |= SizingPolicy.ExpandXIfNeeded;
            InitStyle();
        }

        public ForceContainer(Page page, Shape forceContainer) : base(page)
        {
            RShape = forceContainer;
            Array ident = forceContainer.ContainerProperties.GetMemberShapes(16);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();
            foreach (Shape shape in shapes)
            {
                if (ForceConcernComponent.IsForceConcern(shape.Name))
                {
                    Children.Add(new ForceConcernComponent(page, shape));
                } else if (ForceDescriptionComponent.IsForceDescription(shape.Name))
                {
                    Children.Add(new ForceDescriptionComponent(page, shape));
                } else if (ForceValueComponent.IsForceValue(shape.Name))
                {
                    Children.Add(new ForceValueComponent(page, shape));
                }
            }
        }

        private void InitStyle()
        {
            UsedSizingPolicy |= (UsedSizingPolicy & SizingPolicy.ExpandXIfNeeded);
            LayoutManager = new InlineLayout(this);
        }

        [SuppressMessage("ReSharper", "SimplifyLinqExpression")]
        public override void Repaint()
        {

            //foreach alternative in model { add a force value component, if it is not aleady there }
            ObservableCollection<Alternative> alternatives = Globals.ThisAddIn.Model.Alternatives;

            List<ForceValueComponent> alreadyThere = Children.Where(c => c is ForceValueComponent).Cast<ForceValueComponent>().ToList();
            foreach (Alternative alt in alternatives)
            {
                if (Children.Where(c => c is ForceValueComponent && ((ForceValueComponent)c).AlternativeIdentifier == alt.Identifier).ToList().Count != 1)
                {
                    alreadyThere.Add(new ForceValueComponent(Globals.ThisAddIn.Application.ActivePage, alt.Identifier));
                }
            }

            //at this point, all alternatives have a component in alreadyThere, but there might be components of removed alternatives in there as well
            alreadyThere = alreadyThere.Where(f => alternatives.ToList().Any(alt => alt.Identifier == f.AlternativeIdentifier)).ToList();

            //finally, order the alternative columns similar to the alternatives container
            alreadyThere = alreadyThere.OrderBy(fc => alternatives.IndexOf(alternatives.First(a => a.Identifier == fc.AlternativeIdentifier))).ToList();

            //remove the current force values from the force row and insert the new sorted list
            Children.RemoveAll(c => c is ForceValueComponent);

            Children.AddRange(alreadyThere);


            base.Repaint();
        }

        public static bool IsForceContainer(string name)
        {
            return ForceContaineRegex.IsMatch(name);
        }
    }
}
