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
            Shape = alternativesContainer;
            Array ident = alternativesContainer.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsExcludeNested);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();
            foreach (Shape shape in shapes.Where(shape => AlternativeShape.IsAlternativeContainer(shape.Name)))
            {
                Children.Add(new AlternativeShape(page, shape));
            }
            Children = Children.OrderBy(c => c.Index).ToList();
            LayoutManager = new VerticalStretchLayout(this);
            InitStyle();
        }

        private void InitStyle() => UsedSizingPolicy = SizingPolicy.ExpandYIfNeeded;

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            //make s into an rcomponent for access to wrapper
            VisioShape shapeComponent = new VisioShape(Page) {Shape = s};

            if (AlternativeShape.IsAlternativeContainer(s.Name))
            {
                if (Children.All(c => c.Index != shapeComponent.Index)) //there is no forcecontainer stub with this index
                {
                    Children.Add(new AlternativeShape(Page, s));
                }
                else
                {
                    //remove stub, insert s as new containers
                    AlternativeStubContainer stub = (AlternativeStubContainer) Children.First(c => c.Index == shapeComponent.Index);
                    Children.Remove(stub);
                    AlternativeShape con = new AlternativeShape(Page, s);
                    if (Children.Count < con.Index)
                    {
                        Children.Add(con);
                    }
                    else
                    {
                        Children.Insert(con.Index, con);
                    }
                    
                }
            }
            else
            {
                bool isAlternativeChild = AlternativeStateShape.IsAlternativeState(s.Name) || AlternativeIdentifierShape.IsAlternativeIdentifier(s.Name) || AlternativeTitleComponent.IsAlternativeTitle(s.Name) || AlternativeDescriptionShape.IsAlternativeDescription(s.Name);

                if (isAlternativeChild && Children.All(c => c.Index != shapeComponent.Index)) //if parent not exists
                {
                    AlternativeStubContainer stub = new AlternativeStubContainer(Page, shapeComponent.Index);
                    Children.Insert(stub.Index, stub);
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
            List<int> indexList = Children.Select(alternative => alternative.Index).ToList();
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
            Log.Debug("About to create Alternative model object");
            Alternative newAlternative = new Alternative(title, state);
            newAlternative.GenerateIdentifier(Globals.RationallyAddIn.Model.Alternatives.Count);
            Log.Debug("Identifier generated");
            Globals.RationallyAddIn.Model.Alternatives.Add(newAlternative);
            Children.Add(new AlternativeShape(Globals.RationallyAddIn.Application.ActivePage, Globals.RationallyAddIn.Model.Alternatives.Count - 1, newAlternative));
            RepaintHandler.Repaint();
            pleaseWait.Hide();
        }
        
        public override void Repaint()
        {
            //remove alternatives that are no longer in the model, but still in the view
            List<VisioShape> toDelete = Children.Where(alternative => !Globals.RationallyAddIn.Model.Alternatives.Select(alt => alt.Id).Contains(alternative.Id)).ToList();


            if (Globals.RationallyAddIn.Model.Alternatives.Count > Children.Count)
            {
                Globals.RationallyAddIn.Model.Alternatives
                    .Where(alt => Children.Count == 0 || Children.All(c => c.Id != alt.Id)).ToList()
                    .ForEach(alt => { alt.GenerateIdentifier(Children.Count);
                        Children.Add(new AlternativeShape(Globals.RationallyAddIn.Application.ActivePage, Children.Count, alt));
                    });
            }


            toDelete.ForEach(alt => alt.Shape.Delete());
            base.Repaint();
        }
    }
}
