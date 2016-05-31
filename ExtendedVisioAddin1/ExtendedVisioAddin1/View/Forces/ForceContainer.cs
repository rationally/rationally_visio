using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
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
            this.Children.Add(concern);

            ForceDescriptionComponent description = new ForceDescriptionComponent(page);
            this.Children.Add(description);

            this.AddUserRow("rationallyType");
            this.RationallyType = "forceContainer";
            this.Name = "ForceContainer";
            this.Height = 0.33;
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
                    this.Children.Add(new ForceConcernComponent(page, shape));
                } else if (ForceDescriptionComponent.IsForceDescription(shape.Name))
                {
                    this.Children.Add(new ForceDescriptionComponent(page, shape));
                } else if (ForceValueComponent.IsForceValue(shape.Name))
                {
                    this.Children.Add(new ForceValueComponent(page, shape));
                }
            }
        }

        private void InitStyle()
        {
            this.UsedSizingPolicy |= SizingPolicy.ExpandXIfNeeded;
            this.LayoutManager = new InlineLayout(this);
        }

        [SuppressMessage("ReSharper", "SimplifyLinqExpression")]
        public override void Repaint()
        {

            //foreach alternative in model { add a force value component, if it is not aleady there }
            ObservableCollection<Alternative> alternatives = Globals.ThisAddIn.Model.Alternatives;

            List<ForceValueComponent> alreadyThere = this.Children.Where(c => c is ForceValueComponent).Cast<ForceValueComponent>().ToList();
            for (int i = 0; i < alternatives.Count; i++)
            {
                if (this.Children.Where(c => c is ForceValueComponent && ((ForceValueComponent)c).AlternativeIdentifier == alternatives[i].Identifier).ToList().Count != 1)
                {
                    alreadyThere.Add(new ForceValueComponent(Globals.ThisAddIn.Application.ActivePage, alternatives[i].Identifier));
                }
            }

            //at this point, all alternatives have a component in alreadyThere, but there might be components of removed alternatives in there as well
            alreadyThere = alreadyThere.Where(f => alternatives.ToList().Any(alt => alt.Identifier == f.AlternativeIdentifier)).ToList();

            //finally, order the alternative columns similar to the alternatives container
            alreadyThere = alreadyThere.OrderBy(fc => alternatives.IndexOf(alternatives.First(a => a.Identifier == fc.AlternativeIdentifier))).ToList();

            //remove the current force values from the force row and insert the new sorted list
            this.Children.RemoveAll(c => c is ForceValueComponent);

            this.Children.AddRange(alreadyThere);


            base.Repaint();
        }

        public static bool IsForceContainer(string name)
        {
            return ForceContaineRegex.IsMatch(name);
        }
    }
}
