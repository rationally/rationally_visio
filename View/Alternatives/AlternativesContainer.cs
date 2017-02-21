using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.EventHandlers;
using Rationally.Visio.Forms;
using Rationally.Visio.Model;

namespace Rationally.Visio.View.Alternatives
{
    public class AlternativesContainer : RationallyContainer
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex AlternativesRegex = new Regex(@"Alternatives(\.\d+)?$");
        
        public AlternativesContainer(Page page, Shape alternativesContainer) : base(page)
        {
            RShape = alternativesContainer;
            Array ident = alternativesContainer.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsExcludeNested);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();
            foreach (Shape shape in shapes.Where(shape => AlternativeContainer.IsAlternativeContainer(shape.Name)))
            {
                Children.Add(new AlternativeContainer(page, shape));
            }
            Children = Children.OrderBy(c => c.AlternativeIndex).ToList();
            LayoutManager = new VerticalStretchLayout(this);
            InitStyle();
        }

        private void InitStyle() => UsedSizingPolicy = SizingPolicy.ExpandYIfNeeded;

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            //make s into an rcomponent for access to wrapper
            RationallyComponent shapeComponent = new RationallyComponent(Page) {RShape = s};

            if (AlternativeContainer.IsAlternativeContainer(s.Name))
            {
                if (Children.All(c => c.AlternativeIndex != shapeComponent.AlternativeIndex)) //there is no forcecontainer stub with this index
                {
                    Children.Add(new AlternativeContainer(Page, s));
                }
                else
                {
                    //remove stub, insert s as new containers
                    AlternativeStubContainer stub = (AlternativeStubContainer) Children.First(c => c.AlternativeIndex == shapeComponent.AlternativeIndex);
                    Children.Remove(stub);
                    AlternativeContainer con = new AlternativeContainer(Page, s);
                    if (Children.Count < con.AlternativeIndex)
                    {
                        Children.Add(con);
                    }
                    else
                    {
                        Children.Insert(con.AlternativeIndex, con);
                    }
                    
                }
            }
            else
            {
                bool isAlternativeChild = AlternativeStateComponent.IsAlternativeState(s.Name) || AlternativeIdentifierComponent.IsAlternativeIdentifier(s.Name) || AlternativeTitleComponent.IsAlternativeTitle(s.Name) || AlternativeDescriptionComponent.IsAlternativeDescription(s.Name);

                if (isAlternativeChild && Children.All(c => c.AlternativeIndex != shapeComponent.AlternativeIndex)) //if parent not exists
                {
                    AlternativeStubContainer stub = new AlternativeStubContainer(Page, shapeComponent.AlternativeIndex);
                    Children.Insert(stub.AlternativeIndex, stub);
                    Children.ForEach(r => r.AddToTree(s, allowAddOfSubpart));
                }
                else
                {
                    Children.ForEach(r => r.AddToTree(s, allowAddOfSubpart));
                }
            }
        }

        public static bool IsAlternativesContainer(string name) => AlternativesRegex.IsMatch(name);

        public void RemoveAlternativesFromModel()
        {
            //Get a list of all alternativeIndices
            List<int> indexList = Children.Select(alternative => alternative.AlternativeIndex).ToList();
            indexList.Sort();
            indexList.Reverse(); //Reverse so indices don't change
            foreach (int index in indexList)
            {
                Globals.RationallyAddIn.Model.Alternatives.RemoveAt(index);
            }
        }

        public void AddAlternative(string title, string state)
        {
            PleaseWait pleaseWait = new PleaseWait();
            pleaseWait.Show();
            pleaseWait.Refresh();
            Alternative newAlternative = new Alternative(title, state);
            newAlternative.GenerateIdentifier(Globals.RationallyAddIn.Model.Alternatives.Count);
            Globals.RationallyAddIn.Model.Alternatives.Add(newAlternative);
            Children.Add(new AlternativeContainer(Globals.RationallyAddIn.Application.ActivePage, Globals.RationallyAddIn.Model.Alternatives.Count - 1, newAlternative));
            RepaintHandler.Repaint();
            pleaseWait.Hide();
        }
        
        public override void Repaint()
        {
            if (Globals.RationallyAddIn.Model.Alternatives.Count > Children.Count)
            {
                Globals.RationallyAddIn.Model.Alternatives
                    .Where(alt => Children.Count == 0 || Children.All(c => c.Id != alt.Id)).ToList()
                    .ForEach(alt => { alt.GenerateIdentifier(Children.Count);
                        Children.Add(new AlternativeContainer(Globals.RationallyAddIn.Application.ActivePage, Children.Count, alt));
                    });
            }
            base.Repaint();
        }
    }
}
